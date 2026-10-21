# Veritas Sentinel

`JavaScript`

## What It Is

A black-box recorder for AI agent sessions. It tails local JSONL log files and turns them into a human-readable Markdown HUD, `live_audit.md`, that updates as the session runs and can be watched in an editor's preview pane. It reads, it writes one file, and it warns on the console when the agent is talking instead of working. One file of code, one dependency, written Feb 2026.

## How I Work

- **`A`**: `Watch` -- `chokidar` watches a log directory, set by `LOG_DIR`, for `.jsonl` files, polling every 100 milliseconds.
- **`B`**: `Cursors` -- an in-memory map of file path to byte offset. On every change it reads only the bytes appended since the last read, so nothing is ever re-read and several files can be followed at once.
- **`C`**: `Transform` -- each line is one JSON object with a `role`, `content`, a `timestamp`, and any `tool_calls`. Each becomes a timestamped section in `live_audit.md`, with the tools it called listed under it.
- **`D`**: `The Alert` -- an `assistant` turn longer than 1,500 characters with no `tool_calls` prints a warning to the console: a long monologue, intervene now.
- **`E`**: `The Shape` -- functional and data-oriented, no classes, built for Node on macOS.

## What It Is Useful For

Watching what an agent is doing while it does it, from a log file it already writes, without touching the agent. The byte-offset cursor makes it cheap to leave running for a whole session, and the Markdown output means the record is readable by a person the moment it is written and stays readable afterward.

## Examples Of What You Could Use It For

- **`F`**: `A Live View During A Demo` -- tail any agent's JSONL session log into a Markdown pane on a second screen, so the audience sees each turn and each tool call as it happens.
- **`G`**: `A Chatbot's Conversation Logs` -- point it at a production chatbot's append-only logs and watch for long replies that called no tool, the shape of an answer that is padding instead of doing.
- **`H`**: `Any Append-Only JSONL Event Log` -- an application's event stream, rendered as a live Markdown feed for someone who will not read raw JSON.

## Status

Built; lives in its home repo, `veritas-sentinel`, at v1.1.0.
