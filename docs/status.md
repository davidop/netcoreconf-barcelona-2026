# Implementation status — evidence-oriented

[← Back to README](../README.md) · [Roadmap](roadmap.md) · [CI](https://github.com/davidop/netcoreconf-barcelona-2026/actions/workflows/ci.yml)

## Implemented and CI-validated

- .NET 10 API, three locally simulated scenarios, YAML fail-closed policy and SQLite synthetic run snapshots.
- Microsoft Agent Framework **real graph execution** with four **deterministic local Guardian executors**. MAF is genuinely executing workflow edges but **no generative AI models are connected**.
- Synthetic SSE, replay files and visible provenance.
- Slidev presentation, procedural 3D MerlinOps with reduced-motion fallback, Mission Control UI and static GitHub Pages.
- xUnit **10 tests**, Replay fixtures **4 tests**, privacy sanitizer **5 tests**, browser Playwright tests **2**, build of Slidev/Aspire, Terraform format and validate.
- Privacy tool produces REVIEW_REQUIRED candidates; it never publishes authentic private traces automatically.
- Visual assets and docs repository makeover (SVG banners, mascot, technology brand attribution and architecture diagram sources); Markdown/SVG integrity check in CI.

**Prior passing evidence:**
- [Four CI jobs passed](https://github.com/davidop/netcoreconf-barcelona-2026/actions/runs/44840427893)
- [Playwright browser E2E passed](https://github.com/davidop/netcoreconf-barcelona-2026/actions/runs/44840482115)
- [GitHub Pages published successfully](https://github.com/davidop/netcoreconf-barcelona-2026/actions/runs/44845428686)

New changes may require a fresh CI run. A passing badge does not imply real Azure integration.

## Approved next step: Slidev Live Orchestration

- [Architectural requirement and acceptance criteria](live-orchestration.md) and [issue #22](https://github.com/davidop/netcoreconf-barcelona-2026/issues/22) are recorded. **This is planned, not delivered.**
- Existing Slidev can start SIMULATED MAF execution and control synthetic Replay. The API currently saves a finished run before emitting delayed SSE events; that is **post-run playback, not real-time orchestration**.
- Private presenter controls, read-only synchronized projection, v1/v2 live execution, authenticated approval, resuming live events and verifiable local rollback remain to be built and tested.

## Current limitations and not implemented

- Real Microsoft Foundry token metering, quality evaluations, model routing and observed savings.
- Real Copilot Studio GitHub Copilot Harness integration via Microsoft 365 Agents SDK; compatibility unverified.
- Entra ID/OIDC/PKCE, tenant authorization, backend RBAC, verified Teams/Power Automate approval.
- Azure cloud resources, deployment, auto-rollback or Azure Table/Blob persistent storage.
- Production OpenTelemetry exporter and immutable compliant event log.
- An authentic sanitized Replay captured from an authorized production environment.
- Two full 45-minute dress rehearsals with Érika and David.
- The README Mission Control SVG is an **illustration**, not an authentic screenshot.

## Safety commitment

**No Azure resources have been created or modified through this implementation workflow.** Azure creation/modification and LIVE are prohibited until separately and explicitly authorized. Local fictional approvals do not grant rights to mutate infrastructure.
