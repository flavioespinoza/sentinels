# Sentinels

Independent, read-only watchdogs over production systems, each written in the language that fits its host. Built by Flavio Espinoza, every one of them with a Claude Code agent at the keyboard.

**Docs site:** [flavioespinoza.github.io/sentinels](https://flavioespinoza.github.io/sentinels/)

## What A Sentinel Is

A sentinel is a guard posted outside a system to watch it and raise the alarm. It never acts. It reads what the system exposes, compares it against a line drawn in advance, and the moment the line is crossed it pages a person. Then it goes back to watching.

The rule is the same for every one of them:

- **`A`**: `It Cannot Act On The System It Watches` -- a sentinel reads a feed, a folder, a log, or a chain. It holds nothing that could place a trade, close a position, or change the thing it guards. If the sentinel itself is compromised, the worst it can do is page.
- **`B`**: `It Holds No Key` -- no signing key, no credential that moves money. A bearer token to read a feed is the most it carries, and that token is a secret in the host, never a line in a file.
- **`C`**: `It Pages A Person` -- a sentinel decides nothing. It raises an alert once, at the first crossing, sends it to a phone, and stays quiet until the condition clears. The chain of watchers ends at a desk, where a person makes the call.

It is turtles all the way down: a watcher over the engine, a watcher over the watcher, and at the bottom a human. The file watcher on the roster bends the first rule on purpose: it acts on the folder it watches, and on nothing else.

Each sentinel is meant to stand on its own, with its own build and its own tests and nothing shared between them, so a visitor can take the one in the language they want and leave the rest. Today the C# sentinel lives in this repo at the root. The others are listed with where their code lives now, and each one has a page under `_deep-dives/` that says what it is, how it works, what it is useful for, and how far along it honestly is.

## The Roster

### Sol Bot Sentinel

<div class="sentinel-row">
<div class="sentinel-media">
<img src="_design/final/sol-bot-sentinel/sol-bot-sentinel-1.png" alt="Sol Bot Sentinel" />
<p class="sentinel-caption"><em>Created with OpenArt AI</em></p>
</div>
<div class="sentinel-text">

`C# + ASP.NET Core`

**Status:** deployed on production, on Cloud Run, since Oct 05 2026.

Every 15 seconds it reads the feed of two live auto-traders and pages a phone on a missed trend flip, a position past a risk line, or a bot out of bounds or gone quiet.

[Deep dive: Sol Bot Sentinel](_deep-dives/sol-bot-sentinel.md)

</div>
</div>

### Tribal Knowledge Chief

<div class="sentinel-row">
<div class="sentinel-media">
<img src="_design/final/tribal-knowledge/tribal-knowledge-1.png" alt="Tribal Knowledge Chief" />
<p class="sentinel-caption"><em>Created with OpenArt AI</em></p>
</div>
<div class="sentinel-text">

`TypeScript`

**Status:** built; runs from its home repo, `watcher-tribal-knowledge`.

Watches a folder for Slack screenshots and turns each one into structured Markdown, transcribed word for word and filed under an index. Then it plays the drums and pops an alert.

[Deep dive: Tribal Knowledge Chief](_deep-dives/tribal-knowledge-chief.md)

</div>
</div>

### Mercy

<div class="sentinel-row">
<div class="sentinel-media">
<img src="_design/final/mercy/mercy-1.png" alt="Mercy" />
<p class="sentinel-caption"><em>Created with OpenArt AI</em></p>
</div>
<div class="sentinel-text">

`JavaScript`

**Status:** older code; lives in its home repo, eScanner.

The oldest sentinel, a Node watcher that copies an exchange's candles, order book, and trades for every market quoted in BTC, ETH, USDT, or USD into Elasticsearch. It predates the three rules above.

[Deep dive: Mercy](_deep-dives/mercy.md)

</div>
</div>
