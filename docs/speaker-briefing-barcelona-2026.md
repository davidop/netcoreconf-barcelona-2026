# NetCoreConf Barcelona 2026 — speaker briefing and presentation compliance

**Source:** organiser email **"NetCoreConf Barcelona: información importante para speakers"**, received **2026-11-28 09:24 UTC** (10:24 Europe/Madrid), read from the owner's Outlook inbox. The links for speaker registration/dinner are **private** and intentionally NOT copied into this public Git repository.

**Session:** «El aprendiz de agente: magia, telemetría y gobernanza bajo control 🪄»  
**Presenters:** Érika Cepeda and David Oliva Paredes  
**Event:** Saturday **12 December 2026**, Centre Urquinaona / Centre Politècnic, **Plaça Urquinaona 10, 08010 Barcelona**. Eventbrite currently lists general event time **09:00–18:00**; the individual MerlinOps session slot/room has **NOT been confirmed** by the email. See the [official conference website](https://www.netcoreconf.com/) and [agenda](https://www.netcoreconf.com/barcelona/agenda) for updates.

## Title/version consistency check

The newly provided **28 November 2026 promotional poster** displays **«El aprendiz de agente: magia, telemetría y gobernanza bajo control 🪄»** and credits **Érika Cepeda + David Oliva**. An **older 24 October Sessionize co-speaker invitation** referenced a prior submission title: **«El aprendiz de agente: Copilot Studio, telemetría y mejoras bajo control 🪄»**. Because the invitation predates the current speaker poster, this is a **possible metadata mismatch, not evidence that the final programme is incorrect**. Verify the **current** Sessionize session title before speakers/promotional material are finalised; do not silently overwrite the organisers' submission.

Follow progress and outstanding manual actions in [speaker readiness issue #24](https://github.com/davidop/netcoreconf-barcelona-2026/issues/24). The complete links and invitation contents remain private in the mailbox.

## Mandatory organiser instructions

| Due date | Task | Owner | Status |
|---|---|---|---|
| **06 Dec 2026** | Register through the **private speaker Eventbrite link** from the email; choose **Speakers**. A previously purchased ticket remains valid. | David, Érika independently | NOT CONFIRMED |
| **06 Dec 2026** | RSVP for **speaker dinner** using the **private Microsoft Forms link** from the email. Dinner is **Fri 11 Dec**; venue to be confirmed. | Each presenter | NOT CONFIRMED |
| Before the conference | Confirm commemorative polo size, requested as part of the registration flow. | Each presenter | NOT CONFIRMED |
| Before publication/recording | **Use the official initial cover AND the official sponsors slide** in the actual conference presentation. | Demo team | Offline PDF packaging supported; stage copy pending |
| Before conference | Review the final agenda/slot, room, projector resolution, connectivity, adapters, microphones and rehearsals. | Demo team | PENDING organiser agenda |

**Speakers may bring a companion**; for additional tickets they should contact the organisers rather than publish the private speaker registration code. The organisers offered help finding accommodation for visitors. The email specifically encouraged sharing the event on social networks and downloading a personalised speaker card through Sessionize.

## Official PowerPoint template — inspected, NOT committed

The link in the speaker email points to **`netcoreconfBarcelona2026.pptx`**, an 11 MB PowerPoint (not a native Google Slides deck), last modified 2026-11-28 according to Drive metadata. Its SHA-256 when inspected:

```text
98e835314add9a0fb95aa1525220b021618f281c86b849e130346ad2e31c6c30
```

This revision has **six slides**. The first is a true NetCoreConf Barcelona '26 cover with empty title area on the right, and the second is **the organiser's actual sponsor slide with branded graphics**. The final slide says **GRACIAS**. The sponsor slide contains the official artwork; DO NOT redraw logos or substitute sponsor names taken from an older photograph.

### Build the compliant speaker PDF

Keep a copy of the exact official PPTX **locally**, outside Git. Do not commit the presentation or speaker-only links to this public repository.

```bash
# On developer machine: .NET/Slidev still handles the interactive demo separately.
python -m pip install -r tools/requirements-speaker.txt
cd slides
npm install
npx slidev export --format pdf
cd ..

# Adjust the local --slidev-pdf path to the file actually written by Slidev.
python tools/assemble_speaker_deck.py \
  --template /path/to/netcoreconfBarcelona2026.pptx \
  --slidev-pdf slides/slides-export.pdf \
  --output artifacts/private/netcoreconf-barcelona-2026-speaker.pdf
```

Requires local **LibreOffice** (`soffice`). The script:
1. Validates that slide 1 is the official Barcelona '26 cover and slide 2 is the real Sponsors/Platinum/Gold/Colabora artwork.
2. Populates the cover's intentionally empty right-hand title and presenters area in a temporary **copy** of the PowerPoint, without changing the source.
3. Generates the final PDF **official cover → exact official sponsors → technical Slidev pages (omitting the duplicated Slidev title page) → official GRACIAS**. It preserves logos as original PPTX-rendered content and never modifies them.
4. Writes the result only under the private ignored artefact directory. No Google account, Entra credential or cloud provisioning is required to package it.

**Critical limitation:** this is a PDF packaging workflow for the speaker deliverable, **not an automatic integration of the official sponsor artwork into the public/interactive Slidev web UI**. For the live demo, use the compliant PDF as the opening (cover and sponsors) before switching to local Slidev for MerlinOps. The public website/Slidev alone is not yet a compliant substitute. Do not claim that it is. The official template is deliberately not redistributed without the organiser's consent.

## Speaker vs demo readiness

The primary demo remains **Copilot Studio → MAF → Foundry → evaluations → Slidev** as the LIVE target. Today only the local MAF workflow is verified; Copilot Studio/Foundry inference and private presenter controls remain pending. Follow [the 45-minute technical rehearsal](demo-runbook.md), enforce PASS ≠ human approval, and always distinguish local/SIMULATED from real cloud telemetry.

**Conference tech dry run:** present at 16:9, test a local URL with stage connectivity disabled, rehearse slide #7 incident v1/v2, DENY and verified local rollback, projector legibility, speaker transitions, microphone/HDMI and offline fallback. **No production cloud writes are authorized.**

## External reference sources

- [Official NetCoreConf website](https://www.netcoreconf.com/) — date, location and sponsors
- [Agenda](https://www.netcoreconf.com/barcelona/agenda) — individual session slot still to be checked
- [Eventbrite general event listing](https://www.eventbrite.co.uk/e/entradas-netcoreconf-2026-barcelona-2003871978029) — general 09:00–18:00 timeframe
- Speaker's private November 28 organiser email — authoritative for registration and dinner deadlines; do not copy private URLs into this repository
