#!/usr/bin/env python3
"""Extract locally supplied Ninja Slash Unity Web Player assets for the HTML port."""
from __future__ import annotations

import argparse
import json
import re
import shutil
from pathlib import Path

import UnityPy


SAFE = re.compile(r"[^A-Za-z0-9_.-]+")


def safe_name(name: str, fallback: str) -> str:
    clean = SAFE.sub("_", (name or "").strip()).strip("._")
    return clean or fallback


def unique_path(directory: Path, stem: str, suffix: str) -> Path:
    candidate = directory / f"{stem}{suffix}"
    i = 2
    while candidate.exists():
        candidate = directory / f"{stem}_{i}{suffix}"
        i += 1
    return candidate


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("--source", type=Path, default=Path("originals/ninjaslash_webplayer.unity3d"))
    ap.add_argument("--out", type=Path, default=Path("build_webgl/NinjaSlash/original_assets"))
    ap.add_argument("--clean", action="store_true")
    args = ap.parse_args()

    if not args.source.is_file():
        raise SystemExit(f"Missing source artifact: {args.source}")

    if args.clean and args.out.exists():
        shutil.rmtree(args.out)

    texture_dir = args.out / "textures"
    audio_dir = args.out / "audio"
    meta_dir = args.out / "metadata"
    for directory in (texture_dir, audio_dir, meta_dir):
        directory.mkdir(parents=True, exist_ok=True)

    env = UnityPy.load(str(args.source))
    manifest: dict[str, object] = {
        "source": str(args.source),
        "textures": [],
        "audio": [],
        "scripts": [],
        "object_counts": {},
        "errors": [],
    }

    counts: dict[str, int] = {}
    for obj in env.objects:
        type_name = obj.type.name
        counts[type_name] = counts.get(type_name, 0) + 1
        try:
            data = obj.read()
        except Exception as exc:  # noqa: BLE001 - extraction should continue per object
            manifest["errors"].append({"type": type_name, "path_id": obj.path_id, "error": str(exc)})
            continue

        name = getattr(data, "name", "") or getattr(data, "m_Name", "") or ""

        if type_name == "Texture2D":
            stem = safe_name(name, f"texture_{obj.path_id}")
            out = unique_path(texture_dir, stem, ".png")
            try:
                image = data.image
                image.save(out)
                manifest["textures"].append({
                    "name": name,
                    "file": str(out.relative_to(args.out).as_posix()),
                    "width": getattr(data, "m_Width", None),
                    "height": getattr(data, "m_Height", None),
                    "path_id": obj.path_id,
                })
            except Exception as exc:  # noqa: BLE001
                manifest["errors"].append({"type": type_name, "name": name, "path_id": obj.path_id, "error": str(exc)})

        elif type_name == "AudioClip":
            stem = safe_name(name, f"audio_{obj.path_id}")
            try:
                samples = data.samples
                if isinstance(samples, dict):
                    for sample_name, sample_bytes in samples.items():
                        sample_stem = safe_name(sample_name or stem, stem)
                        suffix = Path(sample_stem).suffix or ".audio"
                        out = unique_path(audio_dir, Path(sample_stem).stem, suffix)
                        out.write_bytes(sample_bytes)
                        manifest["audio"].append({"name": name, "file": str(out.relative_to(args.out).as_posix()), "path_id": obj.path_id})
                elif samples:
                    out = unique_path(audio_dir, stem, ".audio")
                    out.write_bytes(samples)
                    manifest["audio"].append({"name": name, "file": str(out.relative_to(args.out).as_posix()), "path_id": obj.path_id})
            except Exception as exc:  # noqa: BLE001
                manifest["errors"].append({"type": type_name, "name": name, "path_id": obj.path_id, "error": str(exc)})

        elif type_name == "MonoScript":
            manifest["scripts"].append({"name": name, "path_id": obj.path_id})

    manifest["object_counts"] = dict(sorted(counts.items()))
    manifest_path = args.out / "asset_manifest.json"
    manifest_path.write_text(json.dumps(manifest, indent=2, ensure_ascii=False), encoding="utf-8")

    print(json.dumps({
        "source": str(args.source),
        "out": str(args.out),
        "textures": len(manifest["textures"]),
        "audio": len(manifest["audio"]),
        "scripts": len(manifest["scripts"]),
        "errors": len(manifest["errors"]),
        "manifest": str(manifest_path),
    }, indent=2))
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
