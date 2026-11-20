# Architecture — MerlinOps and the Azure Guardians

~~~mermaid
flowchart TD
    A[Slidev Mission Control] -->|synthetic fixture| R[Replay JSON]
    A -->|SIMULATED SSE/REST| API[.NET 10 API]
    API --> DB[SQLite run snapshots]
    API --> P[YAML guardrails]
    P --> G{Policy passes?}
    G -->|No| DENY[Stop and audit]
    G -->|Yes| HITL[Human approval needed]
    HITL -. future .-> TEAMS[Entra + Teams / Power Automate]
    API -. adapter not wired .-> MAF[Microsoft Agent Framework]
    MAF -. future LIVE .-> FOUND[Foundry + Copilot Studio]
    HITL -. Azure writes not authorized .-> TF[Terraform deployment]
~~~

## Implementation truth

| Capability | Current state |
|---|---|
| .NET 10 deterministic three-scenario API | Implemented, build verification pending |
| Local YAML fail-closed policy checks | Implemented |
| SQLite stored run snapshots | Implemented |
| Read-only SSE event stream | Implemented for SIMULATED, not LIVE |
| Slidev Mission Control + MerlinOps procedural 3D | Implemented, browser verification pending |
| Replay files | Synthetic fixtures only, not authentic captured Azure traces |
| Microsoft Agent Framework integration | Separate SDK smoke adapter pending build verification |
| Real specialist AI agents, model routing | Not wired |
| Foundry consumption and billing | Not wired |
| Copilot Studio GitHub Copilot Harness + M365 Agents SDK | Compatibility unverified |
| Entra + backend RBAC + Teams human approvals | Not implemented |
| OpenTelemetry Azure integration | Not implemented; event trace IDs are illustrative |
| Azure Table/Blob persistence | Future adapter |
| Terraform | Optional resources, never applied |
| Real automatic rollback | Not implemented; scenario is SIMULATED |
| Azure infrastructure | No provisioning authorized |

## Trust model

The public slides consume versioned fixtures or local synthetic SSE. LIVE execution requires Entra-backed authentication and backend authorization; these controls are not in place and no LIVE endpoints exist. Data from any real trace must be reviewed and sanitized before publication. Critical policy failures must prevent approval and deployment; demo approvals never authorize real action.

## Replay contract

Every event carries sequence, kind, agent, message, gate and traceId; metric is optional. Each run has id, scenario, mode, status and createdAt. The synthetic source remains distinguishable in the UI. Future Event Sourcing/OpenTelemetry integrations must preserve correlation, sequence, provenance and retention rules.

## Official references

- Microsoft Agent Framework workflows: https://learn.microsoft.com/agent-framework/get-started/workflows
- Copilot Studio SDK integration and harness support caveat: https://learn.microsoft.com/microsoft-365/agents-sdk/integrate-with-mcs
- Slidev static hosting: https://sli.dev/guide/hosting

## Local Replay privacy preparation

`tools/sanitize_replay.py` generates an unpublished `REVIEW_REQUIRED` candidate, checked against tests in CI. It does not publish authentic events, guarantee full anonymization or replace mandatory manual approval.
