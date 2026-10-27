using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SolBotSentinel.Domain;
using SolBotSentinel.Rules;
using SolBotSentinel.Sources;
using SolBotSentinel.Watchers;

namespace SolBotSentinel.Tests;

/// <summary>Flavio, Oct 27 2026: the judge proof. No real API is called; every model and wire is a fake.</summary>
public class JudgeTests
{
	private const string FeedToken = "feed-token-for-tests";
	private static readonly DateTimeOffset T0 = new(2026, 10, 22, 0, 0, 30, TimeSpan.Zero);

	private const string QuickVerdict =
		"""{"verdict":"noise","cause":"c","evidence":[],"action":"wait","confidence":0.5}""";

	// ---- fakes ----

	private sealed class ManualClock(DateTimeOffset start) : TimeProvider
	{
		public DateTimeOffset Now = start;
		public override DateTimeOffset GetUtcNow() => Now;
	}

	private sealed class FakeNotifier(List<string>? events = null) : INotifier
	{
		public List<Alert> Pages { get; } = [];
		public bool Enabled => true;

		public Task NotifyAsync(Alert alert, CancellationToken ct)
		{
			lock (Pages) Pages.Add(alert);
			events?.Add("page:" + alert.Reason);
			return Task.CompletedTask;
		}
	}

	private sealed class FakeSource(PositionSnapshot[] bots) : IPositionSource, ITrendSource
	{
		public string Name => "fake";
		public Task<IReadOnlyList<PositionSnapshot>> ReadAsync(CancellationToken ct) =>
			Task.FromResult<IReadOnlyList<PositionSnapshot>>(bots);
		Task<TrendReading?> ITrendSource.ReadAsync(CancellationToken ct) =>
			Task.FromResult<TrendReading?>(new TrendReading { Trend = "short", Since = T0.AddMinutes(-1) });
	}

	private sealed class Handler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> on)
		: HttpMessageHandler
	{
		public List<(string Path, string Query, string Auth)> Seen { get; } = [];
		public int JudgeCalls => Seen.Count(s => s.Path == "/judge");

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage r, CancellationToken ct)
		{
			lock (Seen) Seen.Add((r.RequestUri!.AbsolutePath, r.RequestUri.Query, r.Headers.Authorization?.ToString() ?? ""));
			return on(r, ct);
		}
	}

	private sealed class OneClientFactory(HttpMessageHandler handler) : IHttpClientFactory
	{
		public HttpClient CreateClient(string name) => new(handler, disposeHandler: false);
	}

	private static Task<HttpResponseMessage> Json(string json, HttpStatusCode status = HttpStatusCode.OK) =>
		Task.FromResult(new HttpResponseMessage(status)
		{
			Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json"),
		});

	private static Task<HttpResponseMessage> EmptyLog(HttpRequestMessage r) =>
		Json("""{"since":0,"now":0,"lines":[]}""");

	private static async Task<HttpResponseMessage> Never(CancellationToken ct)
	{
		await Task.Delay(Timeout.Infinite, ct);
		return new HttpResponseMessage(HttpStatusCode.OK);
	}

	private static WatchOptions Opts(Action<WatchOptions>? tweak = null)
	{
		var o = new WatchOptions
		{
			JudgeEnabled = true,
			FeedUrl = "http://engine.test/sentinel-feed",
			FeedToken = FeedToken,
			JudgeUrl = "http://judge.test/judge",
		};
		tweak?.Invoke(o);
		return o;
	}

	private static JudgeClient Client(WatchOptions o, HttpMessageHandler h, INotifier n, TimeProvider clock) =>
		new(new OneClientFactory(h), n, new FakeSource([Bot("tallboy:AAAAAA")]), new FakeSource([]),
			Options.Create(o), clock, NullLogger<JudgeClient>.Instance);

	private static PositionSnapshot Bot(string id) => new()
	{
		BotId = id, OpensOn = "long", Status = "open", At = T0,
	};

	private static Alert FlipAlert(string bot = "tallboy:AAAAAA") => new()
	{
		BotId = bot,
		Watcher = "trend",
		Reason = TrendRules.FlipNotClosed,
		Detail = $"trend short since 2026-10-22 00:00:00Z, {bot} is open",
		At = T0,
	};

	// ---- the second page ----

	[Fact]
	public void The_defaults_are_off_20_seconds_and_10_an_hour()
	{
		var o = new WatchOptions();
		Assert.False(o.JudgeEnabled);
		Assert.Equal(20_000, o.JudgeTimeoutMs);
		Assert.Equal(10, o.JudgeMaxPerHour);
		Assert.Equal("http://127.0.0.1:8096/judge", o.JudgeUrl);
	}

	[Fact]
	public async Task A_judge_that_never_answers_gives_the_unavailable_timeout_page()
	{
		var h = new Handler((r, ct) => r.RequestUri!.AbsolutePath == "/judge" ? Never(ct) : EmptyLog(r));
		var n = new FakeNotifier();
		var client = Client(Opts(o => o.JudgeTimeoutMs = 100), h, n, new ManualClock(T0));

		await client.JudgeAndPageAsync(FlipAlert(), CancellationToken.None);

		var page = Assert.Single(n.Pages);
		Assert.Equal("flip-not-closed:verdict", page.Reason);
		Assert.Equal("judge unavailable: timeout", page.Detail);
	}

	[Fact]
	public async Task An_http_error_or_an_unreachable_judge_gives_one_unavailable_page_and_never_throws()
	{
		var down = new Handler((r, ct) => r.RequestUri!.AbsolutePath == "/judge"
			? Task.FromException<HttpResponseMessage>(new HttpRequestException("refused"))
			: EmptyLog(r));
		var err = new Handler((r, ct) => r.RequestUri!.AbsolutePath == "/judge"
			? Json("{}", HttpStatusCode.BadGateway)
			: EmptyLog(r));

		var n1 = new FakeNotifier();
		await Client(Opts(), down, n1, new ManualClock(T0)).JudgeAndPageAsync(FlipAlert(), CancellationToken.None);
		Assert.Equal("judge unavailable: unreachable", Assert.Single(n1.Pages).Detail);

		var n2 = new FakeNotifier();
		await Client(Opts(), err, n2, new ManualClock(T0)).JudgeAndPageAsync(FlipAlert(), CancellationToken.None);
		Assert.Equal("judge unavailable: HTTP 502", Assert.Single(n2.Pages).Detail);
	}

	[Fact]
	public async Task A_log_that_cannot_be_fetched_gives_one_unavailable_page_and_no_model_call()
	{
		var h = new Handler((r, ct) => Json("{}", HttpStatusCode.Unauthorized));
		var n = new FakeNotifier();
		await Client(Opts(), h, n, new ManualClock(T0)).JudgeAndPageAsync(FlipAlert(), CancellationToken.None);

		Assert.Equal("judge unavailable: engine log HTTP 401", Assert.Single(n.Pages).Detail);
		Assert.Equal(0, h.JudgeCalls);
	}

	[Fact]
	public async Task The_log_is_read_from_the_feed_origin_for_the_last_15_minutes_with_the_feed_bearer()
	{
		var h = new Handler((r, ct) => r.RequestUri!.AbsolutePath == "/judge" ? Json(QuickVerdict) : EmptyLog(r));
		var clock = new ManualClock(T0);
		await Client(Opts(), h, new FakeNotifier(), clock).JudgeAndPageAsync(FlipAlert(), CancellationToken.None);

		var log = Assert.Single(h.Seen, s => s.Path == "/sentinel-log");
		Assert.Equal($"?since={T0.AddMinutes(-15).ToUnixTimeMilliseconds()}", log.Query);
		Assert.Equal("Bearer " + FeedToken, log.Auth);
	}

	[Fact]
	public async Task Only_flip_not_closed_is_judged()
	{
		var h = new Handler((r, ct) => Json(QuickVerdict));
		var n = new FakeNotifier();
		var other = FlipAlert() with { Reason = TrendRules.FlipNotOpened };
		await Client(Opts(), h, n, new ManualClock(T0)).JudgeAndPageAsync(other, CancellationToken.None);
		Assert.Empty(n.Pages);
		Assert.Empty(h.Seen);
	}

	// ---- the caps ----

	[Fact]
	public async Task At_most_10_calls_an_hour_the_11th_sends_nothing_and_the_first_after_the_hour_sends_again()
	{
		var h = new Handler((r, ct) => r.RequestUri!.AbsolutePath == "/judge" ? Json(QuickVerdict) : EmptyLog(r));
		var n = new FakeNotifier();
		var clock = new ManualClock(T0);
		var client = Client(Opts(), h, n, clock);

		for (var i = 0; i < 10; i++) await client.JudgeAndPageAsync(FlipAlert($"bot{i}"), CancellationToken.None);
		Assert.Equal(10, n.Pages.Count);

		await client.JudgeAndPageAsync(FlipAlert("bot10"), CancellationToken.None);
		Assert.Equal(10, n.Pages.Count);
		Assert.Equal(10, h.JudgeCalls);

		clock.Now = T0.AddMinutes(59);
		await client.JudgeAndPageAsync(FlipAlert("bot10"), CancellationToken.None);
		Assert.Equal(10, n.Pages.Count);

		clock.Now = T0.AddMinutes(61);
		await client.JudgeAndPageAsync(FlipAlert("bot10"), CancellationToken.None);
		Assert.Equal(11, n.Pages.Count);
		Assert.Equal(11, h.JudgeCalls);
	}

	[Fact]
	public async Task One_call_per_bot_and_reason_per_repage_interval()
	{
		var h = new Handler((r, ct) => r.RequestUri!.AbsolutePath == "/judge" ? Json(QuickVerdict) : EmptyLog(r));
		var n = new FakeNotifier();
		var clock = new ManualClock(T0);
		var client = Client(Opts(), h, n, clock);

		await client.JudgeAndPageAsync(FlipAlert("a"), CancellationToken.None);
		clock.Now = T0.AddMinutes(10);
		await client.JudgeAndPageAsync(FlipAlert("a"), CancellationToken.None);
		await client.JudgeAndPageAsync(FlipAlert("b"), CancellationToken.None);
		Assert.Equal(2, n.Pages.Count);

		clock.Now = T0.AddMinutes(31);
		await client.JudgeAndPageAsync(FlipAlert("a"), CancellationToken.None);
		Assert.Equal(3, n.Pages.Count);
	}

	// ---- the rule: page first, never wait on the judge ----

	private sealed class Probe(
		IPositionSource source, AlertStore alerts, INotifier notifier, IJudgeClient judge,
		IOptions<WatchOptions> options, TimeProvider clock)
		: WatcherBase(source, alerts, notifier, judge, options, clock, NullLogger.Instance)
	{
		protected override string WatcherName => "trend";
		protected override void Check(PositionSnapshot snapshot) { }
		public void Trip(PositionSnapshot s, string reason) => Raise(s, reason, "detail");
	}

	private sealed class HangingJudge(List<string> events) : IJudgeClient
	{
		public int Calls;
		public Task JudgeAndPageAsync(Alert alert, CancellationToken ct)
		{
			Interlocked.Increment(ref Calls);
			events.Add("judge");
			return new TaskCompletionSource().Task;
		}
	}

	private static Probe Watcher(INotifier n, IJudgeClient j, WatchOptions o, TimeProvider clock) =>
		new(new FakeSource([]), new AlertStore(), n, j, Options.Create(o), clock);

	[Fact]
	public void The_first_page_goes_out_before_the_judge_is_called_and_a_hung_judge_does_not_block_raise()
	{
		var events = new List<string>();
		var n = new FakeNotifier(events);
		var judge = new HangingJudge(events);
		var watcher = Watcher(n, judge, Opts(), new ManualClock(T0));

		var sw = Stopwatch.StartNew();
		watcher.Trip(Bot("tallboy:AAAAAA"), TrendRules.FlipNotClosed);
		sw.Stop();

		Assert.True(sw.ElapsedMilliseconds < 1000);
		Assert.Equal(["page:flip-not-closed", "judge"], events);
	}

	[Fact]
	public async Task The_first_page_is_sent_even_when_the_real_judge_client_hangs_forever()
	{
		var n = new FakeNotifier();
		var h = new Handler((r, ct) => Never(ct));
		var o = Opts(x => x.JudgeTimeoutMs = 60_000);
		var watcher = Watcher(n, Client(o, h, n, new ManualClock(T0)), o, new ManualClock(T0));

		var sw = Stopwatch.StartNew();
		watcher.Trip(Bot("tallboy:AAAAAA"), TrendRules.FlipNotClosed);
		sw.Stop();

		Assert.True(sw.ElapsedMilliseconds < 1000);
		var page = Assert.Single(n.Pages);
		Assert.Equal("flip-not-closed", page.Reason);
		await Task.Delay(50);
		Assert.Single(n.Pages);
	}

	[Fact]
	public void The_judge_is_not_called_when_disabled_for_other_reasons_or_on_the_repeat_page()
	{
		var events = new List<string>();
		var judge = new HangingJudge(events);
		var clock = new ManualClock(T0);

		Watcher(new FakeNotifier(events), judge, Opts(o => o.JudgeEnabled = false), clock)
			.Trip(Bot("a"), TrendRules.FlipNotClosed);
		Watcher(new FakeNotifier(events), judge, Opts(), clock).Trip(Bot("b"), TrendRules.FlipNotOpened);
		Assert.Equal(0, judge.Calls);

		var repeat = Watcher(new FakeNotifier(events), judge, Opts(), clock);
		repeat.Trip(Bot("c"), TrendRules.FlipNotClosed);
		Assert.Equal(1, judge.Calls);
		repeat.Trip(Bot("c") with { At = T0.AddMinutes(31) }, TrendRules.FlipNotClosed);
		Assert.Equal(1, judge.Calls);
		Assert.Equal(
			["page:flip-not-closed", "page:flip-not-opened", "page:flip-not-closed", "judge", "page:flip-not-closed"],
			events);
	}

	// ---- the replay: Oct 22 fixture, real judge service, fake model ----

	private const string ReplayModelReply =
		"""{"verdict":"real","cause":"the close was refused by Jupiter with HTTP 429 three times in the first four seconds after the flip; nothing was sent, so the position stayed open and the engine is retrying","evidence":[{"at":"2026-10-22T00:00:01+0000","line":"[jupiter] swap-instructions: Jupiter swap-instructions HTTP 429 -- retry 1/3 in 1000ms"},{"at":"2026-10-22T00:00:08+0000","line":"bot tallboy:DVGSXybY9QD5PuRT5iZUyakjf9QGZujzgvAeixTMnBuk: failed (Jupiter swap-instructions HTTP 429)"}],"action":"watch","confidence":0.9}""";

	private sealed class ReplayModel(string reply) : SolBotJudge.IModel
	{
		public string Name => "fake";
		public bool KeyPresent => true;
		public string LastUser = "";
		public Task<string> CompleteAsync(string system, string user, CancellationToken ct)
		{
			LastUser = user;
			return Task.FromResult(reply);
		}
	}

	/// <summary>Turns the journal lines of the fixture into the rows GET /sentinel-log answers.</summary>
	private static string FixtureAsEngineLog()
	{
		var path = Path.Combine(AppContext.BaseDirectory, "fixtures", "engine-log--2026-10-22T00-00Z--flip-not-closed.txt");
		var rows = File.ReadAllLines(path).Where(l => l != "").Select(l =>
		{
			var m = Regex.Match(l, @"^(\S+) \S+ \S+\]: (.*)$");
			var at = DateTimeOffset.ParseExact(
				m.Groups[1].Value.Insert(m.Groups[1].Value.Length - 2, ":"), "yyyy-MM-dd'T'HH:mm:sszzz", null);
			return new { at = at.ToUnixTimeMilliseconds(), level = "info", line = m.Groups[2].Value };
		});
		return JsonSerializer.Serialize(new { since = 0, now = T0.ToUnixTimeMilliseconds(), lines = rows });
	}

	[Fact]
	public async Task Replay_of_the_Oct_22_flip_gives_the_exact_second_page()
	{
		var model = new ReplayModel(ReplayModelReply);
		var judgeApp = SolBotJudge.JudgeApp.Create([], b =>
		{
			b.WebHost.UseTestServer();
			b.Services.AddSingleton<SolBotJudge.IModel>(model);
		});
		await judgeApp.StartAsync();
		var judgeWire = new HttpMessageInvoker(judgeApp.GetTestServer().CreateHandler());

		var engineLog = FixtureAsEngineLog();
		var wire = new Handler((r, ct) => r.RequestUri!.AbsolutePath == "/sentinel-log"
			? Json(engineLog)
			: judgeWire.SendAsync(r, ct));
		var n = new FakeNotifier();

		await Client(Opts(), wire, n, new ManualClock(T0)).JudgeAndPageAsync(
			FlipAlert("tallboy:DVGSXybY9QD5PuRT5iZUyakjf9QGZujzgvAeixTMnBuk"), CancellationToken.None);

		Assert.Contains("[jupiter] swap-instructions: Jupiter swap-instructions HTTP 429 -- retry 3/3 in 4000ms", model.LastUser);
		Assert.Contains("[daemon] FLIP SHORT @ 2026-10-22T00:00:00.000Z price=106.84", model.LastUser);

		var page = Assert.Single(n.Pages);
		Assert.Equal("flip-not-closed:verdict", page.Reason);
		Assert.Equal(
			"cause: the close was refused by Jupiter with HTTP 429 three times in the first four seconds after the flip; "
			+ "nothing was sent, so the position stayed open and the engine is retrying"
			+ " | evidence: 2026-10-22T00:00:01+0000 [jupiter] swap-instructions: Jupiter swap-instructions HTTP 429 -- retry 1/3 in 1000ms; "
			+ "2026-10-22T00:00:08+0000 bot tallboy:DVGSXybY9QD5PuRT5iZUyakjf9QGZujzgvAeixTMnBuk: failed (Jupiter swap-instructions HTTP 429)"
			+ " | verdict real | action watch | confidence 0.90",
			page.Detail);
	}

	[Fact]
	public async Task Replay_with_an_invented_quote_pages_the_rejection_for_a_person()
	{
		var model = new ReplayModel(ReplayModelReply.Replace("retry 1/3 in 1000ms", "retry 1/3 in 9999ms"));
		var judgeApp = SolBotJudge.JudgeApp.Create([], b =>
		{
			b.WebHost.UseTestServer();
			b.Services.AddSingleton<SolBotJudge.IModel>(model);
		});
		await judgeApp.StartAsync();
		var judgeWire = new HttpMessageInvoker(judgeApp.GetTestServer().CreateHandler());
		var engineLog = FixtureAsEngineLog();
		var wire = new Handler((r, ct) => r.RequestUri!.AbsolutePath == "/sentinel-log"
			? Json(engineLog)
			: judgeWire.SendAsync(r, ct));
		var n = new FakeNotifier();

		await Client(Opts(), wire, n, new ManualClock(T0)).JudgeAndPageAsync(FlipAlert(), CancellationToken.None);

		Assert.Equal(
			"cause: judge output rejected: evidence line not in the log | evidence: none"
			+ " | verdict unknown | action person | confidence 0.00",
			Assert.Single(n.Pages).Detail);
	}
}
