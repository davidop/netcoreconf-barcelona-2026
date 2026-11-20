# Implementation status (do not overclaim)

The code and build artefacts determine status. A feature is not verified until the corresponding CI run or live integration has been checked.

**Implemented / awaiting CI:** .NET 10 synthetic scenario API; fail-closed YAML guardrails; SQLite run snapshots; simulated SSE; three synthetic Replay datasets; Slidev presentation and Mission Control; procedural 3D MerlinOps with reduced-motion fallback; deterministic tests.

**Scaffold / pending:** Agent Framework preview SDK smoke workflow, Aspire AppHost, Terraform opt-in configuration, GitHub Pages manual publish, Playwright tests, CI workflows.

**Not implemented or verified:** real Foundry telemetry and model routing, M365 Agents SDK to GitHub Copilot Harness, actual multiagent AI planning, Entra authentication, Teams HITL authorization, Azure backend policy, Azure state store, production OTel, measured cost savings, real deployment and rollback, authentic captured telemetry, conference dress rehearsal.

**Never performed:** creating or modifying resources in Azure. Explicit new user authorization is needed for any cloud write.

**SDK integration detail:** `src/MerlinOps.Api/MafSimulationEngine.cs` uses Microsoft Agent Framework's real workflow builder and parallel fan-out to four deterministic local functions; its `SIMULATED` event stream is exposed via `/api/runs/maf-simulate`. This is not live generative-agent orchestration and must never be described as such. **Replay privacy:** `tools/sanitize_replay.py` produces local candidates stamped `REVIEW_REQUIRED`; no automatic public publishing occurs.
