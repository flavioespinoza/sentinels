# Sol Bot Sentinel `C# + ASP.NET Core`

<div class="sentinel-row sentinel-row-3">
<div class="sentinel-media sentinel-media-wide">
<img src="_design/final/sol-bot-sentinel/sol-bot-sentinel-1.png" alt="Sol Bot Sentinel, opening frame" />
<p class="sentinel-caption"><em>Created with OpenArt AI</em></p>
</div>
<div class="sentinel-text">

## I Watch The Trading Bots

Sol Bot is a pair of trading bots that borrow against collateral. A mistake costs real money, fast. The quiet failure is a trend flip a bot does not act on. This sentinel checks for it, and for risk lines and silent bots.

It holds no key and cannot trade. It reads the live feed every 15 seconds. Its first run caught a restart bug in the engine.

</div>
<div class="sentinel-media sentinel-media-wide">
<img src="_design/final/sol-bot-sentinel/sol-bot-sentinel-last.png" alt="Sol Bot Sentinel, last frame" />
<p class="sentinel-caption"><em>Created with OpenArt AI</em></p>
</div>
</div>

## How It Works

Sol Bot is a pair of automated trading bots that run for a private client on a public blockchain. One bot trades when the trend is long, the other when it is short, and each one borrows against collateral on a lending protocol, so a mistake costs real money and can cost it quickly. The engine carries its own layers against that: key custody in a policy signer, a position cap, a risk watcher, heartbeats, and a dead-man's switch on another machine.

The one failure those layers do not catch is the quiet one. The trend flips and a bot does not act. Nothing is down, nothing is over a line, and the position is simply wrong. Sol Bot Sentinel is the check for that, and for two more things while it is looking: an open position riding through a risk line, and a bot stepping outside its own limits or going silent. It reads a feed the engine serves for this purpose and nothing else. It cannot place, change, or close a trade, and it holds no key.

An independent, read-only sentinel for the Sol Bot auto-traders, written in C# on ASP.NET Core. It watches the bots from outside and raises an alert when a position crosses a risk line, a bot steps outside its limits, or the trend flips and a bot does not act. It cannot place, change, or close a trade.

For where this sits in the whole system, read [how the security is layered](../_docs/DOC__sol_bot_sentinel--architecture--security-layers.md).

Built Oct 05 2026 with Claude Code agents and deployed the same day: it runs on Cloud Run as one always-on instance and reads the production engine's feed over the private network, every 15 seconds, since 4:28 PM (MDT) on Oct 05 2026. By default a fresh checkout runs against a scripted market; `Source` set to `http` is the deployed configuration. Paging is wired: the deployed service reads its Pushover token and user from Secret Manager, so an alert pages Flavio's devices as well as being logged and served.

### The Three Watchers

Each watcher is one class and one hosted background service in the same process. A new watcher is one more class and one line in `Program.cs`.

- **Trend watcher** -- the double-check on the trend flip. When the trend flips, the bot for the old trend must close and the bot for the new trend must open:
  - `flip-not-closed`: the old-trend bot is still open after the grace window.
  - `flip-not-opened`: the new-trend bot is still idle after the grace window and never acted on the flip. A bot that opened and was then stopped out is not flagged.
  - A bot the engine reports as disabled, manual, or disarmed is not judged.
- **Risk watcher** -- three exit checks on an open position:
  - `price-stop`: price at or under `entry * (1 - stopLossPct / leverage)`.
  - `ltv-guard`: debt over collateral value at or over the threshold, ahead of the lender's own liquidation line.
  - `borrow-rate`: net carry under the floor for a full dwell window, so a brief rate spike does not trip it.
  - A reading that carries no prices is not judged on price.
- **Conduct watcher** -- that the bot is behaving:
  - `cap-exceeded`: a position opened over the per-position cap.
  - `leverage-exceeded`: leverage over the allowed maximum.
  - `stale-heartbeat`: the bot has stopped reporting.

The service also watches its own watchers: `/health` reports `degraded` when any one of them has stopped polling.

An alert is raised once, at the first crossing, and not again until the condition clears. When Pushover is configured, each new alert is also sent to a phone, once; `/health` reports `paging: true` when it is. A read error never stops a watcher: it logs and tries again on the next tick.

### Run It

From the root of this repo:

```bash
dotnet run --project src/SolBotSentinel
```

Or in a container:

```bash
docker build -t sol-bot-sentinel:1 .
docker run --rm -p 8080:8080 --name sol-bot-sentinel-1 sol-bot-sentinel:1
```

```bash
curl http://localhost:5037/health
curl http://localhost:5037/alerts
curl http://localhost:5037/alerts/history
```

The scripted market runs about a minute. One bot's price walks down to its stop and the risk watcher raises `price-stop`. Then the trend flips: the second bot opens over the cap and the conduct watcher raises `cap-exceeded`; the first bot never closes, and once the grace window is over the trend watcher raises `flip-not-closed`.

### Deploy It

```bash
GCP_PROJECT=... FEED_URL=... FEED_TOKEN_SECRET=... API_TOKEN_SECRET=... ./scripts/deploy.sh
```

It deploys to Cloud Run as one always-on instance under a service account that can read only its own secrets, reads the feed over the private network, and finishes only when `/health` reports `ok` on the live feed. Run it again to redeploy.

### Test It

```bash
dotnet test
```

Thirty-eight tests: the rule arithmetic at each boundary, the dwell window, the flip grace window, the raise-once store, the health of the watchers, the bearer check, the Pushover notifier, the running service read over HTTP, and the contract with the engine's feed.

### Configure It

Every threshold is in the `Watch` section of `appsettings.json` and can be overridden by environment variable, for example `Watch__PollMs=500`. No address or key of ours is written in source. The tokens are secrets and belong in the host's secret store, never in a file.

| Setting | Default | Meaning |
|---------|---------|---------|
| `Source` | `simulated` | `simulated`, or `http` to read the engine's feed |
| `FeedUrl` | empty | the address of the engine's feed |
| `FeedToken` | empty | the bearer token the engine's feed requires |
| `ApiToken` | empty | when set, `/alerts` and `/alerts/history` require it as a bearer token |
| `PollMs` | `2000` | how often the watchers read |
| `StopLossPct` | `0.10` | share of the deposit that may be lost before the price stop |
| `LtvTripThreshold` | `0.80` | loan-to-value that trips the guard |
| `CarryFloorApr` | `0` | net carry under this starts the dwell clock |
| `CarryDwellMs` | `1200000` | how long carry must stay under the floor |
| `PositionCapUsd` | `100` | largest deposit a bot may open with |
| `MaxLeverage` | `3.0` | highest leverage a bot may use |
| `HeartbeatStaleMs` | `60000` | age at which a reading counts as stale |
| `PushoverToken`, `PushoverUser` | empty | when both are set, each new alert is pushed to a phone |
| `FlipGraceMs` | `30000` | how long a bot has to act on a trend flip |
| `FlipToleranceMs` | `2000` | slack when matching a bot's last action to the flip time |

### Layout

```txt
src/SolBotSentinel/
  Program.cs            wiring and the three endpoints
  Domain/Models.cs      snapshot, trend reading, alert, options
  Rules/                the checks, as pure functions
  Sources/              where readings come from
  Watchers/             the three background services
  AlertStore.cs         raise-once alert memory
  Health.cs             the check on the watchers themselves
  ApiAuth.cs            the bearer check for the alert routes
  Notify.cs             the Pushover notifier
tests/SolBotSentinel.Tests/
_docs/                  the architecture
scripts/deploy.sh        deploy to Cloud Run
Dockerfile
.github/workflows/ci.yml
```

## What It Is Useful For

Any system where an automated actor must react to a signal inside a window, where a position must stay inside lines drawn in advance, and where a quiet failure to act costs as much as a loud crash. The three watchers are three reusable patterns: did the actor act on the signal (trend), is the position inside its risk lines (risk), is the actor inside its own limits and still alive (conduct). Each rule is a pure function with a test at every boundary, each watcher is one class, and the readings come through an interface, so the feed can be any JSON endpoint that reports a state and a timestamp. The sentinel never needs a write path to the thing it watches, which is what makes it safe to run against production from day one.

## Examples Of What You Could Use It For

- **`A`**: `Market Makers On A Centralized Exchange` -- point the feed at a fleet of bots' reported state, set the per-position cap and the maximum leverage, and get paged when one bot opens over its size, leans past its leverage, or stops reporting, without the sentinel ever holding an exchange key.
- **`B`**: `A Lending Desk Watching Loan-To-Value` -- the risk watcher's `ltv-guard` is a threshold set ahead of the lender's own liquidation line. Feed it debt and collateral per account on any lending venue and it pages while there is still time to add collateral, not after the liquidation has happened.
- **`C`**: `Workers That Must React To A Flag` -- the trend watcher is a grace window around a signal. Replace the trend with a feature flag, a failover event, or a deploy marker, and the sentinel pages when a worker that should have switched over did not, which is the kind of failure a health check never sees.

## Status

Deployed on production. The code lives in this repo, at the root: `src/SolBotSentinel/`, `tests/SolBotSentinel.Tests/`, the `Dockerfile`, `scripts/deploy.sh`, and the GitHub Actions workflow. It runs on Cloud Run as one always-on instance, reading the production engine's feed over the private network every 15 seconds since Oct 05 2026. Thirty-eight xUnit tests run on every push. Paging through Pushover is in the code and switched on by configuration; `/health` reports `paging: true` when it is live. On its first run against the production feed it exposed a restart bug in the engine, a replay of months-old trend flips into two new bots, and the engine now gives every bot an origin date and ignores any flip from before it.
