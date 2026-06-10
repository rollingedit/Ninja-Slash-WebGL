#!/usr/bin/env python3
"""
Fingerprint and sanity-check a Unity Web Player / Unity asset bundle candidate.
This script does not decrypt, crack, or modify anything. It reads metadata,
prints hash/file information, and optionally extracts printable strings for audit.
"""
from __future__ import annotations

import argparse
import hashlib
import json
import os
import re
import sys
from pathlib import Path

MAGICS = [b"UnityWeb", b"UnityRaw", b"UnityFS"]
SUSPICIOUS_TERMS = [
    "hack", "hacked", "cheat", "money", "coin", "currency", "upgrade", "shop",
    "purchase", "cost", "kongregate", "facebook", "advert", "mission", "zombie",
]


def sha256_file(path: Path, chunk_size: int = 1024 * 1024) -> str:
    h = hashlib.sha256()
    with path.open("rb") as f:
        while True:
            data = f.read(chunk_size)
            if not data:
                break
            h.update(data)
    return h.hexdigest()


def printable_strings(data: bytes, min_len: int = 5) -> list[str]:
    # ASCII/UTF-8-ish printable spans. Good enough for metadata triage.
    pattern = rb"[\x20-\x7E]{%d,}" % min_len
    return [m.group(0).decode("utf-8", "replace") for m in re.finditer(pattern, data)]


def detect_magic(data: bytes) -> str:
    for magic in MAGICS:
        if data.startswith(magic):
            return magic.decode()
    # Some cached files are wrappered; scan first 4 KiB.
    head = data[:4096]
    for magic in MAGICS:
        idx = head.find(magic)
        if idx >= 0:
            return f"{magic.decode()}@{idx}"
    return "unknown"


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("path", type=Path, help="Candidate .unity3d/.bundle/.asset file")
    ap.add_argument("--strings", action="store_true", help="Scan printable strings for triage")
    ap.add_argument("--out", type=Path, help="Write JSON manifest")
    args = ap.parse_args()

    path = args.path
    if not path.exists() or not path.is_file():
        print(f"ERROR: file not found: {path}", file=sys.stderr)
        return 2

    data_head = path.read_bytes()[:1024 * 1024]
    size = path.stat().st_size
    manifest = {
        "path": str(path),
        "filename": path.name,
        "size_bytes": size,
        "sha256": sha256_file(path),
        "magic": detect_magic(data_head),
        "first_32_bytes_hex": data_head[:32].hex(),
        "suspicious_string_hits": {},
    }

    if args.strings:
        # Limit memory on huge files: scan full file in chunks and keep unique terms/context.
        hits: dict[str, list[str]] = {term: [] for term in SUSPICIOUS_TERMS}
        tail = b""
        with path.open("rb") as f:
            while True:
                chunk = f.read(1024 * 1024)
                if not chunk:
                    break
                scan = tail + chunk
                for s in printable_strings(scan, 5):
                    sl = s.lower()
                    for term in SUSPICIOUS_TERMS:
                        if term in sl and len(hits[term]) < 20:
                            hits[term].append(s[:180])
                tail = chunk[-256:]
        manifest["suspicious_string_hits"] = {k: v for k, v in hits.items() if v}

    print(json.dumps(manifest, indent=2, ensure_ascii=False))
    if args.out:
        args.out.parent.mkdir(parents=True, exist_ok=True)
        args.out.write_text(json.dumps(manifest, indent=2, ensure_ascii=False), encoding="utf-8")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
