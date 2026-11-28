"""Offline regression tests for speaker deck composition (no network, no real template)."""
from __future__ import annotations

import base64
from io import BytesIO
from pathlib import Path
import tempfile
import unittest

from pptx import Presentation
from pptx.util import Inches
from pypdf import PdfReader, PdfWriter

from tools.assemble_speaker_deck import (
    DEFAULT_TITLE, compose_pdf, fill_official_cover, verify_official_template,
)

# Tiny valid PNG used only for synthetic test fixtures, not a sponsor logo.
PIXEL = base64.b64decode(
    "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVQIHWP4"
    "//8/AwAI/AL+XqKHmQAAAABJRU5ErkJggg=="
)


def make_template(path: Path, *, valid: bool = True) -> None:
    ppt = Presentation()
    ppt.slide_width, ppt.slide_height = Inches(20), Inches(11.25)
    for i in range(6):
        slide = ppt.slides.add_slide(ppt.slide_layouts[6])
        box = slide.shapes.add_textbox(Inches(1), Inches(1), Inches(14), Inches(2))
        if i == 0:
            box.text = "<NetCoreConf/> Barcelona '26"
        elif i == 1:
            box.text = "Sponsors Platinum sponsors Gold sponsors Colabora" if valid else "Generic title"
            slide.shapes.add_picture(BytesIO(PIXEL), Inches(2), Inches(3), width=Inches(1))
        elif i == 5:
            box.text = "GRACIAS"
    ppt.save(str(path))


def make_pdf(path: Path, sizes: list[tuple[int, int]]) -> None:
    writer = PdfWriter()
    for width, height in sizes:
        writer.add_blank_page(width=width, height=height)
    with path.open("wb") as stream:
        writer.write(stream)


class SpeakerDeckTests(unittest.TestCase):
    def setUp(self):
        self.temp = tempfile.TemporaryDirectory()
        self.addCleanup(self.temp.cleanup)
        self.root = Path(self.temp.name)
        self.template = self.root / "official.pptx"
        make_template(self.template)

    def test_validate_official_cover_sponsors_and_original_artwork(self):
        self.assertEqual(verify_official_template(self.template), 6)

    def test_reject_generic_deck_as_official(self):
        make_template(self.template, valid=False)
        with self.assertRaisesRegex(ValueError, "patrocinadores"):
            verify_official_template(self.template)

    def test_cover_overlay_keeps_original_sponsor_slide(self):
        output = self.root / "speaker-cover.pptx"
        fill_official_cover(self.template, output, DEFAULT_TITLE, "Érika Cepeda  ·  David Oliva Paredes")
        original = Presentation(str(self.template))
        result = Presentation(str(output))
        cover = " ".join(x.text for x in result.slides[0].shapes if x.has_text_frame)
        self.assertIn(DEFAULT_TITLE, cover)
        self.assertIn("Érika", cover)
        self.assertEqual(len(result.slides[1].shapes), len(original.slides[1].shapes))

    def test_pdf_packaging_preserves_original_front_and_sponsor(self):
        official, slidev, final = (self.root / p for p in ("official.pdf", "slidev.pdf", "result.pdf"))
        make_pdf(official, [(1440, 810)] * 6)
        make_pdf(slidev, [(960, 540)] * 3)
        count = compose_pdf(official, slidev, final)
        self.assertEqual(count, 5)
        pages = PdfReader(str(final)).pages
        self.assertEqual([int(x.mediabox.width) for x in pages], [1440, 1440, 960, 960, 1440])

    def test_optional_slidev_cover_keeps_both_covers(self):
        official, slidev, final = (self.root / p for p in ("official.pdf", "slidev.pdf", "result.pdf"))
        make_pdf(official, [(1440, 810)] * 6)
        make_pdf(slidev, [(960, 540)] * 2)
        count = compose_pdf(official, slidev, final, keep_slidev_cover=True, include_thanks=False)
        self.assertEqual(count, 4)

    def test_reject_incomplete_slidev_pdf(self):
        official, slidev, final = (self.root / p for p in ("official.pdf", "slidev.pdf", "result.pdf"))
        make_pdf(official, [(1440, 810)] * 6)
        make_pdf(slidev, [(960, 540)])
        with self.assertRaisesRegex(ValueError, "dos diapositivas"):
            compose_pdf(official, slidev, final)


if __name__ == "__main__":
    unittest.main()
