# Ten final decisions — NetCoreConf Barcelona 2026 MerlinOps

> Accepted by the owner on 2026-11-21. **No further architecture questionnaire.** This is the scope baseline for PR #23 and subsequent implementation work. The original 78 decisions, 77 subsequently accepted D options, Slidev Live Orchestration and every recommendation from Érika remain recorded. Where any earlier detail conflicts, these final ten scope priorities take precedence.

## The agreed ten answers (verbatim intent)

| # | Choice | Binding decision | Implementation / proof |
|---|---|---|---|
| 1 | **A** | MerlinOps improves its **own incident-resolution workflow** through evidence and evaluations. | New `IncidentImprovementEngine` executes versions v1/v2 over `INC-2042`, no Container App provisioning. |
| 2 | **C** | Synthetic datasets for fair comparisons, **real telemetry of agent execution**. | Immutable synthetic fixture SHA-256; actual local tool-call counts, MAF Guardian completions and Stopwatch wall-clock on local operations; explicitly distinguished from synthetic service/Cloud metrics. |
| 3 | **A** | Existing **development Copilot Studio environment**. | Connection NOT CONFIGURED. Owner will review tenant/environment changes before they occur. No agent created in this PR. |
| 4 | **A** | Existing **ai-omniroute-dev** Foundry project, only after permissions and endpoint check. | Name in preflight; no remote access or assumption that model deployments exist. |
| 5 | **C** | Verified primary + backup model; quality, latency and tokens. | Endpoint-shape and distinct-model preflight; actual model invocation/usage **not yet implemented**, values null, not zero. |
| 6 | **C** | A2A preferred; protected REST fallback if supported/verified. | A2A compatibility and OAuth2 requirements documented. Neither is exposed or connected by the repository yet. |
| 7 | **A** | Full Copilot Studio → MAF → Foundry → evaluations → Slidev should run live at the talk. | **Target, not current implementation.** Stage LIVE is prohibited until fully tested with authenticated sessions, real streaming, correlation, and no unauthorized cloud writes. |
| 8 | **C** | Combined efficiency, quality, security, audit and cost when measurable. | New report counts local tools, measures local duration, records quality/policy gates, source SHA-256, sequence audit and null cost/token fields. |
| 9 | **C** | Érika challenges agent, evaluates evidence and approves/rejects improvement; backend has final policy authority. | Policy DENY is enforced independently; technical PASS is only `AWAITING_HUMAN_REVIEW` and not a real human approval. Presenter role and authenticated approval are later slices. |
| 10 | **C** | Code in GitHub now; **review before any Copilot Studio or Foundry configuration**. | Changes staged as draft PR. No Azure/cloud resources or agent configuration modified. |

## Conference narrative anchored to the title

**«El aprendiz de agente: magia, telemetría y gobernanza bajo control»**

1. **El aprendiz**: Copilot Studio receives an incident and delegates an authenticated task to MerlinOps; MAF runs a four-Guardian incident workflow. Today this is a **planned cloud integration** and the local MAF engine is real.
2. **Magia**: MerlinOps discovers v1 performs redundant diagnostic tool calls and proposes a versioned v2. Show the observable improvement, no misleading model intelligence claims.
3. **Telemetría**: identical fixture and rubric for both versions, local counted tool calls (9 vs 4 in the deterministic example), actual local elapsed time, correlation/evidence hash, quality, and Foundry usage only **if really obtained**.
4. **Gobernanza**: Érika challenges an unsafe retry budget; backend Policy-as-Code returns DENY before requesting review. For a late regression, show an actual **local** workflow manifest restored from v2 to v1, SHA-256 validated.
5. **Control**: Slidev Mission Control makes these state changes clear; projected controls cannot approve or modify infrastructure. The public site remains Replay.

## Implementation gates — define DONE without pretending

| Gate | Work | Status |
|---|---|---|
| G0 | Real local MAF workflow on incident scenario, benchmark and tests | Implemented in PR; CI confirmation required for latest revision |
| G1 | Foundry primary inference; approved endpoint identity and primary/fallback model; capture actual token counts and observed local/network latency | **NOT CONNECTED** |
| G2 | Copilot Studio authenticated A2A connection to .NET MAF, or tested protected REST fallback | **NOT CONNECTED** |
| G3 | Genuine incremental event stream while MAF and Foundry run, plus isolated authenticated presenter controls in Slidev | **NOT IMPLEMENTED** |
| G4 | One end-to-end authorized live rehearsal of improvement, DENY and local rollback; full eval report and offline fallback | **NOT VERIFIED** |

**Nonnegotiable:** no Azure Container App creation is needed. The old Container Apps comparator and optional read-only Azure CLI evidence script may remain as secondary code examples; the primary stage story is the agent's **own incident workflow**.

## Secure future setup (review required)

- `MERLINOPS_FOUNDRY_PROJECT_ENDPOINT`: verified HTTPS Foundry project URL from the existing `ai-omniroute-dev` project, never guessed.
- `MERLINOPS_FOUNDRY_PRIMARY_MODEL` and `MERLINOPS_FOUNDRY_FALLBACK_MODEL`: distinct, actual verified model deployments. Model selection only after testing each model's endpoint/API support and meterable usage.
- Identity via verified Entra application/service principal and documented least-privilege roles; do not commit credentials.
- Copilot Studio agent in an existing development environment, with auth-protected A2A agent card/endpoint or authenticated REST tool. Remote endpoints must never be exposed anonymously for convenience.
- Review costs and the 115 EUR/month goal before enabling model calls; no commitment to actual charge until usage data exists.

## Local verification

```bash
dotnet test tests/MerlinOps.Tests/MerlinOps.Tests.csproj
dotnet run --project src/MerlinOps.Api
# separate terminal:
cd slides && npm install && npm run dev
# in a further terminal:
curl -X POST http://localhost:5080/api/incident/compare \
  -H 'Content-Type: application/json' \
  -d '{"caseId":"incident-improvement"}'
curl http://localhost:5080/api/integrations/preflight
```

**Local API run is self-contained.** No Foundry credentials or Copilot environment is required to reproduce all deterministic incident scenarios. The new slide is #7; GitHub Pages stays read-only.

## Microsoft official guidance

- [A2A connected agent in Copilot Studio](https://learn.microsoft.com/microsoft-copilot-studio/agents-experience/add-agent-to-agent)
- [Agent Framework Foundry model provider](https://learn.microsoft.com/agent-framework/agents/providers/microsoft-foundry)
- [Foundry Agents / direct Responses API](https://learn.microsoft.com/agent-framework/integrations/by-component/agent-services/foundry)
