# Slidev Live Orchestration — accepted architecture requirement

[← Architecture](architecture.md) · [Design decisions](decisions.md) · [Roadmap](roadmap.md)

**Status: ACCEPTED DESIGN REQUIREMENT — NOT IMPLEMENTED AS LIVE.**

The NetCoreConf demo must be orchestratable directly from the Slidev presentation, while projecting a clean, read-only mission view. This is a new cross-cutting requirement, not a replacement for any of the 78 original decisions, the 17 subsequent questionnaire choices or Érika's recommendations.

## Presenter and audience surfaces

- **Presenter Control** (private, locally accessed and authenticated for any real actions): start v1, start v2, compare results, inspect evaluations, submit approval/rejection, request local rollback, pause or advance **Replay**, recover from error and choose an existing permitted Container App target.
- **Audience Mission Control** (read-only): animated MerlinOps and four Guardians, event timeline, quality/cost/latency/call-count comparisons, source provenance, proposal/evidence/policy decision and final outcome. It must never allow authorization or deployment by clicking projected UI.
- **Shared session state** lives in ASP.NET Core, not in slide component memory; Slidev presenter mode alone does not synchronize custom workflows. A session identifier and read-only audience subscription provide control/view separation.
- **Static GitHub Pages** stays public Replay-only; it cannot talk to private control endpoints or display actual confidential telemetry.

## Command and event contract

```text
Presenter Slidev (private) -- POST authenticated commands --> ASP.NET Core .NET 10
   commands: start-v1 | start-v2 | evaluate | request-approval |
             approve | reject | rollback-local | cancel-run
   backend: validate identity/role, state transition, policies, authorization,
            idempotency key, audit, read-only Azure capability
   workflows: Microsoft Agent Framework, Guardian adapters, evaluation gates
   events: persist append-only event + monotonic sequence and trace ID
Audience Slidev (read-only) <-- SSE events + reconnection/catch-up -- backend
Presenter Slidev (private) <-- command receipts and session snapshots -- backend
Offline fallback: versioned SYNTHETIC Replay with presenter step/pause/reset
```

A UI command is merely a **request**. The backend is the authority for authorization, guardrails, accepted state transitions and durable outcomes. An evaluation PASS is not human approval; a human approval is not by itself permission to modify Azure. Existing prohibitions on Azure mutations remain unchanged.

### Execution model

1. Presenter selects a pre-authorized target/scope, scenario and run mode. For authentic Azure analysis, use Azure CLI/Entra locally or future Managed Identity through `DefaultAzureCredential`, read-only scopes. Fall back to local synthetic fixtures if permissions or connectivity fail.
2. Presenter starts **v1**; backend begins execution and emits actual events **as they happen**, not after completion.
3. Presenter starts **v2** using the same captured input evidence/conditions to make comparison fair. Distinguish runtime-observed, estimated and synthetic values.
4. Backend evaluates identical normal/exception/policy-block cases, checks quality >= 90%, no critical regressions, objectives of 20% fewer tool calls and 15% less duration, and mandatory policy compliance. Objectives are acceptance thresholds, not claimed achievements.
5. Mission Control displays the proposal, evidence, policy version and gate outcomes. Presenter may request an authorized review; only authenticated backend review records can approve/reject. Simulated approvals always carry `SIMULATED`.
6. If the improved local workflow later regresses, a backend operation restores the **versioned active local workflow configuration**, confirms v1 is active, and records the result. No Azure rollback or deployment is allowed.

### Streaming, integrity and recovery

- A live SSE event must be emitted from the executing workflow or its event store as soon as committed; never produce `LIVE` from delayed playback of a completed run.
- Events include sessionId, runId, sequence, traceId, timestamp, kind, actor, status and provenance (`LIVE`, `SIMULATED`, `REPLAY`). Replay data cannot silently switch to LIVE. Preserve origin and policy/approval evidence.
- Restore after disconnect via `Last-Event-ID` or equivalent resume cursor, with deduplication and a consistent snapshot after refresh. Do not restart commands after a dropped SSE connection.
- Presenter mode needs an explicit **safe-to-project** boundary (separate route, access control and minimized confidential text). Reject control actions from unauthenticated viewers and read-only endpoints.
- Animate MerlinOps success **only** after final mission success, never after an individual Guardian `agent.completed`. Failure, DENY, rejection and rollback have distinct terminal states.
- Provide accessible high-contrast text, keyboard controls, reduced-motion/no-WebGL fallback, and a clear mode badge on every run.
- Emergency controls: cancel a local run, return to deterministic Replay, pause/step/reset Replay, reset view and restore after backend failure. Pause is not implied to suspend already running AI/Azure calls.

## Implementation slices and verification

1. **Slice 1 — backend orchestration and streaming**: session IDs, explicit command state machine, async run coordinator, append-only event storage, truly live SSE, replay cursor and isolated read-only audience routes. No Azure write operations.
2. **Slice 2 — presenter UX**: private presenter controls embedded in Slidev, read-only projector view, keyboard shortcuts, scenario switching and clear provenance; run independent of slide navigation.
3. **Slice 3 — evidence/evals/governance**: deterministic v1/v2 on identical evidence, standardized evaluation report, backend-enforced approval, local version rollback and visible audit trail.
4. **Slice 4 — validated read-only Azure**: Container Apps, ARM, Monitor, Log Analytics, Policy and Cost Management adaptors, all permission-scoped; offline snapshot fallback; no infrastructure creation.
5. **Slice 5 — conference readiness**: browser E2E for savings, DENY and rollback; negative authorization tests; reconnect/resume, duplicate events, backend errors, switch while running, pause/reset, keyboard/reduced motion and offline rehearsals.

**Definition of Done:** during a rehearsal, the presenter's Slidev controls genuinely trigger a local run, project incremental correlated events, compare v1/v2, enforce policy and human decision separately, restore an actual versioned local configuration when needed, and recover into clearly labelled Replay. The presenter/projector separation and all negative security tests must pass. No Azure resources are created or modified.

## Actual repository state when this was agreed

The Vue Mission Control can select three scenarios, play/pause/step/reset SYNTHETIC Replay and invoke a locally simulated MAF run. However, `/api/runs/maf-simulate` completes before the UI subscribes to `/api/runs/{id}/stream`, which then emits saved events with a delay. Therefore this is **not live workflow streaming**. There is no dedicated private presenter surface, backend command authorization or working v1/v2 comparison and local rollback yet.
