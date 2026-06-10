#!/usr/bin/env python3
"""Unpack files embedded inside the Ninja Slash UnityWeb bundle."""
from __future__ import annotations

import argparse
import json
import shutil
from pathlib import Path

import UnityPy


def reader_bytes(reader) -> bytes:
    pos = reader.tell()
    try:
        reader.seek(0)
        return reader.read_bytes(reader.Length)
    finally:
        reader.seek(pos)


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("--source", type=Path, default=Path("originals/ninjaslash_webplayer.unity3d"))
    ap.add_argument("--out", type=Path, default=Path("unpacked_unityweb"))
    ap.add_argument("--clean", action="store_true")
    args = ap.parse_args()

    if args.clean and args.out.exists():
        shutil.rmtree(args.out)
    args.out.mkdir(parents=True, exist_ok=True)

    env = UnityPy.load(str(args.source))
    manifest = {"source": str(args.source), "files": []}

    for bundle in env.files.values():
      for name, entry in bundle.files.items():
        out = args.out / name
        out.parent.mkdir(parents=True, exist_ok=True)
        if hasattr(entry, "reader"):
            data = reader_bytes(entry.reader)
        elif hasattr(entry, "save"):
            data = entry.save()
        else:
            data = reader_bytes(entry)
        out.write_bytes(data)
        manifest["files"].append({"name": name, "bytes": len(data)})

    manifest_path = args.out / "unpacked_manifest.json"
    manifest_path.write_text(json.dumps(manifest, indent=2), encoding="utf-8")
    print(json.dumps({"out": str(args.out), "files": len(manifest["files"]), "manifest": str(manifest_path)}, indent=2))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
