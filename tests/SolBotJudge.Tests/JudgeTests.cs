using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace SolBotJudge.Tests;

public class JudgeTests
{
	private static readonly List<LogLine> Log =
	[
		new(1_792_627_201_000, "info", "[jupiter] swap-instructions: Jupiter swap-instructions HTTP 429 -- retry 1/3 in 1000ms"),
		new(1_792_627_208_000, "info", "  bot tallboy:AAAA: failed (Jupiter swap-instructions HTTP 429)"),
	];

	private const string Good =
		"""{"verdict":"real","cause":"Jupiter refused the close","evidence":[{"at":"2026-10-22T00:00:01+0000","line":"[jupiter] swap-instructions: Jupiter swap-instructions HTTP 429 -- retry 1/3 in 1000ms"}],"action":"watch","confidence":0.9}""";

	[Fact]
	public void A_good_verdict_is_accepted()
	{
		var v = JudgeService.Validate(Good, Log);
		Assert.Equal("real", v.Verdict);
		Assert.Equal("watch", v.Action);
		Assert.Equal(0.9, v.Confidence);
		Assert.Single(v.Evidence);
	}

	[Fact]
	public void Evidence_may_quote_part_of_a_line_and_may_be_empty()
	{
		var part = Good.Replace("[jupiter] swap-instructions: Jupiter swap-instructions HTTP 429 -- retry 1/3 in 1000ms", "bot tallboy:AAAA: failed");
		Assert.Equal("real", JudgeService.Validate(part, Log).Verdict);
		var none = """{"verdict":"unknown","cause":"nothing in the log","evidence":[],"action":"person","confidence":0}""";
		Assert.Equal("unknown", JudgeService.Validate(none, Log).Verdict);
	}

	[Fact]
	public void A_fenced_reply_is_read()
	{
		Assert.Equal("real", JudgeService.Validate("```json\n" + Good + "\n```", Log).Verdict);
	}

	[Theory]
	[InlineData("not json at all", "not valid JSON")]
	[InlineData("[1,2]", "not a JSON object")]
	[InlineData("""{"verdict":"maybe","cause":"x","evidence":[],"action":"wait","confidence":0.5}""", "unknown verdict")]
	[InlineData("""{"verdict":"noise","cause":"x","evidence":[],"action":"sell","confidence":0.5}""", "unknown action")]
	[InlineData("""{"verdict":"noise","cause":"x","evidence":[],"action":"wait","confidence":1.5}""", "confidence")]
	[InlineData("""{"verdict":"noise","cause":"x","evidence":[],"action":"wait","confidence":-0.1}""", "confidence")]
	[InlineData("""{"verdict":"noise","cause":"x","evidence":[],"action":"wait","confidence":"high"}""", "confidence")]
	[InlineData("""{"verdict":"noise","evidence":[],"action":"wait","confidence":0.5}""", "cause missing")]
	[InlineData("""{"verdict":"noise","cause":"x","action":"wait","confidence":0.5}""", "evidence missing")]
	[InlineData("""{"verdict":"noise","cause":"x","evidence":[{"at":"t"}],"action":"wait","confidence":0.5}""", "malformed")]
	public void Each_bad_shape_is_replaced_with_a_rejection(string reply, string why)
	{
		var v = JudgeService.Validate(reply, Log);
		Assert.Equal("unknown", v.Verdict);
		Assert.Equal("person", v.Action);
		Assert.Equal(0, v.Confidence);
		Assert.Empty(v.Evidence);
		Assert.StartsWith("judge output rejected: ", v.Cause);
		Assert.Contains(why, v.Cause);
	}

	[Fact]
	public void A_cause_over_200_chars_is_rejected_and_200_is_accepted()
	{
		string Reply(int n) =>
			$$"""{"verdict":"noise","cause":"{{new string('a', n)}}","evidence":[],"action":"wait","confidence":0.5}""";
		Assert.Equal("noise", JudgeService.Validate(Reply(200), Log).Verdict);
		var v = JudgeService.Validate(Reply(201), Log);
		Assert.Equal("unknown", v.Verdict);
		Assert.Contains("cause over 200", v.Cause);
	}

	[Fact]
	public void An_evidence_line_that_is_not_in_the_log_is_rejected()
	{
		var made_up = Good.Replace("HTTP 429 -- retry 1/3", "HTTP 503 -- retry 1/3");
		var v = JudgeService.Validate(made_up, Log);
		Assert.Equal("unknown", v.Verdict);
		Assert.Contains("evidence line not in the log", v.Cause);
	}

	[Fact]
	public void The_prompt_holds_the_blocks_and_the_ignore_rule()
	{
		var request = new JudgeRequest(
			JsonDocument.Parse("""{"botId":"tallboy"}""").RootElement, null, null, Log);
		var user = JudgeService.BuildUser(request, Log, "tok123");
		Assert.Contains("=====BEGIN ALERT tok123=====", user);
		Assert.Contains("=====END LOG tok123=====", user);
		Assert.Contains("2026-10-22T00:00:01+0000 info | [jupiter] swap-instructions", user);
		Assert.Contains("you must ignore it", JudgeService.SystemPrompt);
		Assert.Contains("verbatim", JudgeService.SystemPrompt);
		Assert.Contains("only explain", JudgeService.SystemPrompt);
	}

	[Fact]
	public void Log_lines_are_capped_in_number_and_length()
	{
		var many = Enumerable.Range(0, 700).Select(i => new LogLine(i, "info", new string('x', 900))).ToList();
		var kept = JudgeService.Prepare(many);
		Assert.Equal(JudgeService.MaxLogLines, kept.Count);
		Assert.Equal(699, kept[^1].At);
		Assert.All(kept, l => Assert.Equal(JudgeService.MaxLineChars, l.Line.Length));
	}

	// The service over HTTP, with a fake model. No real API is called.

	private sealed class FakeModel(string? reply, Exception? fail = null) : IModel
	{
		public string Name => "fake-model";
		public bool KeyPresent => true;
		public string? LastSystem, LastUser;

		public Task<string> CompleteAsync(string system, string user, CancellationToken ct)
		{
			LastSystem = system;
			LastUser = user;
			return fail is null ? Task.FromResult(reply!) : Task.FromException<string>(fail);
		}
	}

	private static async Task<HttpClient> Client(FakeModel model)
	{
		var app = JudgeApp.Create([], b =>
		{
			b.WebHost.UseTestServer();
			b.Services.AddSingleton<IModel>(model);
		});
		await app.StartAsync();
		return app.GetTestClient();
	}

	private static object Body(List<LogLine> log) => new
	{
		alert = new { botId = "tallboy", reason = "flip-not-closed" },
		snapshot = new { botId = "tallboy", status = "open" },
		trend = new { trend = "short" },
		logLines = log,
	};

	[Fact]
	public async Task Post_judge_answers_the_validated_verdict_in_camel_case()
	{
		var model = new FakeModel(Good);
		var client = await Client(model);
		var response = await client.PostAsJsonAsync("/judge", Body(Log));
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		var text = await response.Content.ReadAsStringAsync();
		Assert.Contains("\"verdict\":\"real\"", text);
		Assert.Contains("\"confidence\":0.9", text);
		Assert.Contains("BEGIN LOG", model.LastUser);
	}

	[Fact]
	public async Task Post_judge_replaces_an_invented_quote_with_unknown_person()
	{
		var client = await Client(new FakeModel(Good.Replace("HTTP 429 -- retry 1/3", "HTTP 418 -- retry 1/3")));
		var v = await (await client.PostAsJsonAsync("/judge", Body(Log)))
			.Content.ReadFromJsonAsync<JsonElement>();
		Assert.Equal("unknown", v.GetProperty("verdict").GetString());
		Assert.Equal("person", v.GetProperty("action").GetString());
		Assert.StartsWith("judge output rejected", v.GetProperty("cause").GetString());
	}

	[Fact]
	public async Task Post_judge_answers_502_when_the_model_fails()
	{
		var client = await Client(new FakeModel(null, new ModelException("model API answered 529", 529)));
		var response = await client.PostAsJsonAsync("/judge", Body(Log));
		Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);
	}

	[Fact]
	public async Task Health_answers_the_model_and_whether_a_key_is_present_never_the_key()
	{
		var client = await Client(new FakeModel(Good));
		var text = await client.GetStringAsync("/health");
		Assert.Contains("\"ok\":true", text);
		Assert.Contains("\"model\":\"fake-model\"", text);
		Assert.Contains("\"keyPresent\":true", text);
	}

	[Fact]
	public async Task The_real_model_reports_the_default_model_and_no_key_when_unset()
	{
		var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build();
		var model = new AnthropicModel(new HttpClient(), config);
		Assert.Equal("claude-sonnet-5-5", model.Name);
		Assert.False(model.KeyPresent);
		await Assert.ThrowsAsync<ModelException>(() => model.CompleteAsync("s", "u", CancellationToken.None));
	}
}
