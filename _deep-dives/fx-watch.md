# FX Watch

`Shell`

## What It Is

A macOS launch agent that renames every screenshot the moment it lands. A capture app writes `My App__2026-09-13_AM-3-01-32.png`; about five seconds later the file is `myapp--2026-09-13--03-01-32--am.png`: lowercase, no spaces, one timestamp shape, an AM or PM tag at the end, the created and modified dates untouched. It is the shell half of `fx-renamer`, whose rename rule is a TypeScript module with its own tests; the watcher is what makes the rule happen without anyone running it. The renamed file is ready to be filed into an asset store. It acts on the folder it watches, and on nothing else.

## How I Work

- **`A`**: `Install` -- `scripts/install-watch.sh /path/to/folder` writes a launch agent plist from a template, lints it with `plutil`, records the install time, and bootstraps the agent with `launchctl`. Only files created after that time are ever renamed, so the folder's history is never touched.
- **`B`**: `Trigger` -- launchd runs `fx-watch.sh` whenever a file lands in the watched folder.
- **`C`**: `Wait` -- the script sleeps 5 seconds so the capture app can finish writing, and any file younger than 2 seconds is skipped on top of that.
- **`D`**: `Rename` -- it calls the renamer on the folder with the install time as the floor, and every rename goes to a log.
- **`E`**: `Preserve` -- created and modified dates are never changed, so sorting by date keeps working, and files tracked by git are skipped.

## What It Is Useful For

Any folder where files arrive with names chosen by a tool and need names chosen by a person. The capture app, the camera, and the browser each have a naming shape of their own; the watcher makes them all one shape within seconds, and it never touches what was already there.

## Examples Of What You Could Use It For

- **`F`**: `A Downloads Folder` -- every new file gets a lowercase, timestamped, sortable name as it lands, and the pile from last year is left exactly as it was.
- **`G`**: `A Team's Shared Screenshot Folder` -- one naming shape for everyone, whatever capture tool each person uses, with the AM or PM tag so the hour reads at a glance.
- **`H`**: `Camera Imports` -- photos renamed as they are copied in, with the original capture time preserved in the file's dates and spelled out in the name.

## Status

Running as a macOS launch agent. The code lives in its home repo, `fx-renamer`: `scripts/fx-watch.sh`, `scripts/install-watch.sh`, and the launchd template.
