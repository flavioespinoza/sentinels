#!/usr/bin/env python3
"""Build the comic-portraits OpenArt prompt spec for the ten sentinels.

Edit the STYLE, NEGATIVE, MODEL, and CHARACTERS data below, then run:

    python3 scripts/comic-prompts/build-spec.py

It writes _design/SPEC__sentinels--openart-prompts--comic-portraits.md:
one section per sentinel, every prompt in its own quote block, ready to
paste into OpenArt. Drafts only; nothing here generates an image.
"""

from pathlib import Path

HERE = Path(__file__).resolve().parent
ROOT = HERE.parent.parent
OUT = ROOT / "_design" / "SPEC__sentinels--openart-prompts--comic-portraits.md"
STAMP = "v1 | Oct 20 2026 - 11:24 PM (MDT)"

MODEL = "Qwen Image 3"
MODEL_LIMIT = "4,500 tokens; the longest prompt here is under 1,900 characters"
FALLBACK = "Seedream 5.0 Pro, 4,000 character cap"

STYLE = (
    "Comic-book hero illustration, one humanoid character, full figure, three-quarter view, low camera angle. "
    "Bold black ink line work, clean cel shading with two-tone shadows, flat saturated color fills, hard-edged highlights, halftone dots in the shadows. "
    "Hero-shooter character design: exaggerated readable silhouette, strong pose, expressive face, a personality you can read at thumbnail size. "
    "Armored costume with one saturated accent color used for the eyes, the glow, and one stripe on the armor. "
    "Background: flat dark panel #0d0f14 with one radial burst of the accent color behind the figure, no scenery. "
    "Aspect ratio 3:4."
)

NEGATIVE = (
    "photorealistic, painted, soft blurry shading, text, letters, numbers, logo, watermark, signature, "
    "white background, muddy colors, cluttered background, extra limbs, deformed hands, anime"
)

CHARACTERS = [
    {
        "slug": "sol-bot-sentinel",
        "name": "Sol Bot Sentinel",
        "element": "wide horizontal visor; watchtower silhouette",
        "silhouette": "tallest, widest shoulders, tower head",
        "accent_name": "amber",
        "accent": "#f59e0b",
        "language": "C#",
        "role": "the lookout",
        "block": (
            "Sol Bot Sentinel, the lookout. A tall woman in her forties with a soldier's bearing, the tallest and widest-shouldered hero of the roster, "
            "dark skin, a hard calm jaw, close-cropped grey hair at the temples. Polished gunmetal plate armor, military grade, with empty insignia mounts on the chest. "
            "Her helmet is a wide trapezoid, wider than her shoulders, a watchtower worn as a helmet, and across its full width runs one horizontal amber visor slit (#f59e0b) that hides her eyes and lights her face from above. "
            "One amber stripe runs down the left arm. She stands with arms crossed, weight even, the stance of a guard who moves only when something crosses a line. "
            "Accent glow: amber, visor and stripe only."
        ),
        "negative": "",
        "variations": [
            ("Hero Pose", "Full figure, arms crossed, chin slightly down, the wide amber visor reading as a horizon line across the frame. The brightest point is the visor."),
            ("On Watch", "Full figure, standing at the edge of a ledge, one hand raised flat above the visor as if shading her eyes, the other resting on her hip. Body turned three-quarter, head turned toward the viewer."),
            ("Roster Bust", "Cropped at mid-torso, the trapezoid helmet filling the upper frame, the amber slit at center, her jaw and mouth lit from above. Direct, unmoving."),
        ],
    },
    {
        "slug": "veritas-sentinel",
        "name": "Veritas Sentinel",
        "element": "large circular camera-lens eye; recording lights on shoulders",
        "silhouette": "lean, forward lean, long plated coat",
        "accent_name": "red",
        "accent": "#ef4444",
        "language": "JavaScript",
        "role": "the recorder",
        "block": (
            "Veritas Sentinel, the recorder. A lean man in his fifties, grizzled, weathered olive skin, a grey stubbled jaw, a long worn plated coat of older-generation armor with patched seams and paint rubbed off the edges. "
            "Over his left eye sits a large circular camera lens, bigger than a fist, bolted to the side of his head, with concentric metal rings and a glowing red pupil (#ef4444); his right eye is bare and tired and sharp. "
            "A small red recording light glows on each shoulder pauldron. One red stripe runs down the coat's left edge. "
            "He leans forward, one hand raised to the lens as if pulling focus, the other loose at his side, the posture of a man who misses nothing and writes it all down. "
            "Accent glow: red, lens, shoulder lights, and stripe only."
        ),
        "negative": "",
        "variations": [
            ("Hero Pose", "Full figure, forward lean, hand at the lens, coat hanging open, both shoulder lights visible. The lens is the brightest point."),
            ("Pulling Focus", "Full figure, turned three-quarter away then looking back over his shoulder at the viewer, the lens eye catching the light, the coat swinging."),
            ("Roster Bust", "Cropped at mid-torso, the lens eye large in frame with its concentric rings and red pupil, the bare right eye narrowed beside it, one shoulder light at the frame edge."),
        ],
    },
    {
        "slug": "tribal-knowledge",
        "name": "Tribal Knowledge",
        "element": "dish antenna on helmet",
        "silhouette": "slim, crouched, dish crown",
        "accent_name": "cyan",
        "accent": "#22d3ee",
        "language": "TypeScript",
        "role": "the listener",
        "block": (
            "Tribal Knowledge, the listener. A slim young woman in her twenties, East Asian, bright alert face, short black hair swept to one side, light scout armor with clean angular seams and one circuit-board trace on the right shoulder. "
            "Her helmet is a wide low headset with a small parabolic dish antenna mounted on top, tilted up and forward as if catching something out of the air; a cyan triangle lens (#22d3ee) sits over her left eye, her right eye bare. "
            "One cyan stripe runs across the chest plate. She is down on one knee, one hand cupped behind her ear, head tilted, listening to something off frame. "
            "Accent glow: cyan, triangle lens, dish rim, and stripe only."
        ),
        "negative": "",
        "variations": [
            ("Hero Pose", "Full figure, one knee down, hand cupped at the ear, the dish antenna extending the silhouette upward. The cyan lens is the brightest point."),
            ("Signal Found", "Full figure, springing up from the crouch, pointing off frame with one hand, the dish swinging toward the same direction, face lit with recognition."),
            ("Roster Bust", "Cropped at mid-torso, the headset and dish antenna crowning the frame, the cyan triangle lens at center, a half smile."),
        ],
    },
    {
        "slug": "sentry-engine",
        "name": "Sentry Engine",
        "element": "cube helmet; heaviest armor of the set",
        "silhouette": "widest body, cube head, short legs",
        "accent_name": "steel blue",
        "accent": "#3b82f6",
        "language": "C++",
        "role": "the tank",
        "block": (
            "Sentry Engine, the tank. A huge heavy man in his thirties, the widest and heaviest hero of the roster, short legs, enormous arms, a broad bearded face. "
            "He wears a massive cube-shaped helmet almost as wide as his shoulders, its faceplate open to show his face, four structural bolts at the corners, heat-sink fins across the top, bundles of exposed wiring at the neck joint. "
            "Tank-grade layered shoulder plates, a round speaker grille set into the chest plate, one steel-blue stripe (#3b82f6) across the chest, and steel-blue glowing eyes. "
            "He stands planted with feet wide and both fists pressed together in front of his chest, immovable. "
            "Accent glow: steel blue, eyes, grille, and stripe only."
        ),
        "negative": "thin build, narrow shoulders",
        "variations": [
            ("Hero Pose", "Full figure, feet wide, fists together at the chest, the cube helmet and shoulders reading as the biggest silhouette in the set."),
            ("Hold The Line", "Full figure, braced low with both forearms raised like a wall, head down, the heat-sink fins on top catching the light, the chest grille glowing."),
            ("Roster Bust", "Cropped at chest level, the cube helmet filling the frame, the bearded face inside, bolt corners at the frame edges, the blue eyes steady."),
        ],
    },
    {
        "slug": "candle-watch",
        "name": "Candle Watch",
        "element": "lantern hanging from left shoulder",
        "silhouette": "medium, rounded helm, one-sided plated cape",
        "accent_name": "warm orange",
        "accent": "#fb923c",
        "language": "Rust",
        "role": "the lamplighter",
        "block": (
            "Candle Watch, the lamplighter. An older man in his sixties, a night watchman, lined brown face, white beard trimmed short, calm patient eyes. "
            "Medium build, a rounded helm with softened corners, a short plated cape hanging over the left shoulder, and from that shoulder a geometric lantern on a short chain, angular glass panels around a warm orange flame core (#fb923c) that lights his face from below. "
            "One rust-red engraved circuit trace on the right shoulder plate, one orange stripe down the cape's edge. "
            "He stands with the lantern raised high in his left hand, looking into the dark ahead, the pose of a man checking that every light is still burning. "
            "Accent glow: warm orange, lantern flame and stripe only."
        ),
        "negative": "",
        "variations": [
            ("Hero Pose", "Full figure, lantern raised high in the left hand, cape swinging, face lit from below by the flame, which is the brightest point."),
            ("Making Rounds", "Full figure, mid-stride, the lantern swinging from the shoulder chain at his side, head turned toward the viewer, one hand raised in a small wave."),
            ("Roster Bust", "Cropped at mid-torso, the lantern hanging at the lower frame edge throwing warm light up onto the rounded helm and the lined face above."),
        ],
    },
    {
        "slug": "obligation-reader",
        "name": "Obligation Reader",
        "element": "ledger plate on chest; diamond-shaped eye",
        "silhouette": "tall and thin, long plated skirt, open book",
        "accent_name": "jade",
        "accent": "#34d399",
        "language": "Rust",
        "role": "the auditor",
        "block": (
            "Obligation Reader, the auditor. A tall thin woman in her thirties, pale, severe and elegant, dark hair pulled back tight, a scholar's long plated skirt and narrow shoulders, the thinnest silhouette in the roster. "
            "Her helmet is a flat rectangular hood with thin horizontal scan lines etched across the brow like ledger rules; a diamond-shaped jade lens (#34d399) sits over her right eye, her left eye bare and exacting. "
            "On her chest a recessed ledger plate with faint ruled jade lines. One rust-red engraved circuit trace on the left shoulder, one jade stripe down the skirt. "
            "In her left hand an open geometric book spilling jade light; her right hand is raised with two fingers up, comparing two values. "
            "Accent glow: jade, lens, ledger plate, book pages, and stripe only."
        ),
        "negative": "",
        "variations": [
            ("Hero Pose", "Full figure, book open in the left hand, two fingers raised on the right, head tilted a few degrees as if cross-checking. The book is the brightest point."),
            ("The Delta", "Full figure, holding the open book out toward the viewer with both hands, the jade light from the pages lighting her face from below, one eyebrow raised."),
            ("Roster Bust", "Cropped at mid-torso, the flat hood and scan lines filling the upper frame, the diamond jade lens at center, the ledger plate glowing at the lower edge."),
        ],
    },
    {
        "slug": "corpus-sentinel",
        "name": "Corpus Sentinel",
        "element": "arch-shaped helmet like a gate",
        "silhouette": "square and broad, door-shaped shield on the forearm",
        "accent_name": "violet-purple",
        "accent": "#a78bfa",
        "language": "Rust",
        "role": "the gatekeeper",
        "block": (
            "Corpus Sentinel, the gatekeeper. A broad square man in his forties, Black, shaved head, heavy brow, unreadable face. "
            "His helmet is shaped like an arch, a stone doorway with a keystone at the top, framing his face as if he stands inside a gate; his eyes glow violet-purple (#a78bfa). "
            "Square heavy armor, a tall door-shaped tower shield strapped to the left forearm, one rust-red engraved circuit trace on the right shoulder, one violet-purple stripe down the center of the shield. "
            "He stands square to the viewer, shield planted on the ground, right hand raised palm out: nothing passes without authorization. "
            "Accent glow: violet-purple, eyes and shield stripe only."
        ),
        "negative": "",
        "variations": [
            ("Hero Pose", "Full figure, shield planted, right palm raised toward the viewer, feet set in a doorway stance. The eyes are the brightest point."),
            ("Gate Open", "Full figure, shield swung aside on the left arm, right hand gesturing someone through, head turned to watch them pass, the arch helmet in three-quarter."),
            ("Roster Bust", "Cropped at shoulder level, the arch helmet and keystone filling the frame, the face inside the arch, violet-purple eyes level and steady."),
        ],
    },
    {
        "slug": "directive-sync-watcher",
        "name": "Directive Sync Watcher",
        "element": "pointed angular helmet with spike",
        "silhouette": "narrow and tall, blade crest, hands behind back",
        "accent_name": "violet",
        "accent": "#8b5cf6",
        "language": "TypeScript",
        "role": "the enforcer",
        "block": (
            "Directive Sync Watcher, the enforcer. A narrow tall figure in their thirties, androgynous, sharp cheekbones, pale, composed. "
            "The helmet is angular and pointed, a trapezoid with a blade-like spike rising from the top center like a crest; both eyes are diamond-shaped violet lenses (#8b5cf6). "
            "A precise fitted uniform of clean armor plates with tight seams, one circuit-board trace on the left shoulder, one violet stripe down the spine of the helmet spike and the back. "
            "Stands perfectly upright, hands clasped behind the back, weight even, the posture of a process that never deviates from its rules. "
            "Accent glow: violet, eyes and stripe only."
        ),
        "negative": "",
        "variations": [
            ("Hero Pose", "Full figure, hands behind the back, chin level, the helmet spike reading clearly against the burst. The eyes are the brightest point."),
            ("Correction", "Full figure, one hand brought forward with a single finger raised, the other still behind the back, head tilted one degree, the spike in slight profile."),
            ("Roster Bust", "Cropped at shoulder level, the pointed helmet and spike filling the upper frame, the two diamond violet lenses at center, lips a flat line."),
        ],
    },
    {
        "slug": "mercy",
        "name": "Mercy",
        "element": "medic cross on forehead; two visible eyes; shorter proportions",
        "silhouette": "short and barrel-chested, old-model, patched",
        "accent_name": "soft green",
        "accent": "#4ade80",
        "language": "JavaScript",
        "role": "the veteran",
        "block": (
            "Mercy, the veteran, the oldest hero of the roster. A short stocky man in his seventies, barrel-chested, bow-legged, a broad weathered face with two visible kind eyes and deep laugh lines, a white moustache. "
            "Old-model patched armor of an earlier generation, paint worn through, repairs visible at every joint, a battered round helmet with an old headlamp on the side and a small soft-green medic cross (#4ade80) raised on the forehead plate. "
            "One soft-green stripe across the chest. A coil of cable on one hip. He stands with arms slightly out and open, feet planted, the protective stance of a dock watchman standing between something fragile and something dangerous. "
            "He is a lookout, not a healer. Accent glow: soft green, cross, headlamp, and stripe only."
        ),
        "negative": "wings, halo, staff, angel, white and gold costume, medical bag, syringe, nurse",
        "variations": [
            ("Hero Pose", "Full figure, arms open and slightly forward, feet planted, chin up, the forehead cross and headlamp glowing. Warm, grounded, unhurried."),
            ("First Watch", "Full figure, sitting on an overturned crate with one elbow on a knee, the headlamp on, looking out past the viewer, the oldest man on the team keeping the first watch."),
            ("Roster Bust", "Cropped at shoulder level, the battered helmet and the small green cross filling the upper frame, the two eyes crinkled, the moustache, the laugh lines."),
        ],
    },
    {
        "slug": "fx-watch",
        "name": "FX Watch",
        "element": "camera lens aperture as the face; concentric ring eye",
        "silhouette": "compact and boxy, mid-stride",
        "accent_name": "yellow",
        "accent": "#fbbf24",
        "language": "Shell",
        "role": "the courier",
        "block": (
            "FX Watch, the courier. A compact quick young man in his early twenties, wiry, the smallest hero of the roster, in a matte utilitarian jumpsuit of flat grey plates with no decoration at all. "
            "His whole face is a camera lens: a full-face visor helmet whose front is one large lens with five concentric metal rings and eight aperture blades around the outer ring, a bright yellow core (#fbbf24) at the center. "
            "One yellow stripe down the right leg. Caught mid-stride, leaning forward, one hand snapping shut around a flat rectangular plate as if catching a file out of the air. "
            "Accent glow: yellow, lens core and stripe only."
        ),
        "negative": "",
        "variations": [
            ("Hero Pose", "Full figure, mid-stride, catching the plate, the lens face turned toward the viewer. The yellow core is the brightest point."),
            ("Five Seconds", "Full figure, skidding to a stop and stamping the plate flat against a wall with one hand, the lens face in profile, the aperture blades visible."),
            ("Roster Bust", "Cropped at mid-torso, the lens face filling the frame, the concentric rings and aperture blades in sharp ink, the yellow core at the exact center."),
        ],
    },
]


def negative_for(character: dict) -> str:
    extra = character["negative"].strip()
    return f"{NEGATIVE}, {extra}" if extra else NEGATIVE


def joined(style: str, block: str, variation: str, negative: str) -> str:
    return f"{style} {block} {variation} Exclude: {negative}."


def quote(text: str) -> str:
    return "> " + text


def section(index: int, character: dict) -> list:
    negative = negative_for(character)
    lines = []
    lines.append(f"## {index}. {character['name']}")
    lines.append("")
    lines.append(f"- **`A`**: `Role` -- {character['role']}.")
    lines.append(f"- **`B`**: `Accent` -- {character['accent_name']} `{character['accent']}`, the only saturated color in the image.")
    lines.append(f"- **`C`**: `Language Cue` -- `{character['language']}`.")
    lines.append("")
    for number, (title, text) in enumerate(character["variations"], start=1):
        full = joined(STYLE, character["block"], text, negative)
        lines.append(f"### {index}.{number} {title} ({len(full)} characters)")
        lines.append("")
        lines.append(quote(full))
        lines.append("")
        if number == 1:
            lines.append("Negative prompt, if the model shows a negative field (the same exclusions already sit at the end of every prompt):")
            lines.append("")
            lines.append(quote(negative))
            lines.append("")
    return lines


def main() -> None:
    lines = []
    lines.append("# SPEC: Design -- OpenArt Prompts, Comic Portraits")
    lines.append("")
    lines.append("**File:** `SPEC__sentinels--openart-prompts--comic-portraits.md`")
    lines.append("")
    lines.append(STAMP)
    lines.append("")
    lines.append("**Status:** Draft")
    lines.append("**Type:** Design Spec")
    lines.append("**Created:** Oct 20 2026")
    lines.append("**See-Also:** `openart-prompts/README.md`, the dark mech set this replaces for the public roster")
    lines.append("")
    lines.append("---")
    lines.append("")
    lines.append("## Summary")
    lines.append("")
    lines.append("Ten comic-book hero portraits, one per sentinel, in the design language of a hero-shooter roster: bold inks, cel shading, one accent color each, ten humanoid heroes whose silhouettes read as a team. Every prompt Flavio pastes into OpenArt is in a quote block below, style and character already joined, one paste per image. Drafts; Flavio reviews before anything is generated.")
    lines.append("")
    lines.append("## How To Use")
    lines.append("")
    lines.append(f"- **`A`**: `Model` -- {MODEL} in the OpenArt Image Models menu. Prompt limit {MODEL_LIMIT}. Fallback: {FALLBACK}.")
    lines.append("- **`B`**: `Aspect Ratio` -- 3:4, set in the OpenArt dropdown for the model. The model page does not list its ratios; confirm 3:4 is offered in the UI.")
    lines.append("- **`C`**: `Negative Prompt` -- the model page shows no negative field, so the exclusions ride at the end of every prompt after the word Exclude. If a negative field is there, paste the negative quote block into it as well.")
    lines.append("- **`D`**: `One Paste` -- copy the text of one quote block, paste, generate. Three prompts per sentinel: the hero pose, the action pose, the roster bust.")
    lines.append("- **`E`**: `Inspired, Not Copied` -- no game hero names, logos, emblems, or recognizable designs appear in any prompt. The images go public on flavioespinoza.github.io/sentinels.")
    lines.append("- **`F`**: `No Text In The Image` -- text, letters, numbers, and logos are excluded in every prompt; insignia mounts stay empty.")
    lines.append("- **`G`**: `Rebuild` -- the prompts are data in `scripts/comic-prompts/build-spec.py`; edit there and run it to rewrite this file.")
    lines.append("")
    lines.append("## Shared Style Block")
    lines.append("")
    lines.append("Already joined into every prompt below; shown here so it can be edited in one place.")
    lines.append("")
    lines.append(quote(STYLE))
    lines.append("")
    lines.append("## Roster")
    lines.append("")
    lines.append("| # | Sentinel | Accent | Special Element (kept) | Silhouette | Role |")
    lines.append("|---|----------|--------|------------------------|------------|------|")
    for index, c in enumerate(CHARACTERS, start=1):
        lines.append(f"| {index} | {c['name']} | {c['accent_name']} `{c['accent']}` | {c['element']} | {c['silhouette']} | {c['role']} |")
    lines.append("")
    lines.append("---")
    lines.append("")
    for index, character in enumerate(CHARACTERS, start=1):
        lines.extend(section(index, character))
        lines.append("---")
        lines.append("")
    lines.append("## Change Log")
    lines.append("")
    lines.append("| Date | Version | Change |")
    lines.append("|------|---------|--------|")
    lines.append("| Oct 20 2026 | v1 | First draft: ten humanoid heroes, OpenArt one-paste prompts in quote blocks. Claude Artist, iMac, on Claude Helper's brief. |")
    OUT.write_text("\n".join(lines).rstrip() + "\n", encoding="utf-8")
    longest = max(len(joined(STYLE, c["block"], v[1], negative_for(c))) for c in CHARACTERS for v in c["variations"])
    print(f"wrote {OUT.name} ({len(lines)} lines), longest paste {longest} chars")


if __name__ == "__main__":
    main()
