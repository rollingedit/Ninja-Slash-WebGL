#!/usr/bin/env python3
"""Install WebGL/platform stubs into a recovered Unity project."""
from __future__ import annotations
import argparse
import shutil
from pathlib import Path


def copytree_merge(src: Path, dst: Path):
    for p in src.rglob('*'):
        rel = p.relative_to(src)
        out = dst / rel
        if p.is_dir():
            out.mkdir(parents=True, exist_ok=True)
        else:
            out.parent.mkdir(parents=True, exist_ok=True)
            shutil.copy2(p, out)
            print(f'installed {out}')


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument('unity_project', type=Path)
    args = ap.parse_args()
    project = args.unity_project
    assets = project / 'Assets'
    if not assets.exists():
        raise SystemExit(f'{project} does not look like a Unity project: missing Assets/')
    src = Path(__file__).resolve().parents[1] / 'unity_stubs' / 'Assets'
    copytree_merge(src, assets)
    print('Done. Reopen Unity or refresh Assets.')
    return 0

if __name__ == '__main__':
    raise SystemExit(main())
