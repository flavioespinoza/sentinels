using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SolBotSentinel.Domain;
using SolBotSentinel.Rules;
using SolBotSentinel.Sources;

namespace SolBotSentinel;

/// <summary>
/// Flavio, Oct 27 2026: the judge proof. After the first page is out, a model reads the engine
/// log and sends ONE more page that says why. The judge only explains. It is given the alert and
/// the notifier and nothing else that can act: no AlertStore, no engine command route.
/// </summary>
public interface IJudgeClient
{
	/// <summary>Never throws. Callers do not await it on the alert path.</summary>
	Task JudgeAndPageAsync(Alert alert, CancellationToken ct);
}

/// <summary>Used when the judge is off.</summary>
public sealed class NullJudge : IJudgeClient
{
	public Task JudgeAndPageAsync(Alert alert, CancellationToken ct) => Task.CompletedTask;
}

/// <summary>One line of the engine log, as GET /sentinel-log returns it.</summary>
public sealed record EngineLogLine(long At, string Level, string Line);

public sealed record EngineLog(long Since, long Now, List<EngineLogLine> Lines);

public sealed record JudgeEvidence(string At, string Line);

/// <summary>The judge's answer, already validated by the judge service.</summary>
public sealed record JudgeVerdict(
	string Verdict,
	string Cause,
	List<JudgeEvidence> Evidence,
	string Action,
	double Confidence);

public sealed class JudgeClient(
	IHttpClientFactory clients,
	INotifier notifier,
	IPositionSource source,
	ITrendSource trends,
	IOptions<WatchOptions> options,
	TimeProvider clock,
	ILogger<JudgeClient> logger) : IJudgeClient
{
	public const string VerdictReasonSuffix = ":verdict";
	private static readonly TimeSpan Hour = TimeSpan.FromHours(1);
	private static readonly TimeSpan LogWindow = TimeSpan.FromMinutes(15);
	private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

	private readonly object _gate = new();
	private readonly Queue<DateTimeOffset> _calls = new();
	private readonly Dictionary<string, DateTimeOffset> _lastByKey = [];

	private WatchOptions Opt => options.Value;

	public async Task JudgeAndPageAsync(Alert alert, CancellationToken ct)
	{
		try
		{
			if (alert.Reason != TrendRules.FlipNotClosed) return;
			if (!Admit(alert)) return;

			var detail = await JudgeAsync(alert, ct);
			if (detail is null) return;
			await notifier.NotifyAsync(
				alert with { Reason = alert.Reason + VerdictReasonSuffix, Detail = detail }, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "The judge failed for {Bot} {Reason}", alert.BotId, alert.Reason);
		}
	}

	/// <summary>
	/// The caps, checked and recorded in one step before anything is awaited: at most JudgeMaxPerHour
	/// calls in a rolling hour across all bots, and one per bot and reason per RepageMs.
	/// </summary>
	private bool Admit(Alert alert)
	{
		var now = clock.GetUtcNow();
		var key = alert.BotId + "|" + alert.Reason;
		lock (_gate)
		{
			while (_calls.Count > 0 && now - _calls.Peek() >= Hour) _calls.Dequeue();
			if (_calls.Count >= Opt.JudgeMaxPerHour) return false;
			if (Opt.RepageMs > 0 && _lastByKey.TryGetValue(key, out var last)
				&& now - last < TimeSpan.FromMilliseconds(Opt.RepageMs)) return false;
			_calls.Enqueue(now);
			_lastByKey[key] = now;
			return true;
		}
	}

	/// <summary>The Detail for the second page, or null when the caller cancelled.</summary>
	private async Task<string?> JudgeAsync(Alert alert, CancellationToken ct)
	{
		var (snapshot, trend) = await ReadContextAsync(alert.BotId, ct);

		EngineLog log;
		try
		{
			log = await FetchLogAsync(ct);
		}
		catch (Exception ex) when (!ct.IsCancellationRequested)
		{
			return $"judge unavailable: engine log {Why(ex)}";
		}

		try
		{
			using var deadline = Deadline(ct);
			var body = new { alert, snapshot, trend, logLines = log.Lines };
			using var response = await clients.CreateClient().PostAsJsonAsync(Opt.JudgeUrl, body, Json, deadline.Token);
			response.EnsureSuccessStatusCode();
			var verdict = await response.Content.ReadFromJsonAsync<JudgeVerdict>(Json, deadline.Token);
			return verdict is { Evidence: not null, Verdict: not null, Cause: not null, Action: not null }
				? Format(verdict)
				: "judge unavailable: empty answer";
		}
		catch (Exception ex) when (!ct.IsCancellationRequested)
		{
			return $"judge unavailable: {Why(ex)}";
		}
	}

	/// <summary>cause | evidence | verdict | action | confidence, in that order.</summary>
	public static string Format(JudgeVerdict v)
	{
		var evidence = v.Evidence.Count == 0
			? "none"
			: string.Join("; ", v.Evidence.Select(e => $"{e.At} {e.Line}"));
		return $"cause: {v.Cause} | evidence: {evidence} | verdict {v.Verdict} | action {v.Action}"
			+ $" | confidence {v.Confidence.ToString("0.00", CultureInfo.InvariantCulture)}";
	}

	private async Task<EngineLog> FetchLogAsync(CancellationToken ct)
	{
		if (!Uri.TryCreate(Opt.FeedUrl, UriKind.Absolute, out var feed))
		{
			throw new InvalidOperationException("no feed url");
		}
		var since = (clock.GetUtcNow() - LogWindow).ToUnixTimeMilliseconds();
		var url = $"{feed.GetLeftPart(UriPartial.Authority)}/sentinel-log?since={since}";

		using var deadline = Deadline(ct);
		using var request = new HttpRequestMessage(HttpMethod.Get, url);
		if (Opt.FeedToken != "") request.Headers.Authorization = new("Bearer", Opt.FeedToken);
		using var response = await clients.CreateClient().SendAsync(request, deadline.Token);
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadFromJsonAsync<EngineLog>(Json, deadline.Token)
			?? throw new InvalidOperationException("empty log");
	}

	/// <summary>The bot's snapshot and the trend, for the judge to read. Either may be missing.</summary>
	private async Task<(PositionSnapshot?, TrendReading?)> ReadContextAsync(string botId, CancellationToken ct)
	{
		PositionSnapshot? snapshot = null;
		TrendReading? trend = null;
		try
		{
			using var deadline = Deadline(ct);
			snapshot = (await source.ReadAsync(deadline.Token)).FirstOrDefault(p => p.BotId == botId);
			trend = await trends.ReadAsync(deadline.Token);
		}
		catch (Exception ex)
		{
			logger.LogWarning(ex, "The judge could not read the feed for {Bot}", botId);
		}
		return (snapshot, trend);
	}

	private CancellationTokenSource Deadline(CancellationToken ct)
	{
		var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
		cts.CancelAfter(TimeSpan.FromMilliseconds(Math.Max(1, Opt.JudgeTimeoutMs)));
		return cts;
	}

	private static string Why(Exception ex) => ex switch
	{
		OperationCanceledException => "timeout",
		HttpRequestException { StatusCode: { } code } => $"HTTP {(int)code}",
		HttpRequestException => "unreachable",
		JsonException => "bad answer",
		_ => "error",
	};
}
