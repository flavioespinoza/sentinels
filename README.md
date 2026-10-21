# Sentinels

Independent, read-only watchdogs over production systems, each written in the language that fits its host. Built by Flavio Espinoza, every one of them with a Claude Code agent at the keyboard.

## What A Sentinel Is

A sentinel is a guard posted outside a system to watch it and raise the alarm. It never acts. It reads what the system exposes, compares it against a line drawn in advance, and the moment the line is crossed it pages a person. Then it goes back to watching.

The rule is the same for every one of them:

- **`A`**: `It Cannot Act On The System It Watches` -- a sentinel reads a feed, a folder, a log, or a chain. It holds nothing that could place a trade, close a position, or change the thing it guards. If the sentinel itself is compromised, the worst it can do is page.
- **`B`**: `It Holds No Key` -- no signing key, no credential that moves money. A bearer token to read a feed is the most it carries, and that token is a secret in the host, never a line in a file.
- **`C`**: `It Pages A Person` -- a sentinel decides nothing. It raises an alert once, at the first crossing, sends it to a phone, and stays quiet until the condition clears. The chain of watchers ends at a desk, where a person makes the call.

It is turtles all the way down: a watcher over the engine, a watcher over the watcher, and at the bottom a human. The file watchers at the end of the roster bend the first rule on purpose: each one acts on the folder it watches, and on nothing else.

Each sentinel is meant to stand on its own, with its own build and its own tests and nothing shared between them, so a visitor can take the one in the language they want and leave the rest. Today the C# sentinel lives in this repo at the root. The others are listed with where their code lives now, and each one has a page under `_deep-dives/` that says what it is, how it works, what it is useful for, and how far along it honestly is.

## The Roster

### Sol Bot Sentinel

<div class="sentinel-row">
<div class="sentinel-media">
<img src="_design/final/sol-bot-sentinel/sol-bot-sentinel-1.png" alt="Sol Bot Sentinel" />
</div>
<div class="sentinel-text">

`C# + ASP.NET Core`

**Status:** deployed on production, on Cloud Run, since Oct 05 2026.

An independent watchdog over a pair of live auto-traders that run for a private client on a public blockchain. Three hosted watchers read the engine's feed every 15 seconds: one for the trend flip a bot failed to act on, one for an open position crossing a risk line, and one for a bot stepping outside its own limits or going quiet. On its first run against the production feed it caught a restart replaying months-old trend flips into new bots.

[Deep dive: Sol Bot Sentinel](_deep-dives/sol-bot-sentinel.md)

</div>
</div>

### Sentry Engine

<div class="sentinel-row">
<div class="sentinel-media">
<img src="_images/sentry-engine.svg" alt="Sentry Engine" />
</div>
<div class="sentinel-text">

`C++`

**Status:** running in tests; the graph scheduler is a stub; code in a private repo, to be rebuilt here from source.

The real-time audio engine under a home security sentry: a node graph, parameters with two-stage smoothing, and a BiQuad filter on Transposed Direct Form II that allocates nothing in the hot path. Thirteen DSP tests, from impulse response to a single-bin DFT, and a benchmark that defeats dead-code elimination. The filter is real and tested; the graph scheduler is still a stub, and its page says so.

[Deep dive: Sentry Engine](_deep-dives/sentry-engine.md)

</div>
</div>

### Candle Watch

<div class="sentinel-row">
<div class="sentinel-media">
<img src="_images/candle-watch.svg" alt="Candle Watch" />
</div>
<div class="sentinel-text">

`Rust`

**Status:** specified; build in progress; code in a private repo, to be moved here.

A small daemon that asks one question of every market: are the candles still arriving? It reads the newest candles per market, runs a per-market state machine (ok, stale, flat, unreadable), and pages on a transition and once on recovery. It never repairs and never trades.

[Deep dive: Candle Watch](_deep-dives/candle-watch.md)

</div>
</div>

### Obligation Reader

<div class="sentinel-row">
<div class="sentinel-media">
<img src="_images/obligation-reader.svg" alt="Obligation Reader" />
</div>
<div class="sentinel-text">

`Rust`

**Status:** specified; build in progress; code in a private repo, to be moved here.

A second pair of eyes on an on-chain position. It decodes the lending protocol's obligation accounts in Rust with `solana-sdk` and publishes each reading beside the value the engine's own TypeScript decoder produced, with the delta. It pages on divergence and is never a decision input.

[Deep dive: Obligation Reader](_deep-dives/obligation-reader.md)

</div>
</div>

### Corpus Sentinel

<div class="sentinel-row">
<div class="sentinel-media">
<img src="_images/corpus-sentinel.svg" alt="Corpus Sentinel" />
</div>
<div class="sentinel-text">

`Rust`

**Status:** designed; first pieces building; code in a private repo, to be moved here.

A small, hard front door for a retrieval engine that must stay loaded in memory. An `axum` service carries the bearer-token auth, the rate limit, the health route, and the private firewall boundary, and passes everything else through to the engine on localhost. The matrix stays loaded while the surface stays small.

[Deep dive: Corpus Sentinel](_deep-dives/corpus-sentinel.md)

</div>
</div>

### Tribal Knowledge Watcher

<div class="sentinel-row">
<div class="sentinel-media">
<img src="_design/final/tribal-knowledge/tribal-knowledge-1.png" alt="Tribal Knowledge Watcher" />
</div>
<div class="sentinel-text">

`TypeScript`

**Status:** built; runs from its home repo, `watcher-tribal-knowledge`.

A folder watcher that turns a Slack screenshot into structured Markdown. Drop an image in, and it renames it, transcribes it word for word with Gemini Vision, structures it with Gemini 2.5 Flash into topic, authors, summary, takeaways, and actions, and files the result under an index. Then it plays the drums and pops an alert so nobody misses it.

[Deep dive: Tribal Knowledge Watcher](_deep-dives/tribal-knowledge-watcher.md)

</div>
</div>

### Directive Sync Watcher

<div class="sentinel-row">
<div class="sentinel-media">
<img src="_images/directive-sync-watcher.svg" alt="Directive Sync Watcher" />
</div>
<div class="sentinel-text">

`TypeScript`

**Status:** built; runs from its home repo, `watcher-directive-sync`.

A mechanical watcher that keeps a rules file in sync with the specs that govern it. It polls a specs folder, hashes each directive spec, waits for the file to sit still, then swaps the spec's content in between two boundary markers in the target file. No AI anywhere in it: a deterministic text swap, with 28 tests.

[Deep dive: Directive Sync Watcher](_deep-dives/directive-sync-watcher.md)

</div>
</div>

### Veritas Sentinel

<div class="sentinel-row">
<div class="sentinel-media">
<img src="_images/veritas-sentinel.svg" alt="Veritas Sentinel" />
</div>
<div class="sentinel-text">

`JavaScript`

**Status:** built; lives in its home repo, `veritas-sentinel`.

A black-box recorder for AI agent sessions. It tails a folder of JSONL logs by byte offset, so nothing is ever re-read, and writes every turn into a live Markdown HUD that updates in the editor as the session runs. When an assistant turn runs long with no tool call, it warns on the console.

[Deep dive: Veritas Sentinel](_deep-dives/veritas-sentinel.md)

</div>
</div>

### Mercy

<div class="sentinel-row">
<div class="sentinel-media">
<img src="_design/final/mercy/mercy-1.png" alt="Mercy" />
</div>
<div class="sentinel-text">

`JavaScript`

**Status:** older code; lives in its home repo, eScanner.

The oldest of the set, a Node watcher inside a market scanner. It subscribes to an exchange's websocket for candles, the order book, and trades across every market quoted in BTC, ETH, USDT, or USD, splits the markets across process instances, and mirrors what it sees into Elasticsearch. It predates the three rules above and is listed here as the ancestor of the rest.

[Deep dive: Mercy](_deep-dives/mercy.md)

</div>
</div>

### FX Watch

<div class="sentinel-row">
<div class="sentinel-media">
<img src="_images/fx-watch.svg" alt="FX Watch" />
</div>
<div class="sentinel-text">

`Shell`

**Status:** running as a macOS launch agent; code in its home repo, `fx-renamer`.

A launchd agent that renames every screenshot the moment it lands. It waits five seconds for the capture app to finish writing, then gives the file one lowercase, sortable, timestamped name, and touches nothing that was already in the folder. The renamed file is ready to be filed into an asset store.

[Deep dive: FX Watch](_deep-dives/fx-watch.md)

</div>
</div>
