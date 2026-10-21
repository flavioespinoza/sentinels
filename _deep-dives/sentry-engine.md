# Sentry Engine

`C++`

## What It Is

The real-time audio engine under Veritas, a home security sentry: a thing you put on your phone so your voice cannot be cloned and turned against the people who trust it. Sentry Engine is the part that touches the audio. It is a node graph with parameters that smooth in two stages and a BiQuad filter built for the one place an allocation is a bug, the audio callback. It is the engine, not the detector: the detection layers that were meant to sit on it are designs, not code, and this page does not claim them.

## How I Work

- **`A`**: `The Filter` -- a `BiQuadFilter` on Transposed Direct Form II. Zero allocation in the processing path, per-sample coefficient smoothing so a parameter change never clicks, and a Nyquist clamp on the cutoff so a bad parameter cannot produce an unstable filter.
- **`B`**: `The Graph` -- an `AudioGraph` of nodes whose parameters smooth in two stages. The scheduler that walks the graph, `AudioGraph::processBlock()`, is a stub today, and this page says so rather than a quick start that pretends it runs.
- **`C`**: `The Discipline` -- Pimpl and RAII throughout, no raw `new` anywhere, CMake for the build.
- **`D`**: `The Proof` -- thirteen DSP tests, among them impulse response, a single-bin DFT to check the filter's frequency response, stereo parity, and bypass identity. A benchmark written so the compiler cannot eliminate the work it measures.

## What It Is Useful For

Anywhere audio is processed in a callback that must never block, never allocate, and never click when a knob moves. The filter is the kind of piece a real-time system is built from: deterministic, provable with tests rather than ears, and small enough to read in one sitting. The test harness is reusable on its own, since a single-bin DFT is a cheap, exact way to assert what any filter does at one frequency.

## Examples Of What You Could Use It For

- **`E`**: `An Intercom Or VoIP Front End` -- an equalizer or notch filter in the capture path, where the audio thread cannot afford a heap allocation and a parameter change from the UI must not produce a click.
- **`F`**: `A Wake-Word Pipeline On A Small Board` -- a band-pass stage that cleans microphone input before a detector, built with the same zero-allocation guarantee on hardware with no room for a garbage collector.
- **`G`**: `A DSP Test Bench` -- take the tests and the benchmark alone. The impulse response, single-bin DFT, stereo parity, and bypass identity checks are a template for proving any filter implementation before it goes near an ear.

## Status

Running in tests; the graph scheduler is a stub. The filter, its thirteen tests, and the benchmark are real and green. `processBlock()` is an empty TODO. The code dates from Jan 2026 by its commits and lives in a private repo; it will be rebuilt here as a fresh tree from source, tests, benchmarks, docs, and CMake only, never with the old history. An HTTP wrapper that runs the filter on an uploaded clip is the open question that would make it running for real.
