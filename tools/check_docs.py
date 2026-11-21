#!/usr/bin/env python3
"""Offline repository link and vector asset integrity checker (no network)."""
from __future__ import annotations

import re
import sys
import xml.etree.ElementTree as ET
from pathlib import Path
from urllib.parse import unquote, urlparse

ROOT = Path(__file__).resolve().parents[1]
MARKDOWN_LINKS = re.compile(r"!?\[[^\]]*]\(([^)]+)\)")
ATTR_LINKS = re.compile(r"""(?:src|href)=["']([^"']+)["']""")
IGNORED_SCHEMES = ("http:", "https:", "mailto:", "data:", "javascript:")
errors = []


def verify(source: Path, target: str) -> None:
    target = target.strip().strip("<>").split(" ", 1)[0]
    if not target or target.startswith("#") or target.startswith(IGNORED_SCHEMES):
        return
    if target.startswith("sandbox:"):
        errors.append(f"{source.relative_to(ROOT)}: sandbox paths must not be committed")
        return
    clean = unquote(urlparse(target).path)
    if not clean:
        return
    path = (source.parent / clean).resolve()
    if not path.is_relative_to(ROOT):
        errors.append(f"{source.relative_to(ROOT)}: link escapes repository: {target}")
    elif not path.exists():
        errors.append(f"{source.relative_to(ROOT)}: missing local link: {target}")


for doc in ROOT.rglob("*.md"):
    if any(part in {"node_modules", "dist", ".git"} for part in doc.parts):
        continue
    text = doc.read_text(encoding="utf-8")
    for link in MARKDOWN_LINKS.findall(text):
        verify(doc, link)
    for link in ATTR_LINKS.findall(text):
        verify(doc, link)

for icon in ROOT.rglob("*.svg"):
    if any(part in {"node_modules", "dist"} for part in icon.parts):
        continue
    try:
        root = ET.parse(icon).getroot()
        if root.tag != "{http://www.w3.org/2000/svg}svg":
            errors.append(f"{icon.relative_to(ROOT)}: SVG root mismatch")
        for node in root.iter():
            if node.tag.rsplit("}", 1)[-1].lower() in {"script", "foreignobject"}:
                errors.append(f"{icon.relative_to(ROOT)}: active SVG content not permitted")
    except ET.ParseError as err:
        errors.append(f"{icon.relative_to(ROOT)}: invalid SVG: {err}")

if errors:
    for error in errors:
        print("ERROR", error, file=sys.stderr)
    raise SystemExit(1)
print("DOCS_OK: local Markdown links and SVG documents are well formed")
