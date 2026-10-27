using System.Text.Json;

namespace SolBotJudge;

/// <summary>
/// The judge service. Flavio, Oct 27 2026: the judge proof. It has no route that acts, no
/// database, and no tools: it reads one request and answers one verdict.
/// </summary>
public static class JudgeApp
{
	public static WebApplication Create(string[] args, Action<WebApplicationBuilder>? configure = null)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
		builder.Services.AddHttpClient<AnthropicModel>(c => c.Timeout = TimeSpan.FromSeconds(20));
		builder.Services.AddSingleton<IModel>(sp => sp.GetRequiredService<AnthropicModel>());
		builder.Services.AddSingleton<JudgeService>();
		configure?.Invoke(builder);

		var app = builder.Build();

		app.MapGet("/health", (IModel model) => Results.Ok(new { ok = true, model = model.Name, keyPresent = model.KeyPresent }));

		app.MapPost("/judge", async (JudgeRequest? request, JudgeService judge, CancellationToken ct) =>
		{
			if (request is null) return Results.BadRequest(new { error = "no request body" });
			try
			{
				return Results.Ok(await judge.JudgeAsync(request, ct));
			}
			catch (ModelException ex)
			{
				return Results.Json(new { error = ex.Message }, statusCode: StatusCodes.Status502BadGateway);
			}
			catch (HttpRequestException)
			{
				return Results.Json(new { error = "model unreachable" }, statusCode: StatusCodes.Status502BadGateway);
			}
			catch (OperationCanceledException) when (!ct.IsCancellationRequested)
			{
				return Results.Json(new { error = "model timeout" }, statusCode: StatusCodes.Status504GatewayTimeout);
			}
		});

		return app;
	}
}
