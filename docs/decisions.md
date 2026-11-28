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

## Further accepted decisions from the user (18–77)

The user requested **no more questionnaires: start building the demo now**, with Érika's acceptance criteria as the primary prioritization. The following options were selected as **D** throughout the follow-up questionnaire. They are **target decisions, not claims of completed implementation**.

| # | Selected | Requirement |
|---:|:---:|---|
| 18 | D | Supervised improvement inspired by self-learning; native self-learning only if independently validated |
| 19 | D | Hybrid automatic and presenter-triggered improvement discovery |
| 20 | D | Hybrid deterministic/AI-generated, versioned v2 proposal |
| 21 | D | Progressive validation: static checks, sandbox, regressions and security before PR |
| 22 | D | Hybrid GitHub PR, Actions and Replay contingency |
| 23 | D | Authenticated Teams/Adaptive Card approval with explicit simulation fallback |
| 24 | D | Two presenters with one active operator and distinct human-approval authority |
| 25 | D | Secure handover and supervised operator recovery |
| 26 | D | Zero Trust Entra/RBAC/private presenter access |
| 27 | D | Multi-level LIVE → SIMULATED → REPLAY contingency |
| 28 | D | Aspire orchestration, scripts and offline preflight |
| 29 | D | CLI/CI/Slidev preflight readiness traffic light |
| 30 | D | Private stage timer, per-scene budgets and contingency cues |
| 31 | D | E2E, rehearsal, resilience testing and GO/NO-GO report |
| 32 | D | Complete public community reference kit |
| 33 | D | Governed open-source contributions and extension catalog |
| 34 | D | Pages, Dev Containers/Codespaces and Aspire reproducibility |
| 35 | D | Semantic releases, conference freeze and workflow/policy versioning |
| 36 | D | Protected conference branch, Release Candidate, joint review and offline backup |
| 37 | D | Incident classification, safe recovery, verified hotfix and offline fallback |
| 38 | D | Presenter incident response with dynamic roles |
| 39 | D | Transparent, accessible incident/recovery storytelling |
| 40 | D | Consultative audience participation through QR |
| 41 | D | Hybrid resilient lightweight mobile voting |
| 42 | D | Three narrative participation moments |
| 43 | D | Anonymous voting, abuse protections and isolated public API |
| 44 | D | Accessible live voting charts with controlled reveal |
| 45 | D | Compare audience perception with technical evidence |
| 46 | D | Adaptive poll pacing and presenter override |
| 47 | D | Aggregated privacy-preserving voting report |
| 48 | D | WCAG 2.2 AA accessibility target and verification |
| 49 | D | Adaptive accessible animation, static fallback and correct mission states |
| 50 | D | Progressive disclosure in public Mission Control |
| 51 | D | Evidence-linked explanations with optional AI summaries |
| 52 | D | Multi-level FinOps limits and observed/synthetic provenance |
| 53 | D | Governed model routing and fallback by role/task/budget |
| 54 | D | Controlled multimodel retries, checkpoints and reevaluation |
| 55 | D | End-to-end data classification, minimization, privacy and access |
| 56 | D | Prompt-injection defense in depth and negative tests |
| 57 | D | CI security gates for SAST, dependency, auth and policy failures |
| 58 | D | Supply-chain controls, SBOM, action pinning and provenance |
| 59 | D | Secretless-first identity, vault as needed and audit |
| 60 | D | Versioned configuration by environment with preflight |
| 61 | D | Versioned Policy-as-Code with impact tests and independent approvals |
| 62 | D | End-to-end auditable decisions with integrity proofs |
| 63 | D | Governed evidence retention and verifiable deletion |
| 64 | D | Typed OpenAPI/event contracts and compatibility gates |
| 65 | D | Durable idempotent execution and state transitions |
| 66 | D | Isolated controlled concurrency and serialized critical operations |
| 67 | D | Guardian-specific retry, partial results and fail-closed evidence checks |
| 68 | D | Multilayer output/schema/evidence/cross-agent validation |
| 69 | D | Governed agent disagreement resolution and human escalation |
| 70 | D | Bounded collaboration iterations, budgets and convergence detection |
| 71 | D | Governed shared context, specialized memory and selective persistence |
| 72 | D | TTL/version/provenance-validated evidence cache |
| 73 | D | Fair cold/warm v1/v2 benchmarking on identical evidence |
| 74 | D | Progressive local rollout, regression monitoring and safe restoration |
| 75 | D | Verify active local v1 manifest and functionality before RESTORED |
| 76 | D | End-to-end correlated proposal/evaluation/approval/rollback trace |
| 77 | D | Auditable evidence pack with structured JSON, visual report, hashes and review |

**Question 78 was never answered.** Do not assume a selection. Subsequent instruction supersedes the questionnaire: build the smallest end-to-end, Érika-focused demonstration immediately, with at most ten new essential questions across the work (ideally zero).

### Implementation prioritization

- **P0:** concrete Container Apps task; deterministic v1/v2 on equal evidence; explicit tools and reproducible quality/call measurements; backend DENY on missing metrics or risk; local rollback with proof; accurately labeled provenance; readable Mission Control.
- **P1:** real read-only Azure adapters, real-time OTel/SSE evidence, protected presenter controls and authenticated human review.
- **P2:** rich polls, multimedia, advanced memory, community extensions, cloud deployment and other presentation enhancements.

The first runnable slice is documented at [Érika-first demo](erika-first-slice.md).

## Final scope agreement — ten user answers (supersedes architecture questionnaires)

The owner confirmed ten definitive scope selections on **2026-11-21**, recorded with implementation evidence and outstanding integration gates in [Ten final decisions](ten-decisions-implementation.md). Answers: **1A, 2C, 3A, 4A, 5C, 6C, 7A, 8C, 9C, 10C**.

Primary mission is **MerlinOps optimizing its own incident-resolution workflow**, NOT creating Azure Container Apps. **Copilot Studio in an existing dev environment** → **.NET 10 MAF** → **Foundry project ai-omniroute-dev** (verified main+fallback models) → real evaluation/telemetry → **Slidev Live Orchestration** is the requested conference circuit. Érika remains an active adversarial governance/evaluation actor. GitHub code changes are authorized; any Copilot Studio or Foundry configuration requires review beforehand. Never treat cloud connectors as implemented merely because the architecture names them.

## Infrastructure clarification (2026-11-24)

Foundry is an **optional managed model provider**, not a dependency to execute MAF locally. The LIVE conference objective still includes Copilot Studio → MAF → Foundry, but **all Azure infrastructure must be Terraform templated** with **zero-create defaults** and explicit approval gates. Prefer existing `ai-omniroute-dev` reference-only reuse; optional new Foundry account/project/model deployments are defined under [Terraform](../infra/README.md). Copilot Studio agent definitions use source-controlled [Power Platform ALM](../copilot-studio/README.md), not invented Terraform resources for unsupported platform features. No cloud writes authorized.
