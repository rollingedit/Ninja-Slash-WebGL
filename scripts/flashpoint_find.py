#!/usr/bin/env python3
"""
Locate likely Ninja Slash artifacts inside a Flashpoint installation or export.
It recursively scans filenames and SQLite metadata without assuming a specific
Flashpoint version/schema.
"""
from __future__ import annotations

import argparse
import os
import sqlite3
import sys
from pathlib import Path

CANDIDATE_EXTS = {".unity3d", ".bundle", ".assets", ".asset", ".zip", ".7z"}
TEXT_HINTS = ["ninja", "slash", "doopop", "kongregate"]


def score_path(p: Path) -> int:
    s = str(p).lower()
    score = sum(3 for h in TEXT_HINTS if h in s)
    if p.suffix.lower() in CANDIDATE_EXTS:
        score += 4
    if "hacked" in s or "cheat" in s:
        score -= 2
    return score


def scan_files(root: Path, max_results: int) -> list[tuple[int, Path]]:
    results: list[tuple[int, Path]] = []
    for dirpath, dirnames, filenames in os.walk(root):
        # Avoid huge logs/cache directories that do not contain launch payloads.
        lowered = dirpath.lower()
        if any(skip in lowered for skip in ["node_modules", ".git", "logs"]):
            continue
        for fn in filenames:
            p = Path(dirpath) / fn
            sc = score_path(p)
            if sc > 0:
                results.append((sc, p))
    results.sort(key=lambda x: (-x[0], str(x[1]).lower()))
    return results[:max_results]


def scan_sqlite(db: Path, terms: list[str], max_rows: int = 40) -> list[str]:
    out: list[str] = []
    try:
        con = sqlite3.connect(str(db))
        con.row_factory = sqlite3.Row
    except Exception:
        return out
    try:
        tables = [r[0] for r in con.execute("SELECT name FROM sqlite_master WHERE type='table'")]
        for table in tables:
            try:
                cols = [r[1] for r in con.execute(f"PRAGMA table_info({table})")]
            except Exception:
                continue
            text_cols = [c for c in cols if c.lower() in {"title", "developer", "publisher", "source", "platform", "applicationpath", "launchcommand", "tags", "id", "alternateTitles".lower()} or True]
            if not text_cols:
                continue
            # Pull a bounded sample and match in Python to survive unknown schemas.
            try:
                rows = con.execute(f"SELECT * FROM {table} LIMIT 200000")
            except Exception:
                continue
            for row in rows:
                text = " | ".join(str(row[c]) for c in row.keys() if row[c] is not None)
                low = text.lower()
                if all(t.lower() in low for t in terms):
                    out.append(f"{db}: table={table}: {text[:1000]}")
                    if len(out) >= max_rows:
                        return out
    finally:
        con.close()
    return out


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument("root", type=Path, help="Flashpoint install/export root")
    ap.add_argument("--terms", nargs="*", default=["ninja", "slash"], help="Metadata terms; default: ninja slash")
    ap.add_argument("--max", type=int, default=80)
    args = ap.parse_args()

    root = args.root
    if not root.exists():
        print(f"ERROR: root not found: {root}", file=sys.stderr)
        return 2

    print("== Likely files ==")
    for sc, p in scan_files(root, args.max):
        try:
            size = p.stat().st_size
        except OSError:
            size = -1
        print(f"score={sc:02d} size={size:>10} {p}")

    print("\n== SQLite metadata hits ==")
    dbs = list(root.rglob("*.sqlite")) + list(root.rglob("*.db"))
    any_hit = False
    for db in dbs:
        hits = scan_sqlite(db, args.terms)
        for h in hits:
            print(h)
            any_hit = True
    if not any_hit:
        print("No SQLite hits found with supplied terms. Try --terms Ninja Slash Doopop or scan the launcher UI export.")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
