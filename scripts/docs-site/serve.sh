#!/bin/zsh
# serve.sh -- serve the docsify site from the repo root on localhost, the way GitHub Pages serves it.
# Usage: scripts/docs-site/serve.sh [port]   (default 5190), then open http://localhost:5190/
cd "$(dirname "$0")/../.." && exec python3 -m http.server "${1:-5190}" --bind 127.0.0.1
