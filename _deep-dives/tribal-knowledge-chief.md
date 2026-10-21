# Tribal Knowledge Chief `TypeScript`

<div class="sentinel-row sentinel-row-3">
<div class="sentinel-media sentinel-media-wide">
<img src="_design/final/tribal-knowledge/tribal-knowledge-1.png" alt="Tribal Knowledge Chief, opening frame" />
<p class="sentinel-caption"><em>Created with OpenArt AI</em></p>
</div>
<div class="sentinel-text">

## I Watch The Screenshot Inbox

Knowledge in a chat thread is tribal. It lives in someone's head and in a scrollback nobody will search. This file watcher turns a Slack screenshot into Markdown for a retrieval engine.

Drop a screenshot in a folder. Gemini transcribes it word for word. Then Gemini sorts it into a topic, a summary, takeaways, and actions. The note is filed and indexed.

It acts only on the files it finds. War drums play when a note lands.

</div>
<div class="sentinel-media sentinel-media-wide">
<img src="_design/final/tribal-knowledge/tribal-knowledge-last.png" alt="Tribal Knowledge Chief, last frame" />
<p class="sentinel-caption"><em>Created with OpenArt AI</em></p>
</div>
</div>

## How It Works

Knowledge that lives in a chat thread is tribal: it is in someone's head and in a scrollback nobody will search. Drop a screenshot of the thread into a folder and the watcher transcribes it word for word, structures it, files it, and indexes it, so it can feed a retrieval engine. It is a watcher over an inbox rather than a guard over a production system.

The engine runs from its own repo. A target project gets only a lean output folder: the Markdown, an index, and an `images/` inbox with a `done/` archive, no source and no `node_modules`. Drop an image into `images/`, any name, any format, and the pipeline runs on its own:

- **`A`**: `Rename` -- to `tribal__screenshot--{YYYY-MM-DD}__{epoch}.{ext}`.
- **`B`**: `Preprocess` -- compress to JPEG when the image is over 10 MB.
- **`C`**: `OCR` -- a verbatim transcription with Gemini Vision.
- **`D`**: `Analyze` -- Gemini 2.5 Flash structures it: topic, authors, the verbatim text, a summary, takeaways, and actions.
- **`E`**: `Write` -- a `.md` with the same name as the image.
- **`F`**: `Archive` -- the image moves to `images/done/`.
- **`G`**: `Report` -- an entry is appended to the index file.
- **`H`**: `Alert` -- war drums play, a blocking macOS popup stays up until it is dismissed, and the path and heading land on the clipboard when it is.

It is built on `chokidar` and runs from the command line or as a launchd daemon. It needs a Gemini API key in the environment.

## What It Is Useful For

Anything that arrives as a picture of text and needs to become text that can be searched, linked, and fed to a retrieval engine. The watcher is the plumbing: a folder as the API, one file in, one structured file out, an index that keeps itself current, and an alert loud enough that the result is read the moment it lands.

## Examples Of What You Could Use It For

- **`I`**: `A Support Team's Chat Captures` -- screenshots of customer conversations from any chat tool, transcribed and structured into topic, summary, and actions, filed under one index the whole team can search.
- **`J`**: `Whiteboard Photos After A Meeting` -- a photo of the board dropped in the folder comes out as a Markdown note with the takeaways and the action items pulled out, before anyone has erased the board.
- **`K`**: `Receipts And Invoices Into A Ledger` -- a picture of a receipt becomes a structured entry, archived beside the image it came from, with the index as the running ledger.

## Status

Built; runs from its home repo, `watcher-tribal-knowledge`.
