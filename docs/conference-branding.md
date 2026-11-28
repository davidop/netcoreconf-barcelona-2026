# NetCoreConf Barcelona 2026 · editable Slidev conference branding

## Version status

- Branch: `feat/erika-evidence-first-demo` / PR #23; **not published to `main`** until reviewed and merged.
- Slide 1: editable names, roles, portrait images, dual Microsoft MVP badges and QR; official cover artwork is an independent background.
- Slide 2: the **actual, unmodified** official sponsor slide, not a reconstruction of sponsors.
- Final slide: official `GRACIAS` artwork with the repository link.
- The organiser PowerPoint **must never be added** to the public repository. Private speaker registration links also stay out.
- Source: `netcoreconfBarcelona2026.pptx` supplied by organiser (SHA-256 `98e835314add9a0fb95aa1525220b021618f281c86b849e130346ad2e31c6c30`).
- Portraits: matching public speaker images visible in the official conference poster; original LinkedIn profile image downloads are not guaranteed because of access restrictions. Replace with speaker-approved full-resolution portraits as needed.
- Text / narrative: **Érika = La aprendiz**, `Microsoft MVP · M365 Copilot Specialist`; **David = Merlín**, `Microsoft MVP · Microsoft Specialist`. No employers' logos.

## Required binary assets (intentionally separate from the organiser PPTX)

Install the prepared asset bundle into `slides/public/conference/`:

| File | Purpose |
| --- | --- |
| `official-cover.webp` | Official blank cover background, 16:9 |
| `official-sponsors.webp` | Unmodified original sponsor slide, 16:9 |
| `official-thanks.webp` | Official organiser closing slide |
| `erika.webp` / `david.webp` | Editable separate speaker portrait sources |
| `mvp.webp` | Microsoft MVP mark shown under **both** speakers |

Slidev will show a branded text fallback when an expected asset is absent. This is intentional; **do not claim that official artwork is published until these files have been committed and the browser E2E check has confirmed their availability**.

## Local review

```powershell
git fetch origin
git switch feat/erika-evidence-first-demo
# Unpack the prepared assets into slides/public/conference/ without changing the organisers' source PPTX.
cd slides
npm install
npm run dev
# http://localhost:3030/
```

Check portrait identity/crop, both MVP marks, QR scans to `https://github.com/davidop/netcoreconf-barcelona-2026`, official sponsor artwork at slide 2, and the transitions into Mission Control. The inserted sponsor slide shifts Mission Control to slide **7** and IncidentImprovement to slide **8**. The scenario data remains synthetic and the real cloud circuit remains pending independent approval.
