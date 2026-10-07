# Corpus Sentinel

`Rust`

## What It Is

A guard at the door of a retrieval engine. The engine is TypeScript and holds a matrix that is expensive to load, so it stays loaded, on localhost, and is never reached from outside directly. Corpus Sentinel is the only thing the outside can reach: a Rust service on `axum`, `rusqlite`, and `tokio` that owns the authentication, the rate limit, the health check, and the private firewall boundary, and passes everything that clears the gate through to the engine. The matrix stays loaded while the surface stays small.

## How It Works

- **`A`**: `Auth` -- a bearer token on every request.
- **`B`**: `Rate Limit` -- a cap on how often the door opens.
- **`C`**: `Health` -- a route that says whether the sentinel and the engine behind it are up.
- **`D`**: `The Boundary` -- the engine listens on localhost only; the sentinel is the one surface the firewall exposes.
- **`E`**: `Pass-Through` -- everything that clears the gate goes to the engine on localhost, and the answer comes back the same way.

## What It Is Useful For

A small, auditable surface in front of a large, stateful process. The expensive thing, an index or a model in memory, is written once in the language that suits it and never has to carry its own auth, limits, or hardening. The sentinel is a few hundred lines that can be read end to end, and it is the only code that ever faces the network.

## Examples Of What You Could Use It For

- **`F`**: `A Local Inference Or Embedding Server` -- a model server bound to localhost, reached by one trusted client through a bearer token and a rate limit, with no auth code inside the model server at all.
- **`G`**: `An Internal Admin API` -- a rate-limited, token-checked shim in front of an admin surface that was never written to be exposed, so it can be reached from one place without being rewritten.
- **`H`**: `A Search Index That Takes Minutes To Warm` -- keep the index process alive and private, and put the sentinel in front of it, so a restart of the public surface never costs a reload of the index.

## Status

Designed; the first pieces are building. The code lives in a private repo and is to be moved here.
