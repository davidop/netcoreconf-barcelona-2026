#!/usr/bin/env python3
"""Build a conference PDF using the ACTUAL organiser template and Slidev PDF.

No template artwork/logos are redistributed in the public Git repository.
Requires a locally downloaded copy of the exact official PPTX linked in the
speaker email, python-pptx, pypdf, and LibreOffice (soffice).
"""
from __future__ import annotations

import argparse
from pathlib import Path
import shutil
import subprocess
import tempfile

from pptx import Presentation
from pptx.dml.color import RGBColor
from pptx.enum.text import MSO_ANCHOR, PP_ALIGN
from pptx.util import Inches, Pt
from pypdf import PdfReader, PdfWriter


DEFAULT_TITLE = "El aprendiz de agente: magia, telemetría y gobernanza bajo control"
DEFAULT_SPEAKERS = "Érika Cepeda  ·  David Oliva Paredes"


def verify_official_template(template_path: Path) -> int:
    """Fail closed: do not mistake a generic deck for the required template."""
    if not template_path.is_file():
        raise ValueError(f"No existe la plantilla oficial: {template_path}")
    deck = Presentation(str(template_path))
    if len(deck.slides) < 6:
        raise ValueError("La plantilla oficial debe contener al menos seis diapositivas.")
    def visible_text(slide):
        return " ".join(
            shape.text.strip() for shape in slide.shapes
            if shape.has_text_frame and shape.text.strip()
        ).casefold()

    cover = visible_text(deck.slides[0])
    sponsors = visible_text(deck.slides[1])
    if "netcoreconf" not in cover or "barcelona" not in cover:
        raise ValueError("Diapositiva 1 no corresponde a la portada oficial Barcelona 2026.")
    if not all(word in sponsors for word in ("sponsors", "platinum", "gold", "colabora")):
        raise ValueError("Diapositiva 2 no es la lámina oficial de patrocinadores.")
    if not any(shape.shape_type == 13 for shape in deck.slides[1].shapes):
        raise ValueError("Faltan elementos gráficos/logotipos oficiales en la lámina 2.")
    return len(deck.slides)


def fill_official_cover(template_path: Path, output_path: Path, title: str, speakers: str) -> None:
    """Add session title to the intentionally blank RIGHT region of slide #1."""
    deck = Presentation(str(template_path))
    slide = deck.slides[0]
    width = deck.slide_width / Inches(1)
    height = deck.slide_height / Inches(1)
    if abs(width / height - 16 / 9) > .005:
        raise ValueError("La plantilla no tiene proporción 16:9.")

    box = slide.shapes.add_textbox(Inches(width * .53), Inches(height * .32),
                                   Inches(width * .44), Inches(height * .28))
    tf = box.text_frame
    tf.word_wrap = True
    tf.vertical_anchor = MSO_ANCHOR.MIDDLE
    tf.clear()
    p = tf.paragraphs[0]
    p.alignment = PP_ALIGN.LEFT
    r = p.add_run()
    r.text = title
    r.font.name = "Aptos Display"
    r.font.size = Pt(37)
    r.font.bold = True
    r.font.color.rgb = RGBColor(255, 255, 255)

    box2 = slide.shapes.add_textbox(Inches(width * .53), Inches(height * .67),
                                    Inches(width * .44), Inches(height * .12))
    t2 = box2.text_frame
    t2.clear()
    p2 = t2.paragraphs[0]
    p2.alignment = PP_ALIGN.LEFT
    r2 = p2.add_run()
    r2.text = speakers
    r2.font.name = "Aptos"
    r2.font.size = Pt(23)
    r2.font.color.rgb = RGBColor(237, 224, 252)
    deck.save(str(output_path))


def compose_pdf(organiser_pdf: Path, slidev_pdf: Path, output_pdf: Path,
                *, keep_slidev_cover: bool = False, include_thanks: bool = True) -> int:
    """Keep sponsor source page untouched; do not make approximate logos."""
    official, content = PdfReader(str(organiser_pdf)), PdfReader(str(slidev_pdf))
    if len(official.pages) < 6:
        raise ValueError("La conversión de la plantilla oficial no contiene seis páginas.")
    if len(content.pages) < 2:
        raise ValueError("El PDF de Slidev debe tener al menos dos diapositivas.")
    writer = PdfWriter()
    writer.add_page(official.pages[0])  # actual organiser cover with session title
    writer.add_page(official.pages[1])  # exact original sponsors page
    start = 0 if keep_slidev_cover else 1
    for page in content.pages[start:]:
        writer.add_page(page)
    if include_thanks:
        writer.add_page(official.pages[5])  # organiser's official "GRACIAS"
    writer.add_metadata({
        "/Title": DEFAULT_TITLE,
        "/Subject": "NetCoreConf Barcelona 2026 — speaker-compliant deck",
        "/Creator": "MerlinOps local conference packaging script",
    })
    output_pdf.parent.mkdir(parents=True, exist_ok=True)
    with output_pdf.open("wb") as stream:
        writer.write(stream)
    return len(writer.pages)


def build(template_path: Path, slidev_pdf: Path, output_pdf: Path,
          title: str, speakers: str,
          *, keep_slidev_cover: bool = False, include_thanks: bool = True) -> int:
    verify_official_template(template_path)
    if not slidev_pdf.is_file():
        raise ValueError(f"El PDF exportado de Slidev no existe: {slidev_pdf}")
    office = shutil.which("soffice") or shutil.which("libreoffice")
    if not office:
        raise RuntimeError("Es necesario instalar LibreOffice para convertir la plantilla.")
    with tempfile.TemporaryDirectory(prefix="merlinops-speaker-") as dirname:
        folder = Path(dirname)
        filled = folder / "official-speaker-cover.pptx"
        fill_official_cover(template_path, filled, title, speakers)
        command = [
            office, f"-env:UserInstallation={folder.as_uri()}/office-profile",
            "--headless", "--convert-to", "pdf",
            "--outdir", str(folder), str(filled),
        ]
        run = subprocess.run(command, capture_output=True, text=True, timeout=90, check=False)
        official_pdf = folder / "official-speaker-cover.pdf"
        if run.returncode != 0 or not official_pdf.is_file():
            raise RuntimeError(f"No se pudo convertir la plantilla: {run.stderr[-500:]}")
        return compose_pdf(official_pdf, slidev_pdf, output_pdf,
                           keep_slidev_cover=keep_slidev_cover,
                           include_thanks=include_thanks)


def main() -> None:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--template", required=True, type=Path,
                        help="PowerPoint oficial de la organización, NO se guarda en Git")
    parser.add_argument("--slidev-pdf", required=True, type=Path,
                        help="PDF exportado de Slidev en local")
    parser.add_argument("--output", type=Path, default=Path("artifacts/private/netcoreconf-barcelona-2026-speaker.pdf"))
    parser.add_argument("--title", default=DEFAULT_TITLE)
    parser.add_argument("--speakers", default=DEFAULT_SPEAKERS)
    parser.add_argument("--keep-slidev-cover", action="store_true",
                        help="Mantener también la portada de Slidev (duplica portada)")
    parser.add_argument("--no-official-thanks", action="store_true")
    args = parser.parse_args()
    pages = build(args.template, args.slidev_pdf, args.output, args.title, args.speakers,
                  keep_slidev_cover=args.keep_slidev_cover,
                  include_thanks=not args.no_official_thanks)
    print(f"Paquete oficial creado: {args.output} ({pages} diapositivas)")
    print("Portada y sponsors originales preservados; ningún asset oficial publicado en Git.")


if __name__ == "__main__":
    main()
