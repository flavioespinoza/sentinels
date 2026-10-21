# Directive Sync Watcher

`TypeScript`

## What It Is

A mechanical watcher that keeps a rules file in sync with the specs that govern it. A rule is written once, in a directive spec, and the watcher carries it into the section of the ruleset where it belongs, between two boundary markers, so the spec and the ruleset never drift apart. There is no AI anywhere in it: a deterministic text swap, with 28 tests. It is a watcher over a folder of specs rather than a guard over a production system: it writes into the one target file each spec names, and nothing else.

## How I Work

- **`A`**: `Scan` -- it polls one or more specs folders for files matching `SPEC__*--directive--*.md`, every 5 minutes by default.
- **`B`**: `Snapshot` -- it takes a SHA-256 hash of each file's content, so a change in modification time alone is ignored.
- **`C`**: `Debounce` -- a file must sit unchanged for the full window, 10 minutes by default, before it syncs, so a half-saved spec never lands in the ruleset.
- **`D`**: `Parse` -- it reads the spec's SYNC DIRECTIVE header, in blockquote or HTML comment form, for the subtype (the global ruleset or a project's local one), the target file, and the two boundary markers.
- **`E`**: `Swap` -- it finds the boundary headings in the target, deletes everything between them, and inserts the spec's content with the header stripped. The boundary headings themselves are kept and every other section is untouched.
- **`F`**: `Force And Stop` -- `--sync-now` skips the debounce, syncs every spec it finds, and exits. SIGINT and SIGTERM clear the poll loop and exit cleanly.

Six modules, one responsibility each: the types, the directive parser, the boundary engine, the file watcher with its debounce tracker, the sync executor, and the CLI. The tests cover the parser in both header forms, the boundary swap, the folder scan, and a full round trip.

## What It Is Useful For

Any document that has one canonical source and one or more places where its content must appear word for word. Humans copy sections between files and then forget, and a rule that exists in two slightly different versions is worse than no rule. The watcher makes the copy mechanical, hash-checked, and debounced, and it never touches a line outside the markers.

## Examples Of What You Could Use It For

- **`G`**: `A README Section Mirrored From A Spec` -- keep the installation or configuration section of a README identical to the canonical spec across several repos, each README carrying its own pair of markers.
- **`H`**: `Team Standards Into Every Project` -- one coding-standards document, synced into the contributor guide of each project between markers, so a change to the standard lands everywhere after it has sat still for ten minutes.
- **`I`**: `An Approved Block Into A Generated Page` -- the approved summary of a design document carried into a changelog or wiki page, with the rest of that page left to whatever else writes it.

## Status

Built; runs from its home repo, `watcher-directive-sync`.
