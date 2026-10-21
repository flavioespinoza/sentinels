# SPEC: Design -- OpenArt Prompts, Pencil And Watercolor

**File:** `SPEC__sentinels--openart-prompts--pencil-watercolor.md`

v1 | Oct 21 2026 - 01:37 AM (MDT)

**Status:** Draft
**Type:** Design Spec
**Created:** Oct 21 2026
**See-Also:** `DOC__sentinels--prompt-guide--pencil-watercolor.md` (Flavio's governing guide), `SPEC__sentinels--openart-prompts--comic-portraits.md` (the comic set this restyles)

---

## Summary

Nine OpenArt prompts, one per Sentinel, in the new house style: graphite pencil concept art with restrained watercolor highlights on textured paper, written to Flavio's prompt guide. Mercy is the established baseline and gets no new prompt here. The nine mix three humans, three machines, and three nonhuman creatures, each with one accent color no other Sentinel uses.

## How To Use

- **`A`**: `Model` -- Qwen Image 3 in the OpenArt Image Models menu. The limit is 4,500 tokens; every prompt here is between 900 and 1,800 characters.
- **`B`**: `Aspect Ratio` -- 3:4 portrait, set in the OpenArt dropdown for the model.
- **`C`**: `One Paste Per Image` -- copy the text of one Prompt quote block, paste, generate. The negative constraints already ride at the end of each prompt.
- **`D`**: `Negative Field` -- if OpenArt shows a negative prompt field for the model, paste that Sentinel's Negative Prompt quote block into it.

## Roster

| # | Sentinel | Species Or Body Type | Role | Accent | Signature Elements | Silhouette |
|---|----------|----------------------|------|--------|--------------------|------------|
| 1 | Sol Bot Sentinel | tall slender solar robot | the lookout | amber `#f59e0b` | wide horizontal visor slit; solar-cell watchtower crown; empty hands | tallest, narrow tower, flat roof head |
| 2 | Veritas Sentinel | human man, fifties | the recorder | red `#ef4444` | circular camera-lens eye; shoulder recording lamps; paper tape recorder | lean, forward lean, long coat |
| 3 | Tribal Knowledge | elderly humanoid gorilla | the listener | cyan `#22d3ee` | dish antenna headset; slate; hand drum at the hip | broad shoulders, long arms, dish crown |
| 4 | Sentry Engine | massive squat machine | the audio engine | steel blue `#3b82f6` | cube head with grille face; forearm dial | widest, cube head, short legs |
| 5 | Candle Watch | human man, sixties | the night watchman | candle-wax cream `#f4e4bc` | shoulder lantern with one candle; pocket watch | short, stooped, long coat, lantern |
| 6 | Obligation Reader | human woman, thirties | the auditor | jade `#34d399` | diamond loupe lens; ledger chest plate; two slates | tallest human, thinnest, long skirt |
| 7 | Corpus Sentinel | tortoise-like reptilian | the gatekeeper | violet-purple `#a78bfa` | arch helmet with keystone; door shield with token slot | square, domed shell, tall shield |
| 8 | Directive Sync Watcher | androgynous android | the copyist | magenta `#d946ef` | blade crest helmet; two bracket drones | symmetrical, upright, flanked by drones |
| 9 | Mercy | human woman, adult | the veteran lookout | orange `#f97316` | shoulder wing arrays; headset; cable coil (baseline, established) | full figure, wide blade wings |
| 10 | FX Watch | small four-armed ant-like insectoid | the courier | lime `#a3e635` | aperture lens face; card harness; hand stamp | smallest, four arms, mid-stride |

---

## 1. Sol Bot Sentinel

### Design Summary

Sol Bot Sentinel watches two trading machines from outside and can never touch a trade, so it is drawn as the tallest, stillest figure in the roster with open, empty hands. Its body is a narrow watchtower on legs, the flat roof of its head tiled with solar cells, and one wide amber visor slit reads as a horizon line. Three small chest lamps stand for its three watchers: trend, risk, and conduct. It is a robot with no face, which keeps it far from Mercy and from any human hero.

### Prompt

> Original Sentinel character design, Sol Bot Sentinel, a tall slender solar-powered robot, a read-only lookout that watches two trading machines and raises the alarm. Calm, patient, utterly still, a guard that observes and never touches. The tallest figure in the roster: a narrow tower-like body on long jointed legs and a flat squared head shaped like the top of a watchtower, its roof tiled with small dark solar cells. One wide horizontal amber visor slit runs the full width of the head; no mouth, no human face. Armor of overlapping weathered steel plates, bolted panels, exposed rivets, a ribbed flexible under-layer at the waist and knees, heat-shield tiles on the shoulders, cables routed along the spine. Three small round warning lamps sit in a vertical row on the chest plate, one lit. The hands are open and empty, holding no tool and no key. Pose: upright, weight even, one hand raised flat above the visor as if scanning the horizon, the other hanging loose. Full figure, three-quarter view, slightly low camera angle, feet visible, space above the head. Scratched paint worn through at plate edges, mismatched replacement panels, sun-faded steel. Amber appears only in the visor slit, the one lit lamp, and a thin stripe on the left forearm. Background: off-white textured paper with a faded vertical warm-gray watercolor field. Graphite pencil concept art with visible construction lines, fine hatching and crosshatching, light ink reinforcement on the silhouette, restrained watercolor washes, slight pigment bleeding, dry-brush edges. No wings, no weapons, no text.

### Negative Prompt

> photorealism, glossy 3D render, plastic toy look, anime style, copied franchise character, logo, watermark, signature, text, letters, numbers, cluttered background, muddy colors, extra limbs, deformed hands, cropped feet, wings, halo, staff, weapon, human face, orange palette

### Assumptions

- **`E`**: `Robot, Not Human` -- the guide offers solar charging and a machine body for Sol Bot; the comic set had a human woman, and a robot here gives the roster its tallest nonhuman silhouette.
- **`F`**: `Solar Cells On The Crown` -- the cells sit on the flat roof of the head rather than on the back, so nothing can read as a wing array.
- **`G`**: `Empty Hands Are The Role` -- the deep dive says it holds no key and cannot trade; the open hands make that visible.

### Later Refinements

- **`H`**: `Lamp Colors` -- if the three chest lamps read as decoration, try lighting all three in amber at different brightness.

---

## 2. Veritas Sentinel

### Design Summary

Veritas Sentinel is a black-box recorder for agent sessions, so he is a witness: a lean man in his fifties with a fist-sized camera lens bolted over one eye and a red recording lamp on each shoulder. A recorder unit on his chest spools blank paper tape, the visible form of the live log he writes. His long patched coat gives him a narrow vertical silhouette with a forward lean. He is the only Sentinel whose signature is a single oversized eye.

### Prompt

> Original Sentinel character design, Veritas Sentinel, a lean human man in his fifties, a black-box recorder who watches working machines and writes down every move they make. Dry, sharp, quietly skeptical, a witness who misses nothing. A wiry build with a slight forward lean and a long armored coat falling past the knees, giving a narrow vertical silhouette. Weathered olive skin, gray stubble, deep lines around a tired bare right eye; over his left eye a circular camera lens bigger than a fist, bolted to a bracket at the temple, with concentric metal rings and a glowing red pupil. The coat is old-generation plate sewn onto heavy canvas, with patched seams, worn leather straps, a dark flexible under-suit, reinforced gloves, scuffed knee plates, and heavy boots. A small red recording lamp glows on each shoulder plate. A flat rectangular recorder unit is strapped to his chest, and a long strip of blank paper tape spools out of it. Pose: one hand raised to the lens as if pulling focus, the other holding up a length of tape to read it. Full figure, three-quarter view, eye-level camera, feet visible, coat hem and tape fully in frame. Paint rubbed off plate edges, small dents, field-stitched repairs. Red appears only in the lens pupil, the two shoulder lamps, and one thin stripe down the coat edge. Background: off-white textured paper with a restrained cool-gray radial wash. Graphite pencil concept art with visible construction lines, fine hatching and crosshatching, light ink reinforcement on the silhouette, restrained watercolor washes, slight pigment bleeding, dry-brush edges. No writing on the tape, no weapons.

### Negative Prompt

> photorealism, glossy 3D render, anime style, copied franchise character, logo, watermark, signature, text, letters, numbers, writing on tape, cluttered background, muddy colors, extra limbs, deformed hands, cropped feet, wings, halo, weapon, eyepatch, pirate styling

### Assumptions

- **`I`**: `Kept From The Comic Set` -- the man in his fifties, the lens eye, and the shoulder lamps carry over; only the rendering changes.
- **`J`**: `Paper Tape For The Log` -- the deep dive describes a live Markdown record of every turn; blank tape shows recording without putting text in the image.
- **`K`**: `No Pirate Read` -- a single covered eye can drift toward a pirate; the negative prompt blocks it.

### Later Refinements

- **`L`**: `The Warning` -- a second pass could show the monologue alert: the tape pulled taut and the lens narrowed, red lamps brighter.

---

## 3. Tribal Knowledge

### Design Summary

Tribal Knowledge turns overheard conversation into written record, so it is an elderly gorilla archivist: patient, broad, and listening. A parabolic dish antenna rides on its headset, the signature carried from the comic set, and a slim slate in one hand holds what it hears. A small hand drum at the hip comes from the war drums that sound when a transcript lands. It is the roster's first nonhuman creature and its broadest organic silhouette.

### Prompt

> Original Sentinel character design, Tribal Knowledge, an elderly humanoid gorilla, a listener and archivist who turns overheard conversation into written record. Gentle, attentive, wise, patient. A broad heavy upper body, long powerful arms, short bowed legs, standing upright, silvered gray fur on the shoulders and back, a deeply lined leathery face, thoughtful dark eyes. On the head a low padded headset with a small parabolic dish antenna mounted on top, tilted up and forward as if catching a voice from the air, and a cyan triangular lens over the left eye. Practical field armor sized for gorilla anatomy: a padded vest of overlapping canvas and steel plates, a heavy leather apron, reinforced knuckle guards, forearm bracers, utility pouches stuffed with rolled notes. A small hand drum hangs at the right hip. One large hand holds a slim stone-gray slate; the other is cupped near the dish. Pose: head tilted toward the sound, eyes half closed in concentration, weight settled. Full figure, three-quarter view, eye-level camera, feet visible, the dish extending the silhouette upward. Worn canvas, scuffed plates, a mended strap, fur drawn with loose pencil strokes. Cyan appears only in the triangle lens, the dish rim, and one stripe across the vest. Background: off-white textured paper with a sparse muted gray-green watercolor panel. Graphite pencil concept art with visible construction lines, fine hatching and crosshatching, light ink reinforcement on the silhouette, restrained watercolor washes, slight pigment bleeding, dry-brush edges. No feathers, no masks, no costume stereotypes, no text.

### Negative Prompt

> photorealism, glossy 3D render, cartoon ape, anime style, copied franchise character, logo, watermark, signature, text, letters, numbers, cluttered background, muddy colors, extra limbs, deformed hands, cropped feet, wings, halo, weapon, feathers, masks, war paint

### Assumptions

- **`M`**: `Gorilla By Role` -- an elder who keeps what a group knows fits a silverback archivist; gorilla is one of the guide's named options.
- **`N`**: `Tribal Means Team Lore` -- "tribal knowledge" is knowledge that lives in a team's heads, so the prompt bans feathers, masks, and costume stereotypes.
- **`O`**: `Gender Left Open` -- the prompt says elderly gorilla and does not specify sex, so the model decides.

### Later Refinements

- **`P`**: `The Alert Pose` -- a second image could show it striking the hip drum, dish swung toward the viewer.

---

## 4. Sentry Engine

### Design Summary

Sentry Engine is the real-time audio engine under a voice-security sentry: deterministic, allocation-free, and never clicking when a knob moves. It is drawn as the heaviest machine in the roster, with short legs, a barrel body, and a cube head whose face is a round speaker grille. A large rotary dial on its forearm, turned slowly by two fingers, is the smoothed parameter change. It has no human face, which separates it from the comic set's bearded tank.

### Prompt

> Original Sentinel character design, Sentry Engine, a massive squat industrial machine with a person-like stance, the real-time audio engine that guards a voice from being copied. Steady, unflappable, immovable, precise. The widest and heaviest figure in the roster: short thick legs, a barrel body, enormous arms ending in heavy three-fingered hands. The head is a cube almost as wide as the shoulders, with a round speaker grille as its face, two small steel-blue optical sensors above the grille, four structural bolts at the corners, and heat-sink fins across the top. Layered tank-grade plates, exposed pistons at the elbows and knees, servo joints, bundled cables at the neck, shin guards like bollards. A large knurled rotary dial is set into the left forearm, and two fingers of the right hand rest on it, turning it slowly. Pose: feet planted wide, body square, head level, the calm of a machine that never jitters. Full figure, three-quarter view, low camera angle that emphasizes mass, feet visible, the full width of the shoulders in frame. Gunmetal and desaturated steel, chipped paint, oil stains, a welded repair seam across the chest plate. Steel blue appears only in the two sensors, a thin ring of light around the grille, and a thin line around the dial. Background: off-white textured paper with faint horizontal graphite waveform marks. Graphite pencil concept art with visible construction lines, fine hatching and crosshatching, light ink reinforcement on the silhouette, restrained watercolor washes, slight pigment bleeding, dry-brush edges. No human face, no thin build, no text.

### Negative Prompt

> photorealism, glossy 3D render, plastic toy look, anime style, copied franchise character, logo, watermark, signature, text, letters, numbers, cluttered background, muddy colors, extra limbs, deformed hands, cropped feet, wings, halo, weapon, human face, thin build, narrow shoulders

### Assumptions

- **`Q`**: `Machine, Not Man` -- the comic set's bearded man in a cube helmet becomes a machine whose cube head is the whole head, to put a second robot in the roster with the opposite silhouette to Sol Bot.
- **`R`**: `The Dial Is The Filter` -- the deep dive's coefficient smoothing is shown as a slowly turned knob, the one action that must never click.
- **`S`**: `Three Fingers` -- a three-fingered hand reads as engineered and lowers the chance of deformed human hands.

### Later Refinements

- **`T`**: `Waveform Strength` -- if the background marks compete with the figure, drop them to plain paper.

---

## 5. Candle Watch

### Design Summary

Candle Watch is the night watchman over a trading system's candles: it checks that every feed keeps arriving and keeps moving, and calls out the moment one goes stale. He is a short, stooped elderly man with a small lantern hanging from his shoulder, the special element kept from the comic set, holding one wax candle. An open pocket watch in his other hand is the staleness check. His accent moves from warm orange to candle-wax cream, set against a cool night wash so it reads.

### Prompt

> Original Sentinel character design, Candle Watch, a short elderly human man in his sixties, a night watchman who checks that every light keeps burning and calls out the moment one goes stale. Patient, vigilant, kind, a little weary. A short stooped but sturdy build, a long patched field coat with plated shoulders, a rounded helm with softened corners pushed back on his head. A lined brown face, a short white beard, calm narrowed eyes. From a hook on his left shoulder hangs a small geometric lantern, angular glass panels around one cream wax candle with a small pale flame, wax drips hardened along its frame. Armor: worn steel shoulder plates, a padded under-coat, reinforced gloves, knee plates, heavy scuffed boots. In his right hand an open brass pocket watch on a chain, which he checks while looking into the dark ahead. Pose: half turned toward the viewer, lantern swinging slightly, unhurried. Full figure, three-quarter view, eye-level camera, feet visible, the lantern fully in frame. Paint worn through at plate edges, mended seams, small dents. The accent is candle-wax cream: it appears only in the candle, the soft glow on the lantern glass and on his face, and one narrow stripe down the coat edge. Background: off-white textured paper with a cool slate-gray night wash behind the figure so the cream glow reads. Graphite pencil concept art with visible construction lines, fine hatching and crosshatching, light ink reinforcement on the silhouette, restrained watercolor washes, slight pigment bleeding, dry-brush edges. No orange glow, no fire effects, no numbers on the watch face.

### Negative Prompt

> photorealism, glossy 3D render, anime style, copied franchise character, logo, watermark, signature, text, letters, numbers, cluttered background, muddy colors, extra limbs, deformed hands, cropped feet, wings, halo, staff, weapon, orange palette, large fire, wizard robes

### Assumptions

- **`U`**: `Accent Changed` -- warm orange `#fb923c` from the comic set collides with Mercy's orange, so Candle Watch takes candle-wax cream `#f4e4bc` with a small pale flame.
- **`V`**: `Night Wash For Contrast` -- cream on off-white paper would vanish, so his background is a cool slate-gray night wash, the one darker field in the set.
- **`W`**: `Pocket Watch For Staleness` -- the deep dive's question is whether the feed is still arriving on time; a watch shows that without a chart or numbers.

### Later Refinements

- **`X`**: `Cream Strength` -- if the cream reads as plain paper, push it toward a faint honey tint while keeping it clear of Sol Bot's amber.
- **`Y`**: `Four States` -- a later pass could hang four small tags on the lantern for the four states: ok, stale, flat, and unreadable.

---

## 6. Obligation Reader

### Design Summary

Obligation Reader decodes the same on-chain record twice, in two languages, and pages when the two disagree, so she holds two blank slates side by side and compares them. She is a tall, thin, exacting woman in her thirties with a diamond-shaped jade lens on a hinged loupe, and the ledger plate on her chest carries over from the comic set. A lead seal on a cord at her belt is the guide's obligation cue. She is never a decision input, so she carries no weapon and no tool for acting, only the two readings.

### Prompt

> Original Sentinel character design, Obligation Reader, a tall thin human woman in her thirties, an auditor who reads the same record twice by two methods and flags any difference. Severe, exacting, composed, quietly elegant. The thinnest silhouette in the roster: narrow shoulders, long limbs, a long split plated skirt over a fitted under-suit. Pale skin, dark hair pulled back tight, a sharp brow; a diamond-shaped jade lens on a hinged loupe arm over her right eye, her left eye bare. A recessed ledger plate on the chest with faint ruled lines and no writing. Armor: slim overlapping plates with fine riveted seams, articulated vambraces, a narrow belt with a lead seal hanging from a cord, knee plates, tall heavy boots. In each hand she holds a slim blank slate, raised side by side at eye level, comparing them. Pose: upright and still, head tilted a few degrees, gaze moving between the two slates. Full figure, three-quarter view, eye-level camera, feet visible, both slates fully in frame. Fine scratches, a replaced shoulder plate in a slightly different gray, polished wear at the edges. Jade appears only in the diamond lens, the lines of the ledger plate, and a faint glow on the faces of both slates. Background: off-white textured paper with a faded vertical warm-gray watercolor field and two faint ruled graphite lines. Graphite pencil concept art with visible construction lines, fine hatching and crosshatching, light ink reinforcement on the silhouette, restrained watercolor washes, slight pigment bleeding, dry-brush edges. No book, no writing on the slates, no weapons.

### Negative Prompt

> photorealism, glossy 3D render, fashion illustration, anime style, copied franchise character, logo, watermark, signature, text, letters, numbers, writing on slates, cluttered background, muddy colors, extra limbs, deformed hands, cropped feet, wings, halo, weapon, book, blonde hair

### Assumptions

- **`Z`**: `Two Slates Replace The Book` -- the comic set gave her an open book; two slates show the parity check itself, two readings of one record.
- **`AA`**: `Human By Contrast` -- she is the roster's tall thin human, set against the gorilla and the reptilian, and her dark hair keeps her apart from Mercy.

### Later Refinements

- **`AB`**: `The Delta` -- a second image could show one slate glowing jade and the other dark, the moment the two readings part.

---

## 7. Corpus Sentinel

### Design Summary

Corpus Sentinel is the only door to a retrieval engine that stays private behind it, so it is a gatekeeper built like a tortoise: square, slow, shelled, and unmovable. Its arch-shaped helmet with a keystone and its door-shaped tower shield both carry over from the comic set. The shield gains one narrow lit slot, the place a bearer token is shown. "Corpus" also suggests a constructed shell, and the domed carapace is that shell.

### Prompt

> Original Sentinel character design, Corpus Sentinel, a broad heavyset reptilian being with a tortoise-like build, the gatekeeper who stands at the only door to a guarded archive. Unreadable, patient, absolutely immovable. Square and broad, short thick legs, a domed carapace on the back, thick scaled hide in olive-gray, a short beaked face with heavy-lidded eyes that glow violet-purple. The helmet is shaped like a stone arch with a keystone at the top, framing the face as if the creature stands inside a gateway. Armor: heavy square plates bolted over the hide, a scaled neck guard, broad clawed hands in reinforced gauntlets, heavy plates over the knees and feet. A tall door-shaped tower shield is strapped to the left forearm, with one narrow vertical slot in its center, a place to present a token, lit violet-purple from within. Pose: standing square to the viewer, shield planted on the ground, right hand raised palm out: nothing passes without a token. Full figure, slight three-quarter view, low camera angle, feet visible, the full height of the shield in frame. Chipped stone-like plates, worn bolts, scratches across the shield face, old scars in the scales. Violet-purple appears only in the eyes, the shield slot, and one thin stripe down the shield edge. Background: off-white textured paper with a muted gray watercolor panel suggesting a doorway outline. Graphite pencil concept art with visible construction lines, fine hatching and crosshatching, light ink reinforcement on the silhouette, restrained watercolor washes, slight pigment bleeding, dry-brush edges. No weapon, no dragon features, no text.

### Negative Prompt

> photorealism, glossy 3D render, cartoon turtle, anime style, copied franchise character, logo, watermark, signature, text, letters, numbers, cluttered background, muddy colors, extra limbs, deformed hands, cropped feet, wings, halo, weapon, dragon, ninja styling

### Assumptions

- **`AC`**: `Tortoise By Role` -- a slow, shelled, square creature is the natural gatekeeper; the shell doubles as the guide's "constructed shell" cue for Corpus.
- **`AD`**: `Token Slot` -- the deep dive's bearer token is drawn as a lit slot in the shield, with no symbol or text on it.
- **`AE`**: `No Turtle Heroes` -- the negative prompt blocks cartoon turtles and ninja styling to keep the design original.

### Later Refinements

- **`AF`**: `Gate Open` -- a second pose could swing the shield aside, one hand waving someone through.

---

## 8. Directive Sync Watcher

### Design Summary

Directive Sync Watcher carries one approved rule from a spec into the exact section of a ruleset between two boundary markers, and touches nothing else. It is a precise, symmetrical android with the blade-crested helmet from the comic set and two small drones shaped like square brackets hovering at its shoulders: the two markers. Its pose slides a blank panel into the gap between them. Its accent moves from violet to magenta so it no longer matches Corpus Sentinel.

### Prompt

> Original Sentinel character design, Directive Sync Watcher, an androgynous slender android, a mechanical copyist who carries one approved rule into the exact place it belongs and touches nothing else. Precise, composed, impartial, unhurried. A medium-height symmetrical build, narrow waist, perfectly upright posture. A smooth matte porcelain-gray face with no mouth, two diamond-shaped magenta optical lenses, and an angular pointed helmet with a single blade-like crest rising from the top center. Clean fitted plates with tight seams, visible servo joints at the wrists and knees, a flexible graphite-gray under-suit, cable routing down the spine, slim boots. Two small hovering drones shaped like square brackets float at shoulder height to either side, aligned and level, each with one magenta indicator lamp lit in sync. Pose: both hands sliding a thin blank panel into the gap between the two bracket drones, eyes fixed on the alignment. Full figure, three-quarter view, eye-level camera, feet visible, both drones fully in frame. Faint scuffs, a replaced forearm plate, precise but used. Magenta appears only in the two lenses, the two drone lamps, and one thin line down the crest. Background: off-white textured paper with a restrained radial gray wash and faint vertical timing marks. Graphite pencil concept art with visible construction lines, fine hatching and crosshatching, light ink reinforcement on the silhouette, restrained watercolor washes, slight pigment bleeding, dry-brush edges. No text on the panel, no extra drones, no weapons.

### Negative Prompt

> photorealism, glossy 3D render, plastic toy look, anime style, copied franchise character, logo, watermark, signature, text, letters, numbers, cluttered background, muddy colors, extra limbs, deformed hands, cropped feet, wings, halo, weapon, more than two drones, visible mouth

### Assumptions

- **`AG`**: `Accent Changed` -- violet `#8b5cf6` and Corpus Sentinel's violet-purple `#a78bfa` are two shades of one violet and would read as one color in watercolor, so Directive Sync Watcher takes magenta `#d946ef`.
- **`AH`**: `Bracket Drones Are The Markers` -- the guide suggests linked drones for Directive Sync; the deep dive's two boundary markers decide that there are exactly two.
- **`AI`**: `Android, Not Robot` -- a porcelain face and human proportions set it apart from the two heavier machines.

### Later Refinements

- **`AJ`**: `Debounce` -- a later pass could show it waiting, hands still, panel held just short of the gap.

---

## 10. FX Watch

### Design Summary

FX Watch renames every screenshot within seconds of its landing, so it is the smallest and quickest Sentinel: a four-armed, ant-like insectoid courier. Its face is the aperture lens from the comic set, and its two pairs of hands work at once, one pair catching a new card out of the air while the other stamps a name onto another. A harness of slim card slots shows the filing. Its accent moves from yellow to lime so it no longer matches Sol Bot's amber.

### Prompt

> Original Sentinel character design, FX Watch, a small four-armed ant-like insectoid, a courier and file clerk who renames every new arrival within seconds. Quick, eager, tidy, alert. The smallest figure in the roster: a compact segmented body, two thin jointed legs, two pairs of arms, a narrow waist, no wings. The head carries a single large camera-aperture lens in place of eyes, five concentric metal rings with eight aperture blades around a lime core, and two short antennae. Light courier armor fitted over dark matte chitin: small overlapping shoulder plates, a harness with rows of slim card slots, reinforced wrist guards, padded joints. The upper pair of hands catches a flat blank card out of the air while the lower pair presses a small hand stamp onto another card. Pose: mid-stride, leaning forward, already moving on. Full figure, three-quarter view, eye-level camera, all four arms and both feet visible. Scuffed chitin, a chipped lens ring, a mended harness strap. Lime appears only in the lens core, the stamp face, and one thin stripe down the harness. Background: off-white textured paper with a sparse pale gray watercolor panel. Graphite pencil concept art with visible construction lines, fine hatching and crosshatching, light ink reinforcement on the silhouette, restrained watercolor washes, slight pigment bleeding, dry-brush edges. No wings, no human face, no writing on the cards.

### Negative Prompt

> photorealism, glossy 3D render, cartoon bug, anime style, copied franchise character, logo, watermark, signature, text, letters, numbers, writing on cards, cluttered background, muddy colors, more than four arms, deformed hands, cropped feet, wings, halo, weapon, human face

### Assumptions

- **`AK`**: `Accent Changed` -- yellow `#fbbf24` and Sol Bot Sentinel's amber `#f59e0b` are adjacent amber shades and would read as one color, so FX Watch takes lime `#a3e635`.
- **`AL`**: `Ant-Like For No Wings` -- an ant has no wings, so the insectoid body cannot drift toward a winged design or Mercy's wing arrays.
- **`AM`**: `Four Arms On Purpose` -- the negative prompt says more than four arms rather than extra limbs, so the model keeps the second pair.

### Later Refinements

- **`AN`**: `Five Seconds` -- a second pose could show it waiting, lens half closed, a card hovering just above its hand.

---

## Change Log

| Date | Version | Change |
|------|---------|--------|
| Oct 21 2026 | v1 | First draft: nine Sentinels in pencil and watercolor, written to Flavio's prompt guide. Mercy kept as the established baseline. Accents changed for Candle Watch, Directive Sync Watcher, and FX Watch so no two of the ten share one. Claude Artist, iMac. |
