# Mercy `JavaScript`

<div class="sentinel-row sentinel-row-3">
<div class="sentinel-media sentinel-media-wide">
<img src="_design/final/mercy/mercy-1.png" alt="Mercy, opening frame" />
<p class="sentinel-caption"><em>Created with OpenArt AI</em></p>
</div>
<div class="sentinel-text">

## I Watch The Exchange Feed

The oldest watcher in the set, a Node process inside eScanner, a market scanner. It is the ancestor of the rest. It was the first process set outside the trading logic to watch a market and write down what it saw.

It mirrors HitBTC's candles, order book, and trades into Elasticsearch, market by market. It waits 8 seconds between markets so the exchange is not flooded. It logs its own connection trouble to MongoDB.

It splits the markets eight ways. Each process takes one slice.

</div>
<div class="sentinel-media sentinel-media-wide">
<img src="_design/final/mercy/mercy-last.png" alt="Mercy, last frame" />
<p class="sentinel-caption"><em>Created with OpenArt AI</em></p>
</div>
</div>

## How I Work

Mercy subscribes to an exchange's websocket and keeps an eye on candles, the order book, and trades across every market quoted in BTC, ETH, USDT, or USD, mirroring what it sees into Elasticsearch and logging its own connection trouble to MongoDB. It predates the three rules on the front page and is listed here as the ancestor of the rest: the first time a process was posted outside the trading logic to watch a market and write down what it saw.

- **`A`**: `Subscribe` -- a websocket client to HitBTC subscribes each market to candles, the order book, and trades, with 8 seconds between markets so the exchange is not flooded. It reconnects half a second after a close, and every close and error is written to MongoDB with its timestamp.
- **`B`**: `Shard` -- the market list is split eight ways, and each process instance takes one slice by its process manager id and listens on its own port.
- **`C`**: `Candles` -- each snapshot and update is mapped and written to Elasticsearch, and the newest candle is enriched with percent change, volume, and win-loss ratios over the last 2, 3, and 4 candles.
- **`D`**: `Order Book` -- a sorted insert keeps each side in price order and notes when the best bid or ask changed. Counts and quantities per market are flushed to Elasticsearch every 10 seconds, with the trades.
- **`E`**: `Scan` -- a universe scan walks the instance's markets at the 1, 5, 15, 30, 60, and 240 minute intervals in a loop, 15 seconds between passes.

## What It Is Useful For

Mirroring a live exchange feed into a store that can be queried and charted, across more markets than one process should subscribe to, with the subscriptions spread over several processes by a simple split. The pieces are plain: a reconnecting websocket client with request timeouts, a sorted order-book store, and a periodic flush.

## Examples Of What You Could Use It For

- **`F`**: `An Exchange Feed Into Elasticsearch` -- subscribe to any exchange's websocket for candles, book, and trades, and mirror them into an index that a dashboard can chart.
- **`G`**: `Sharding Subscriptions By Process Id` -- split a long list of markets, devices, or channels across process instances by id, each instance taking its own slice and its own port.
- **`H`**: `A Best-Bid-Or-Ask Change Detector` -- the sorted insert and the flag on a changed top of book, as the trigger for anything that only cares when the best price moves.

## Status

Older code; lives in its home repo, eScanner. It reads exchange credentials from the environment to list the markets, which the newer sentinels would not do; in this file that listing is the only call made with them.
