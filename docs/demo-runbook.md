# NetCoreConf Barcelona 2026 — MerlinOps rehearsal runbook (45 minutes)

> **Título:** «El aprendiz de agente: magia, telemetría y gobernanza bajo control».
> This script follows the [10 final user choices](ten-decisions-implementation.md) and **Érika's acceptance criteria**. Copilot Studio + Microsoft Agent Framework (MAF) + Foundry remains the **target live circuit**; only the local MAF phase is currently connected.

## Conference organiser requirements — received 28 November 2026

**Mandatory:** Use the true **initial cover** and **sponsor** slide from the latest official NetCoreConf Barcelona 2026 PPTX. The new Slidev cover and sponsor stages will meet the visual requirement **once the exact official binary artwork is installed**. Until then, a standard Slidev-only export is **NOT** compliant. Follow [the speaker briefing and local PDF packaging procedure](speaker-briefing-barcelona-2026.md) before the event; the packager preserves the sponsors' original design and places them immediately after the official cover. The public GitHub Pages presentation is an interactive technical preview, not the approved speaker PDF.

**By 6 December:** presenter Eventbrite registration in the `Speakers` category via the private link and dinner RSVP via private Microsoft Forms (do not put either link in Git). **11 December:** dinner, restaurant pending. **12 December:** conference at Centre Urquinaona, 10 Plaça Urquinaona, Barcelona; Eventbrite lists 09:00–18:00 generally, personal session time and room are not yet confirmed.

During rehearsal: with the official binary artwork installed, run the exact branded cover → unchanged sponsor slide → Slidev LIVE/REPLAY in one flow. If the official artwork is unavailable, the compliant PDF packaging remains the fallback. Preserve organiser attribution and obtain any necessary permission before republishing proprietary artwork.

## Operator setup — no cloud changes

Prerequisites: .NET 10 SDK, Node 22+, npm. No Azure subscription, Container App, Foundry credentials, Copilot Studio environment or Docker required to rehearse this first stage.

```bash
# Terminal A (repo root)
dotnet run --project src/MerlinOps.Api

# Terminal B (repo root)
cd slides
npm install
npm run dev

# Terminal C
curl http://localhost:5080/health
curl http://localhost:5080/api/integrations/preflight
curl -X POST http://localhost:5080/api/incident/compare \
  -H 'Content-Type: application/json' \
  -d '{"caseId":"incident-improvement"}'
```

At `http://localhost:3030/8`, slide #8 is **«Merlín: ¿de verdad ha aprendido MerlinOps?»**. Select and execute the cases below. On public GitHub Pages the button is disabled and the view is explanatory; the older Mission Control on slide #7 includes separate synthetic Replay.

Do not expose the localhost developer API via a public tunnel without an independent auth implementation and security review.

## One narrative, three validated cases

| Stage | Presentation role | What is measured or demonstrated | Exact local scenario |
|---|---|---|---|
| **Learn** | Érika, la aprendiz, propone mejorar el diagnóstico de un incidente intermitente `order-processing`. David, Merlín, exige pruebas comparables, políticas y calidad. | v1 = 9 real local adapter invocations, v2 = 4 against the identical **synthetic** incident evidence; both execute four MAF deterministic Guardians; actual local Stopwatch duration; evidence hash, audit trace. | `incident-improvement` |
| **Challenge** | Érika, la aprendiz, propone subir a 8 los reintentos automatizados. David, Merlín, revela el DENY independiente del backend y ayuda a reformularla. | Deterministic policy maximum is 3 retries: **DENY**. There must be **no approval or deployment** events. | `incident-policy-block` |
| **Restore** | Érika activa un escenario de regresión; David solicita evidencias de la recuperación de v1. | Regression quality 0.73 is synthetic; `incident-active.json` is **actually restored** from v2 to v1 in a sandbox and SHA-256 integrity is verified. Not Azure rollback. | `incident-regression` |
| **Negative proof** | Érika prueba qué ocurre sin una traza correlacionada; David muestra el bloqueo fail-closed. | **DENY** on `MISSING_REQUIRED_TRACE` — never default a missing metric to 0 or approve without evidence. | `incident-missing-evidence` |

Technical `PASS` never authorizes a change. The current report says `humanApproval=NOT_REQUESTED` and `foundry.status=NOT_CONNECTED`. No model tokens or costs are invented, and local process time is **not** genuine Azure/Foundry latency.

## Proposed 45-minute stage pacing

| Time | Érika | David | Slide / evidence |
|---|---|---|---|
| 0–5 | Se presenta como aprendiz y plantea el reto del agente. | Entra como Merlín: «¿magia sin pruebas?». | Cover and opening roles |
| 5–12 | Explica la misión que desea lanzar desde Copilot Studio. | Explica MAF, Foundry, Guardianes y políticas innegociables; cloud PENDING hasta verificación. | Arquitectura y roles |
| 12–24 | Propone v2 y opera la comparación sobre el mismo incidente. | Contrasta calidad, trazas, 9→4 llamadas locales y tiempo observado; nunca inventa tokens de Foundry. | Slide #8 · incident improvement |
| 24–32 | Plantea una mejora insegura y comprueba su rechazo. | Explica el DENY determinista independiente del LLM y la secuencia de auditoría. | Slide #8 · policy block |
| 32–38 | Activa la regresión y pregunta cómo recuperar v1. | Demuestra restauración local, SHA-256 y resultado; no afirma rollback de Azure. | Slide #8 · local regression |
| 38–45 | Cierra con lo aprendido y pide decisión humana. | Recuerda que PASS técnico no equivale a aprobación; demuestra Replay y cierra con la plantilla oficial. | Mission Control / conclusión |

**Presenter controls remain a work item**: slide #8 is a local, limited test runner, not a secure authenticated presenter panel. Projected public slides never approve or modify a resource.

## Definition of a genuinely LIVE rehearsal (not yet completed)

- [ ] Copilot Studio in an existing dev environment actually delegates a task over **authenticated A2A**, or an approved, protected REST integration if A2A fails compatibility checks.
- [ ] MAF .NET receives a distinct mission, correlates its trace and runs all four Guardians.
- [ ] **Existing Foundry** project `ai-omniroute-dev` answers actual model calls with a verified primary/fallback routing policy; tokens, latency, usage and cost carry real provenance. Confirm model compatibility and API capabilities.
- [ ] MAF and Foundry emit true incremental correlated events to **read-only** Slidev projection, not delayed playback of completed runs.
- [ ] Érika can submit proposals for backend-enforced Quality Gates, but cannot self-approve; human approval is recorded separately from the technical PASS. An unauthorized presenter/action is blocked.
- [ ] All three cases pass full end-to-end integration and negative tests, with a tested offline Replay route.
- [ ] Operator separation, accessible contrast, projector legibility, 3D static fallback, network-drop/reconnect, and 45-minute rehearsal pass.
- [ ] Authentic LIVE enabled only when those proofs exist; otherwise label **SIMULATED** / **REPLAY**.
- [ ] No cloud changes unless explicitly approved.

## Stage safety

**GitHub code changes are authorized, but Copilot Studio and Foundry configuration changes require prior owner review.** The current draft PR is not a deployment. The only real rollback is the local sandbox workflow file. The public site may show synthetic fixtures but never private Foundry tokens, Entra credentials or customer telemetry.
