# Érika-first demo: first working vertical slice

> **Priority:** Érika's acceptance criteria take precedence over adding features. The 78 original decisions and 77 subsequent accepted choices are retained as target requirements; implementation proceeds in evidence-first slices. **No more architecture questionnaire is necessary.**

[← Architecture](architecture.md) · [Slidev live orchestration plan](live-orchestration.md) · [Current status](status.md)

## One real question; same evidence for v1 and v2

**User request:** "Our Azure Container App is costing too much and has irregular latency. Inspect the configuration, requests, scaling, performance, policies and costs; propose a safe improvement, explain it and do not modify Azure."

**Baseline v1:** the agent repeatedly asks local tool adapters for configuration, traffic and latency. **Proposed v2:** a deterministic workflow gathers related evidence once, validates it, then leaves the reasoning phase for interpretation and recommendation. Both variants use exactly the same immutable synthetic snapshot.

This first slice executes **actual local C# tool-call sequences** against **fictional Container Apps evidence**, not live Azure requests and not generative model agents. It produces a deterministic JSON comparison on the backend and can be invoked from a Slidev component when the local API is running.

## Executable scenarios

| Test case | What is actually done | Expected technical outcome |
|---|---|---|
| `finops-success` | v1 invokes nine local adapters, v2 invokes four against the same fixture; evaluate quality and mandatory policy | `PASS`, then `AWAITING_HUMAN_REVIEW`. No approval or publication |
| `policy-block` | Same comparison; unsafe minReplicas=0 proposal is rejected by the YAML-backed minimum-replicas policy enforced in the .NET backend | `DENY` / `BLOCKED`; no approval or deploy events |
| `quality-rollback` | Quality passes initially, then an explicitly synthetic regression is introduced; a sandbox on-disk workflow manifest is switched from v2 back to v1 | `ROLLED_BACK_LOCAL` only after SHA-256 and active-version checks succeed. **Not Azure rollback** |
| `missing-metrics` | Omit latency p95 from the input snapshot | `DENY` / `BLOCKED` with reason `MISSING_REQUIRED_LATENCY_METRIC` |

**Comparable metrics**: the number of local adapter calls (9 vs 4) is counted during execution; the duration is an explicitly **SYNTHETIC_MODEL** number (150 ms per modeled tool invocation plus fixed overhead), not actual wall-clock runtime or real Azure latency. Rubric quality is deterministic evidence completeness, **not** LLM-as-a-Judge. Synthetic reference scores are not proof of genuine Azure improvements.

### Backend gate and provenance

- Technical quality >= 90%, no missing required latency, call-reduction >= 20% and modeled-duration objective >= 15%, followed by policy enforcement in ASP.NET Core. These are demo criteria, not measured Azure outcomes.
- Technical PASS **does not** imply a human approval, a GitHub merge, a publication or an Azure deployment. The actual human approval state is `NOT_REQUESTED` in this slice.
- Every report includes synthetic origin, the fixture SHA-256, version traces, a structured event sequence, the explicit reason and result, and a local rollback proof when applicable.
- No actual Azure ARM/Monitor/Policy/Cost Management calls, Microsoft Foundry models, Copilot Studio agents, Teams approvals, pull requests, live SSE during workflow execution or self-learning nativo are implemented by this slice.

## Run locally

1. `dotnet run --project src/MerlinOps.Api` (API at `http://localhost:5080`)
2. `cd slides && npm install && npm run dev` (Slidev at `http://localhost:3030`)
3. Open slide **7**, "Érika: ¿ha mejorado realmente el agente?", select the case and click **Ejecutar comparación local**.
4. Or call the API directly:

```bash
curl -X POST http://localhost:5080/api/erika/compare \
  -H 'Content-Type: application/json' \
  -d '{"caseId":"finops-success"}'
```

The public GitHub Pages site never exposes a protected presenter control or private live API. It only shows a clearly synthetic introduction when no local backend is present.

## What must happen next (Érika's acceptance order)

1. **Restore the three-product story:** Copilot Studio as entry surface, Microsoft Agent Framework as the .NET multiagent workflow and Microsoft Foundry for real model inference/interpretation. See [core architecture](demo-core-architecture.md).
2. Instrument MAF v1/v2 execution with OpenTelemetry and genuine incremental event emission. Run against **the existing synthetic fixtures**, with measured local durations and independently verified Foundry token/latency data where available.
3. Validate Foundry model integration using an existing authorized project and then establish an authenticated Copilot Studio → MAF connection via an environment-supported route (A2A or REST).
4. Add private presenter control for Érika/David, projected read-only Slidev, tests and deterministic backend gates. Keep human approval separate from technical PASS.
5. Rehearse connected improvement / policy DENY / verified **local** rollback with offline Replay. **Do not create a Container App for this.** A real read-only Container Apps adapter is optional and not a blocker.

**No Azure resources are created or modified by this feature.** No consent to enable Azure writes, deployment or cloud rollback has been granted.
