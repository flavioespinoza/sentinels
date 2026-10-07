# Obligation Reader

`Rust`

## What It Is

An on-chain parity harness for the Sol Bot trading system. The engine decodes its lending positions, Kamino obligation accounts on Solana, in TypeScript from raw account bytes, with no SDK in the path. Obligation Reader decodes the same accounts a second time, in Rust with `solana-sdk`, and publishes each reading beside the engine's own value, with the delta between them. Two decoders in two languages that must agree, and a page the moment they do not.

## How It Works

- **`A`**: `The Decode` -- `solana-sdk` reads the obligation account from the chain and decodes its fields independently of the engine's code.
- **`B`**: `The Publish` -- each reading is published beside the engine's TypeScript value, with the delta, so the two sit side by side where a person can read them.
- **`C`**: `The Page` -- on divergence between the two readings.
- **`D`**: `The Fence` -- never a decision input. The engine does not read the harness; a person does.

## What It Is Useful For

Any place where one decoder of a critical value is one too few. A hand-rolled decoder of someone else's binary layout is right until the layout changes, and the first sign is usually a wrong number acted on. A second implementation in a second language, reading the same bytes and publishing the delta, turns a silent drift into a page, and because it is never on the decision path it can be wrong without costing anything but a look.

## Examples Of What You Could Use It For

- **`E`**: `Retiring A Vendor SDK` -- run the vendor's decoder and the replacement side by side on live accounts, publish the delta, and page on the first divergence. Cut over only after the delta has been zero for as long as it needs to be.
- **`F`**: `A Custody Reconciliation` -- an independent reading of a balance or a position beside the ledger's own figure, written by a different team in a different language, so a bug in the ledger's parser cannot also be in the check.
- **`G`**: `Two Implementations Of One Spec` -- any protocol with a published account or message layout and more than one consumer. Decode it twice, compare, and let the harness tell you when the spec and the code have parted ways.

## Status

Specified; build in progress. The code lives in a private repo and is to be moved here.
