# SPEC: Sentinels -- Landing Page Design

v1 | Oct 07 2026

**Scope:** visual design language for the landing `README.md` and ten per-sentinel deep-dive pages. Written for the online designer who will polish the layout after the real portraits are generated on OpenArt.

---

## Summary

The landing page reads like a video game character-select screen: a banner establishes the world, a concept block states what the sentinels are in one paragraph, and a roster grid lets the visitor pick a character to go deeper. Each sentinel has one portrait, one accent color, one language badge, and one status indicator. Ten characters, ten colors, one team.

---

## Color Tokens

All colors go through the design system. Never hard-code a one-off value.

| Token | Hex | Role |
|-------|-----|------|
| `--bg-base` | `#0d0f14` | page background |
| `--bg-surface` | `#131720` | section backgrounds |
| `--bg-card` | `#161c28` | card fill |
| `--bg-card-hover` | `#1c2438` | card hover / active fill |
| `--border` | `rgba(255,255,255,0.06)` | card and divider borders |
| `--text-primary` | `#e2e2e8` | headings and body |
| `--text-secondary` | `#8892a0` | secondary labels, captions |
| `--text-code` | `#94a3b8` | inline code, language badges |

Per-sentinel accent tokens:

| Sentinel | Token | Hex |
|----------|-------|-----|
| Sol Bot Sentinel | `--accent-amber` | `#f59e0b` |
| Sentry Engine | `--accent-blue` | `#3b82f6` |
| Candle Watch | `--accent-orange` | `#fb923c` |
| Obligation Reader | `--accent-jade` | `#34d399` |
| Corpus Sentinel | `--accent-purple` | `#a78bfa` |
| Tribal Knowledge Watcher | `--accent-cyan` | `#22d3ee` |
| Directive Sync Watcher | `--accent-violet` | `#8b5cf6` |
| Veritas Sentinel | `--accent-red` | `#ef4444` |
| Mercy | `--accent-green` | `#4ade80` |
| FX Watch | `--accent-yellow` | `#fbbf24` |

The accent is used for: card left-border, eye glow in the portrait, the status badge, and the name heading on the deep-dive page. One sentinel, one color, everywhere.

---

## Type Tokens

No external font imports. Use the system stack.

| Token | Value | Use |
|-------|-------|-----|
| `--font-display` | `ui-monospace, SFMono-Regular, "SF Mono", Menlo, monospace` | headings, sentinel names, labels |
| `--font-body` | `system-ui, -apple-system, sans-serif` | description text, body paragraphs |
| `--size-title` | `3rem` / `letter-spacing: 0.18em` / `text-transform: uppercase` | "SENTINELS" banner title |
| `--size-sentinel-name` | `1rem` / `letter-spacing: 0.12em` / `uppercase` | card and deep-dive heading |
| `--size-body` | `0.9rem` / `line-height: 1.65` | descriptions |
| `--size-label` | `0.7rem` / `letter-spacing: 0.1em` / `uppercase` | language badge, status badge |

---

## Landing README -- Page Layout

```txt
+----------------------------------------------------------+
|  _images/banner.svg  (full width, 1600x400)              |
+----------------------------------------------------------+
|                                                          |
|  CONCEPT BLOCK                                           |
|  One paragraph. What a sentinel is, what it is not.     |
|  The "turtles" metaphor. 40rem max-width, centered.     |
|                                                          |
+----------------------------------------------------------+
|                                                          |
|  ROSTER  (heading: "THE ROSTER")                        |
|                                                          |
|  [card] [card] [card] [card] [card]                     |
|  [card] [card] [card] [card] [card]                     |
|                                                          |
|  5 columns on desktop, 2 on mobile.                     |
|                                                          |
+----------------------------------------------------------+
```

The concept block keeps the visitor from clicking without knowing what the sentinels do. The roster follows immediately -- no extra sections between them.

---

## Card Anatomy

Each roster card links to its sentinel's deep-dive page.

```txt
+---------------------------+
|                           |  <-- left border 3px, accent color
|   [portrait image]        |  <-- aspect ratio 3:4, object-fit cover
|                           |      matches OpenArt output ratio
+---------------------------+
|  [LANGUAGE BADGE]         |  <-- monospace, uppercase, accent bg low-opacity
|                           |
|  SENTINEL NAME            |  <-- monospace, uppercase, accent color text
|                           |
|  One-line description     |  <-- body font, secondary text color
|                           |
|  [STATUS]                 |  <-- RUNNING / BUILDING / DESIGNED
+---------------------------+
```

Card dimensions: 200px wide (portrait) on desktop. The 3:4 portrait makes cards 200x267px for the image slot.

Hover state: `--bg-card-hover` fill, left border brightens (opacity 1.0 vs 0.6 at rest), slight y=-2px translate. No other animation.

Status badge:
- `RUNNING` -- accent color fill, dark text
- `BUILDING` -- amber `#f59e0b` fill, dark text
- `DESIGNED` -- gray `#374151` fill, `--text-secondary` text

---

## Deep-Dive Page Layout

Each sentinel has its own file linked from the roster card. The Architect is writing these files under `_deep-dives/`.

```txt
+----------------------------------------------+
|  [portrait, 3:4, max 360px wide, left-float] |
|                                              |
|  # SENTINEL NAME                             |  <-- accent color, monospace
|  [LANGUAGE BADGE]  [STATUS BADGE]           |
|                                              |
|  One-paragraph introduction.                 |
|  What it watches. What it cannot do.         |
|                                              |
|  ## HOW TO RUN IT                            |
|  code block                                  |
|                                              |
|  ## WHAT IT WATCHES                          |
|  bulleted list                               |
|                                              |
|  [back to roster link]                       |
+----------------------------------------------+
```

The portrait float clears after the intro paragraph. Headings are uppercase monospace. Code blocks use the dark theme verbatim.

---

## Portrait Image Spec

The OpenArt prompts in `_design/openart-prompts/` specify the exact generation parameters. Key points for the designer:

- **`A`**: `Aspect Ratio` -- 3:4 in OpenArt, matching the card slot. Crop to the same ratio if adjustment is needed.
- **`B`**: `Shared Palette` -- near-black background (#0d0f14 or equivalent), metallic dark body, single saturated accent for the eye glow.
- **`C`**: `Team Coherence` -- all ten portraits use the same style block (see `_design/openart-prompts/README.md`). A visitor should recognize them as one roster without reading the names.
- **`D`**: `Language Emblem` -- each portrait includes a marking, emblem, or texture hinting at the language (hex grid for Rust, sharp angles for C++, circuit trace for TypeScript/JavaScript, etc.). This is a visual cue, not text.
- **`E`**: `Eye Glow Color` -- must match the token table above exactly. The generated eye glow is the one element that must match the UI accent color. Adjust in post if generation shifts the hue.

---

## Brief for the Online Designer

You are receiving ten placeholder SVG portraits in `_images/` and this spec. The actual portraits will be generated on OpenArt using the prompts in `_design/openart-prompts/`.

Your work:

- **`A`**: `Replace Placeholders` -- swap each SVG portrait for the OpenArt PNG once generated. Keep the 3:4 aspect ratio.
- **`B`**: `Build the Card Grid` -- implement the roster grid in the README or in a separate HTML page, using the card anatomy above.
- **`C`**: `Apply the Color System` -- use only the tokens above. No one-off colors.
- **`D`**: `Check Eye Glow Match` -- if any generated portrait has an eye color that drifts from the accent token, flag it. The portrait color and the UI accent must read as the same color to the eye.
- **`E`**: `Mobile` -- the 5-column grid collapses to 2 on narrow viewports. The banner scales without cropping -- use `object-fit: contain` or `width: 100%`.
- **`F`**: `Accessibility` -- every portrait image has an alt text of the form "Portrait of {Sentinel Name}, a {language} sentinel". Status badges use sufficient contrast against their fill.
- **`G`**: `No External Fonts` -- the font stack above is system-only. No Google Fonts imports, no CDN links.
- **`H`**: `No Hard-Coded Colors` -- every color reference goes through the token table. The design system must be patchable in one place.

The repo is public. Nothing in any page may reference a client name, a private URL, a task platform, or a frontier model lab as a client.
