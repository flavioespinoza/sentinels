# Candle Watch

`Rust`

## What It Is

A stale-and-flat candle daemon for the Sol Bot trading system. The engine builds its own candles from on-chain reads every minute, and every decision downstream assumes those candles keep arriving and keep moving. Candle Watch asks that one question, per market, all the time, and pages when the answer changes. It never repairs a feed and never trades.

## How I Work

- **`A`**: `The Read` -- a `tokio` daemon with a `mongodb` client reads the newest candles per market.
- **`B`**: `The State Machine` -- one per market, with four states: ok, stale (the candles stopped arriving), flat (the candles arrive but do not move), and unreadable (the read itself failed).
- **`C`**: `The Page` -- Pushover on a transition between states, and once on recovery. Never a page per tick, so a market that is stale for an hour is one alert and one all-clear.
- **`D`**: `The Surface` -- `axum` for the HTTP side.
- **`E`**: `The Host` -- it survives a VM reset under systemd, from a distroless image built by Cloud Build.
- **`F`**: `The Fence` -- it never repairs and never trades. It reads, it judges, it pages.

## What It Is Useful For

Any feed that is supposed to arrive on a schedule and can silently stop. A dead feed is worse than a loud crash: the process stays up, the health check stays green, and everything downstream keeps running on stale numbers. The per-market state machine turns that into four named states and a page at every edge.

## Examples Of What You Could Use It For

- **`G`**: `A Fleet Of Sensors` -- one state machine per device over a collection of readings. A sensor that stops reporting is stale, one that reports the same value forever is flat, and the operator gets one page per edge instead of a dashboard to stare at.
- **`H`**: `A Price Feed Behind A Dashboard` -- watch each market's candles in the store the dashboard reads from, so a feed that quietly stopped is caught before a trader notices a chart that has not moved in an hour.
- **`I`**: `A Scheduled Job That Writes To A Collection` -- an ETL or cron task that appends on a timer. The newest document's age is the whole signal, and a job that stopped at 3 AM pages at 3:05 AM rather than at the morning standup.

## Status

Specified; build in progress, local first, then preview, then production on Google Cloud. The code lives in a private repo and is to be moved here.
