#!/usr/bin/env python3
"""Render the owned MerlinOps SVG to a crisp, verified 1280x640 GitHub OG image.

This is a build artifact only. It does NOT update GitHub repository settings.
"""
from __future__ import annotations

from pathlib import Path
from io import BytesIO

import cairosvg
from PIL import Image

ROOT = Path(__file__).resolve().parents[1]
SOURCE = ROOT / "assets/banners/social-preview.svg"
DESTINATION = ROOT / "assets/banners/social-preview.png"

def render() -> Path:
    source = SOURCE.read_text(encoding="utf-8")
    if "<script" in source.lower() or "<foreignobject" in source.lower():
        raise ValueError("Active SVG content cannot be rendered for social preview.")
    pixels = cairosvg.svg2png(bytestring=source.encode("utf-8"), output_width=1280, output_height=640)
    image = Image.open(BytesIO(pixels)).convert("RGB")
    if image.size != (1280, 640):
        raise AssertionError("Expected social card size 1280x640")
    DESTINATION.parent.mkdir(parents=True, exist_ok=True)
    image.save(DESTINATION, "PNG", optimize=True)
    again = Image.open(DESTINATION)
    if again.size != (1280, 640) or again.format != "PNG":
        raise AssertionError("Saved social preview is invalid")
    print(f"Rendered {DESTINATION.relative_to(ROOT)} ({DESTINATION.stat().st_size} bytes)")
    return DESTINATION

if __name__ == "__main__":
    render()
