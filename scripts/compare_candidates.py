#!/usr/bin/env python3
"""Compare multiple candidate game artifacts by size and SHA-256."""
from __future__ import annotations
import argparse, hashlib
from pathlib import Path


def sha256(path: Path) -> str:
    h = hashlib.sha256()
    with path.open('rb') as f:
        for chunk in iter(lambda: f.read(1024*1024), b''):
            h.update(chunk)
    return h.hexdigest()


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument('files', nargs='+', type=Path)
    args = ap.parse_args()
    rows = []
    for p in args.files:
        if not p.is_file():
            rows.append((str(p), 'MISSING', '', ''))
            continue
        rows.append((str(p), p.stat().st_size, sha256(p), p.suffix.lower()))
    max_name = max(len(r[0]) for r in rows)
    for name, size, h, ext in rows:
        print(f"{name:<{max_name}}  size={size}  sha256={h}  ext={ext}")
    groups = {}
    for name, size, h, ext in rows:
        groups.setdefault(h, []).append(name)
    print('\n== Hash groups ==')
    for h, names in groups.items():
        print(h or 'NOHASH')
        for n in names:
            print(f"  - {n}")
    return 0

if __name__ == '__main__':
    raise SystemExit(main())
