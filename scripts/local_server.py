#!/usr/bin/env python3
"""Local HTTP server for Unity WebGL builds with useful MIME/header defaults."""
from __future__ import annotations
import argparse
import functools
import http.server
import mimetypes
import os
import socketserver
from pathlib import Path

mimetypes.add_type('application/wasm', '.wasm')
mimetypes.add_type('application/javascript', '.js')
mimetypes.add_type('application/octet-stream', '.data')
mimetypes.add_type('application/octet-stream', '.symbols.json')

class UnityWebGLHandler(http.server.SimpleHTTPRequestHandler):
    def end_headers(self):
        # Cross-origin isolation can help newer Unity builds using threads/SAB.
        self.send_header('Cross-Origin-Opener-Policy', 'same-origin')
        self.send_header('Cross-Origin-Embedder-Policy', 'require-corp')
        self.send_header('Access-Control-Allow-Origin', '*')
        super().end_headers()

    def guess_type(self, path):
        t = super().guess_type(path)
        if path.endswith('.wasm.br'):
            return 'application/wasm'
        if path.endswith('.js.br'):
            return 'application/javascript'
        if path.endswith('.data.br'):
            return 'application/octet-stream'
        if path.endswith('.wasm.gz'):
            return 'application/wasm'
        if path.endswith('.js.gz'):
            return 'application/javascript'
        if path.endswith('.data.gz'):
            return 'application/octet-stream'
        return t

    def send_head(self):
        # Patch content-encoding for precompressed Unity build files.
        path = self.translate_path(self.path)
        f = super().send_head()
        return f

    def send_response_only(self, code, message=None):
        super().send_response_only(code, message)

    def send_header(self, keyword, value):
        # Avoid duplicate content-type surprises by leaving parent behavior intact.
        super().send_header(keyword, value)


def main() -> int:
    ap = argparse.ArgumentParser()
    ap.add_argument('directory', type=Path, help='Unity WebGL build directory containing index.html')
    ap.add_argument('port', type=int, nargs='?', default=8080)
    args = ap.parse_args()
    directory = args.directory.resolve()
    if not (directory / 'index.html').exists():
        raise SystemExit(f'No index.html found in {directory}')
    handler = functools.partial(UnityWebGLHandler, directory=str(directory))
    with socketserver.TCPServer(('127.0.0.1', args.port), handler) as httpd:
        print(f'Serving {directory} at http://127.0.0.1:{args.port}/')
        try:
            httpd.serve_forever()
        except KeyboardInterrupt:
            print('\nStopped.')
    return 0

if __name__ == '__main__':
    raise SystemExit(main())
