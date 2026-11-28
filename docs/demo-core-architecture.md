# Demo core — Copilot Studio + Microsoft Agent Framework + Microsoft Foundry

> **User correction / highest priority:** Azure Container Apps is **not** an infrastructure prerequisite and must not distract from the integration of these three products. This document overrides the prioritization order in earlier draft plans, but does **not** erase the 78 original or 77 additional accepted design choices.

## What was originally agreed

- Decision 2: **Copilot Studio + Microsoft Agent Framework** hybrid.
- Decision 4: **MAF with .NET** orchestrator.
- Decision 11: **Microsoft Foundry** workloads/model reasoning.
- Decisions 6, 13 and 22/23: Quality Gates and governance, with an independent human decision for risky operations.
- Decision 7: **Azure Container Apps as a potential diagnostic target**, not as mandatory application hosting and not a resource to create.
- Cross-cutting requirement: **Slidev Live Orchestration** with a separated private presenter view and read-only projected Mission Control.

## Final scope is governed by [the ten user decisions](ten-decisions-implementation.md)

The **primary case is MerlinOps optimizing its own incident-resolution workflow**. Container Apps is no longer the central case; it remains an optional secondary synthetic example. Foundry ai-omniroute-dev will be used only after its endpoint and two deployed models are verified and the configuration is reviewed. A2A is preferred for Copilot Studio; protected REST is the fallback only if A2A cannot be validated. All ten choices are listed with status in the linked decision document.

## Conference demonstration

```text
Audience/Érika (question or incident)
           |
           v
Copilot Studio agent -- delegated task (verified A2A or REST connection) -->
           |
           v
.NET 10 / Microsoft Agent Framework: MerlinOps supervisor
   |      |          |          |
TelemetryGuardian  FinOpsGuardian  QualityGuardian  GovernanceGuardian
   |                |              |
   +--- versioned tool workflow, local fixtures / approved READ-ONLY tools --+
           |
           v
Microsoft Foundry: actual permitted model inference, proposals & explanation
           |
           v
.NET backend deterministic evaluation + Policy as Code
   | DENY => stop without approval or deployment
   | PASS => human review requested; never automatic cloud changes
           |
           v
Slidev: projected timeline, evidence / v1-v2 chart, MerlinOps reactions, Replay
```

**Presenter controls are backend-authorized.** A Copilot Studio message, an agent/tool output or an on-stage button is never itself a cloud deployment authorization.

### Érika's three demonstrations (same concrete incident; one narrative)

1. **Improvement**: MAF v1 repeats synthetic **incident-resolution** diagnostic tools; v2 adopts an explicit evidence-collection workflow. Use identical input evidence, record actual local tool counts, real local durations where instrumented, and separate Foundry token usage if verified. Apply exact quality gates and explain the proposal. No automatic approval.
2. **Governance**: a deliberately unsafe recommendation (for example, increasing incident retries above the backend policy ceiling) is **DENY** in the backend before human-approval or change events.
3. **Late regression**: a synthetic post-change quality drop triggers a **verified local incident-workflow version** restore. Stage notes must say local, not Azure production rollback.

### Azure Container Apps decision

**Do not provision any Container App solely to enable this story.** The synthetic Container Apps snapshot is a concrete *domain case*, not a deployed prerequisite. Reuse this diagnostic domain if useful; switch to a Foundry-agent/tool-routing incident if it improves the conference narrative without changing the gate criteria. The optional `tools/azure-containerapps-readonly.ps1` is **P2, opt-in** and not needed to start implementing Copilot Studio/MAF/Foundry.

### Implementation order (highest priority first)

| Priority | Deliverable | Completion evidence |
|---|---|---|
| P0 | Working local **MAF** incident orchestration and deterministic Guardian workflow; matching v1/v2 synthetic fixtures and gates | Real local workflow executions, measured local telemetry, tests for improvement, DENY and verified local rollback |
| P0 | **Foundry model** invocation for reasoning/explanations using an existing authorized project and Entra identity where compatible | Explicitly validated model response and latency/usage provenance; local fallback |
| P0 | **Copilot Studio** agent invokes a protected MAF endpoint/agent over a supported, environment-verified connector | Real invocation, trace ID and a response in Copilot Studio; negative authorization tests |
| P0 | **Slidev** displays accurate correlated events and supports private presenter commands with read-only projection | Browser E2E and actual event timing; safe replay fallback |
| P1 | GitHub/Teams human review | Authenticated approval, audit, backend policy guard; simulated stage mode if unsupported |
| P2 | Optional real Azure Container Apps telemetry or other richer data source | Only with a separately authorized read-only resource; never required to pass P0 |

**Current implementation truth:** A .NET 10 MAF graph with deterministic executors, local C# v1/v2 comparator, Slidev synthetic view and policy checks exist on the feature branch. A true Copilot Studio ↔ MAF connection and a true Foundry inference invocation are **not implemented or validated** in the repository. Thus the project must not be presented as an integrated 3-product demo yet.

## Integration choices requiring verification

1. Copilot Studio supports external agents over A2A and tools/REST connectors. Prefer one **authenticated**, tested connection. Local testing can use a private development tunnel with the required auth configured; do not expose unauthenticated control endpoints.
2. Agent Framework supports model and agent integrations with Microsoft Foundry. The concrete SDK/client and the Foundry project's API/region/model deployment must be validated in CI/local rehearsal rather than assumed.
3. Feature support varies by Copilot Studio harness/experience, Foundry agent type and protocol. Do not claim all Foundry agents or arbitrary A2A endpoints are interchangeable. Record actual supported combination during preflight.

### Official reference links

- [Copilot Studio: connected agents overview](https://learn.microsoft.com/microsoft-copilot-studio/agents-experience/authoring-add-other-agents)
- [Copilot Studio: external A2A agents](https://learn.microsoft.com/microsoft-copilot-studio/agents-experience/add-agent-to-agent)
- [Copilot Studio: REST/API tools](https://learn.microsoft.com/microsoft-copilot-studio/add-tools-custom-agent)
- [Agent Framework: agent services](https://learn.microsoft.com/agent-framework/integrations/by-component/agent-services/)

**No Azure resource creation or modification is authorized.**
