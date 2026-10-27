using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SolBotJudge;

/// <summary>
/// Flavio, Oct 27 2026: the judge proof. The Messages API over HttpClient, no tools, one user
/// turn. The key comes from the environment and is never logged or returned.
/// </summary>
public sealed class AnthropicModel(HttpClient http, IConfiguration config) : IModel
{
	public const string DefaultModel = "claude-sonnet-5-5";
	public const string Endpoint = "https://api.anthropic.com/v1/messages";

	public string Name => config["Judge:Model"] is { Length: > 0 } m ? m : DefaultModel;
	public bool KeyPresent => !string.IsNullOrEmpty(config["ANTHROPIC_API_KEY"]);

	public async Task<string> CompleteAsync(string system, string user, CancellationToken ct)
	{
		var key = config["ANTHROPIC_API_KEY"];
		if (string.IsNullOrEmpty(key)) throw new ModelException("no ANTHROPIC_API_KEY set");

		using var request = new HttpRequestMessage(HttpMethod.Post, Endpoint);
		request.Headers.Add("x-api-key", key);
		request.Headers.Add("anthropic-version", "2023-06-01");
		request.Content = JsonContent.Create(new
		{
			model = Name,
			max_tokens = 600,
			system,
			messages = new[] { new { role = "user", content = user } },
		});

		using var response = await http.SendAsync(request, ct);
		if (!response.IsSuccessStatusCode)
		{
			throw new ModelException($"model API answered {(int)response.StatusCode}", (int)response.StatusCode);
		}

		using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync(ct));
		var text = new StringBuilder();
		if (doc.RootElement.TryGetProperty("content", out var blocks) && blocks.ValueKind == JsonValueKind.Array)
		{
			foreach (var block in blocks.EnumerateArray())
			{
				if (block.TryGetProperty("type", out var t) && t.GetString() == "text"
					&& block.TryGetProperty("text", out var x)) text.Append(x.GetString());
			}
		}
		return text.Length > 0 ? text.ToString() : throw new ModelException("model reply had no text");
	}
}
