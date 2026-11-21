# Accepted design decisions (78)

> These decisions define intent, not verified implementation. No Azure resources may be created or modified without separate explicit authorization.

| # | Choice | Decision |
|---:|:---:|---|
| 1 | B | Visual technical storytelling |
| 2 | C | Copilot Studio + Agent Framework hybrid |
| 3 | D | Enterprise multiagent system |
| 4 | B | MAF .NET orchestrator |
| 5 | B | Azure Guardians specialists |
| 6 | C | Human approval for real changes |
| 7 | A | Azure Container Apps conference target |
| 8 | C | OpenTelemetry and visual evidence |
| 9 | D | Slidev-integrated Mission Control |
| 10 | D | FinOps optimization focus |
| 11 | C | Foundry workloads |
| 12 | C | Optimize model/tokens/latency/quality/cost |
| 13 | C | Continuous eval and quality gates |
| 14 | B | Terraform + Actions |
| 15 | D | Ephemeral tests + stable demo |
| 16 | B | 115 EUR/month target alerts |
| 17 | C | Fluent + magic visual style |
| 18 | B | 45-minute length |
| 19 | C | Érika/David dynamic co-presenting |
| 20 | A | Presenter-only controls |
| 21 | C | LIVE with authentic Replay contingency |
| 22 | B | Copilot Studio governance specialist |
| 23 | C | Teams/Power Automate approved requests |
| 24 | D | Pipeline-protected AI infrastructure changes |
| 25 | C | Zero Trust and robust agent security |
| 26 | C | Copilot/Claude/Codex engineering tools |
| 27 | B | GitHub Flow collaboration |
| 28 | C | Aspire monorepo |
| 29 | C | DevSecOps CI |
| 30 | C | Public reproducible OSS |
| 31 | A | MIT original code |
| 32 | C | Slidev visual/accessibility/E2E tests |
| 33 | C | GitHub Pages and static exports |
| 34 | B | Vertical slice first |
| 35 | C | Autonomous initial main changes |
| 36 | A | No Azure changes before authorization |
| 37 | C | Mocks, contracts and Aspire |
| 38 | D | Comprehensive architecture with honest gaps |
| 39 | C | Synthetic/licensed/sanitized sources |
| 40 | D | Extensible dynamic model routing |
| 41 | C | Estimated vs observed savings |
| 42 | C | Multidimensional quality gates |
| 43 | C | Slidev animated Mission Control |
| 44 | A | Public Replay/private Entra Live |
| 45 | A | Existing Entra dev tenant |
| 46 | A | Tenant and subscription availability asserted |
| 47 | A | NET 10 and pinned SDK |
| 48 | B | Spanish slides/English docs |
| 49 | A | Official Copilot Studio API/SDK target |
| 50 | B | GitHub Copilot harness SDK support unverified |
| 51 | A | GitHub versioned policies |
| 52 | D | YAML/OPA/Azure hybrid policy |
| 53 | C | Fail closed critical policy |
| 54 | C | GitHub sanitized/Azure private evidence |
| 55 | C | Automated anonymization + human review |
| 56 | C | Event Sourcing + OTel Replay |
| 57 | C | SSE, protected REST and fallback |
| 58 | D | Dynamic planning/parallel/gates |
| 59 | D | SQLite local/Azure Tables Blob future |
| 60 | C | Checkpoints and idempotency |
| 61 | C | Retries/circuit breakers/fallback |
| 62 | C | FinOps budgets per run/agent/env |
| 63 | C | Progressive delivery and rollback |
| 64 | C | Three connected scenarios |
| 65 | C | Two LIVE, rollback Replay |
| 66 | D | Theatrical flexible timing |
| 67 | C | Opposing masters: governance vs automation |
| 68 | C | Apprentice visual plus real orchestrator |
| 69 | A | 3D futuristic wizard |
| 70 | C | Hybrid procedural/prerendered/fallback animation |
| 71 | C | Event-grounded reactions with personality |
| 72 | C | MerlinOps name |
| 73 | B | Close five critical decisions then build |
| 74 | B | Adopt Preview SDKs |
| 75 | C | Explicit LIVE activation with Entra/RBAC |
| 76 | C | Conference readiness gates |
| 77 | D | Autonomous multiagent development |
| 78 | C | Integral delivery Definition of Done |

## Safety rules
- Public Replay data must be sanitized and reviewed; current files are explicitly SYNTHETIC fixtures.
- LIVE is an intended mode only: authentication, RBAC and approval do not yet exist.
- GitHub Copilot Harness compatibility with M365 Agents SDK remains unverified.
- A simulated approval has no authority to trigger Azure changes.
- Initial autonomous commits to main are authorized; later collaborators use reviewed PRs.

## Additional accepted decisions — Érika's feedback and case design (17)

These choices **supplement** the original 78, which remain unchanged. They specify a progressively validated target; a selected tool or LIVE mode is not proof of an implementation.

| # | Choice | Additional decision |
|---:|:---:|---|
| 1 | C | Hybrid optimization: fewer redundant tool calls via explicit workflow, model/cost optimization as later extension |
| 2 | B | Azure Operations Guardian as the concrete agent use case; no automatic Azure changes |
| 3 | B | Azure Container Apps as the primary diagnostic resource |
| 4 | C | Integral cost/performance diagnostic with connected savings, policy DENY and rollback scenarios |
| 5 | C | Deterministic evidence collection/validation workflow; selective agent reasoning for interpretation and proposals |
| 6 | D | Azure Resource Manager, Monitor, Log Analytics, Policy and Cost Management target integrations |
| 7 | D | Hybrid identity: local Azure CLI/Entra and future Managed Identity via DefaultAzureCredential |
| 8 | D | Progressive, configurable subscription/resource-group/app scope; first real demo: a single app |
| 9 | C | Existing authorized read-only Container App if available; local synthetic adapter as fallback |
| 10 | C | Automated v1/v2 test suite and comparative quality-gate report |
| 11 | D | Hybrid evaluations: deterministic checks, operational metrics and optional LLM-as-a-Judge |
| 12 | D | Quality >=90%; no critical regression; objectives: -20% tool calls, -15% duration; all policies mandatory |
| 13 | D | GitHub PR/version gates + eventual Teams operational human approvals; simulated decisions in Replay |
| 14 | D | Verifiable **local** versioned workflow rollback with GitHub traceability and Replay contingency |
| 15 | D | OTel-based hybrid observability, local audit persistence and optional Azure exporter; provenance visible |
| 16 | D | Integral Mission Control: Guardian graph, timeline, v1/v2 evidence and mission-level outcome |
| 17 | D | Genuine runtime SSE, ordered persisted events, correlation, reconnect/resume and distinct Replay |

### New cross-cutting requirement: Slidev Live Orchestration

The presentation itself will become an operator surface: **private presenter controls** send authenticated commands to an enforcing .NET 10 backend, while the public/projected Mission Control is **read-only**. This is a confirmed requirement outside the 17-choice questionnaire, not a replacement decision. See [Slidev Live Orchestration architecture](live-orchestration.md) and [implementation tracking](https://github.com/davidop/netcoreconf-barcelona-2026/issues/22).

The present implementation does **not** yet support actual runtime streaming, private presenter controls, real version comparison or local rollback. These remain outstanding. Azure creation, mutation, deployment and rollback require separate explicit authorization.

### Érika's governing recommendations

- Keep native Microsoft self-learning optional and independently verified. Without that feature, call the pattern **supervised improvement inspired by self-learning**, never native self-learning.
- Define one concrete Azure Container Apps request with known evidence, tooling and expected output. v1 and v2 must use comparable evidence and normal/exception/block test cases.
- Show an observed improvement, a backend policy DENY that cannot reach approval or execution, and a late regression with verified restoration of local v1.
- Gate decisions in the backend and separately record proposal, evidence, policy version, technical decision, authorized person and actual outcome. Reject missing policy/metrics, exceeded limits and unapproved actions.
- Always label **LIVE / SIMULATED / REPLAY** and observed / estimated / synthetic metrics. An `agent.completed` event must not mark the full mission successful.
- Do not claim Teams, Azure, Foundry, or self-learning integrations are real until validated. Maintain a conference-ready replay/offline fallback.
