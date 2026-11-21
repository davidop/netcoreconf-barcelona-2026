<p align="center"><img src="assets/banners/merlinops-hero.svg" alt="MerlinOps — El aprendiz de agente, magia, telemetría y gobernanza bajo control" width="100%"/></p>
<p align="center">
<a href="https://github.com/davidop/netcoreconf-barcelona-2026/actions/workflows/ci.yml"><img alt="CI" src="https://github.com/davidop/netcoreconf-barcelona-2026/actions/workflows/ci.yml/badge.svg"/></a>
<a href="https://github.com/davidop/netcoreconf-barcelona-2026/actions/workflows/e2e.yml"><img alt="Browser E2E" src="https://github.com/davidop/netcoreconf-barcelona-2026/actions/workflows/e2e.yml/badge.svg"/></a>
<a href="https://github.com/davidop/netcoreconf-barcelona-2026/actions/workflows/pages.yml"><img alt="Pages" src="https://github.com/davidop/netcoreconf-barcelona-2026/actions/workflows/pages.yml/badge.svg"/></a>
<img alt="MIT original code" src="https://img.shields.io/badge/code-MIT-2563eb?style=flat-square"/>
<img alt=".NET 10" src="https://img.shields.io/badge/.NET-10-512bd4?style=flat-square"/>
<img alt="SIMULATED and REPLAY" src="https://img.shields.io/badge/demo-SIMULATED%20%2B%20REPLAY-0891b2?style=flat-square"/>
</p>

<p align="center"><strong>NETCORECONF BARCELONA 2026 · 45 MIN · ÉRIKA CEPEDA × DAVID OLIVA PAREDES</strong></p>
<p align="center">
<a href="https://davidop.github.io/netcoreconf-barcelona-2026/"><strong>▶ ABRIR PRESENTACIÓN INTERACTIVA</strong></a> ·
<a href="docs/quickstart.md">⚡ Quickstart</a> ·
<a href="docs/diagrams/README.md">🧭 Arquitectura</a> ·
<a href="docs/scenarios.md">🧪 Escenarios</a> ·
<a href="docs/roadmap.md">🗺️ Roadmap</a>
</p>

---

## 🪄 What is MerlinOps?

**El aprendiz de agente** es una demo de ingeniería de agentes: **Érika** exige gobernanza y calidad; **David** impulsa automatización y FinOps. **MerlinOps** coordina cuatro Guardianes —Telemetría, FinOps, Calidad y Gobernanza— y demuestra que **una recomendación de IA no equivale a autorización para ejecutar cambios**.

La implementación actual ejecuta un **grafo real de Microsoft Agent Framework (MAF)**: delegación paralela a cuatro ejecutores deterministas locales y evaluación de Quality Gates mediante código. Los datos son **sintéticos** y las integraciones cloud están deshabilitadas.

> [!IMPORTANT]
> **Community sample — not an official Microsoft repository.** This project does **not** deploy to Azure, call Foundry models, integrate live with Copilot Studio, authorize real Teams approvals, or perform real rollbacks. Public demo data is explicitly **SIMULATED** or **REPLAY**. Azure creation and modification remain prohibited without separate explicit authorization.

### Technology at a glance

<table><tr>
<td align="center"><img src="assets/brand/microsoft-mark.svg" alt="Microsoft ecosystem mark" height="42"/><br/><sub>Microsoft ecosystem</sub></td>
<td align="center"><img src="assets/brand/dotnet.svg" alt=".NET" height="42"/><br/><sub>.NET 10</sub></td>
<td align="center"><img src="assets/brand/azure.svg" alt="Azure" height="42"/><br/><sub>Azure (future)</sub></td>
<td align="center"><img src="assets/brand/github.svg" alt="GitHub" height="42"/><br/><sub>GitHub Actions</sub></td>
<td align="center"><img src="assets/brand/vue.svg" alt="Vue" height="42"/><br/><sub>Slidev / Vue</sub></td>
<td align="center"><img src="assets/brand/terraform.svg" alt="Terraform" height="42"/><br/><sub>Terraform (opt-in)</sub></td>
</tr></table>

**Microsoft Agent Framework** · **Copilot Studio (roadmap)** · **Microsoft Foundry (roadmap)** · **OpenTelemetry (roadmap)**

## 🎬 See the experience

| MerlinOps | Mission Control |
|---|---|
| <img src="assets/mascot/merlinops.svg" alt="MerlinOps original wizard mascot illustration" width="260" /> | [![Mission Control illustration (not screenshot)](assets/previews/mission-control-illustration.svg)](https://davidop.github.io/netcoreconf-barcelona-2026/6) |
| Original reusable wizard mascot | **UI illustration, not a captured screenshot.** [Open actual slide 6](https://davidop.github.io/netcoreconf-barcelona-2026/6). |

![Three scenario cards: optimization, policy block and rollback](assets/scenarios/three-scenarios.svg)

**Authentic screenshots:** download the test artifacts from the passing [Slidev Browser E2E workflow](https://github.com/davidop/netcoreconf-barcelona-2026/actions/workflows/e2e.yml). Illustrations are never presented as actual captures.

## 🧱 Architecture at a glance

![Current public Replay, local .NET MAF, and disabled future LIVE integration boundaries](assets/diagrams/architecture-overview.svg)

**Current working slice:** local .NET 10 API → MAF workflow graph → four deterministic Guardians → fail-closed YAML policy → SQLite snapshot → SSE/Replay in Slidev.

**Public site:** GitHub Pages serves static Slidev and synthetic Replay fixtures; it does **not** access a privileged API.

**Future LIVE:** Entra authorization, verified human approval, Foundry/Copilot Studio/Teams connectors and Azure deployments. These are **not connected**. Explore [six architecture diagrams](docs/diagrams/README.md) and [security boundaries](docs/security.md).

## 🧑‍🚀 Three demo missions

| Mission | Purpose | Synthetic local outcome | Intended conference path |
|---|---|---|---|
| **01 · El ahorro** | Model routing, cost, latency and quality | PASS → **fictional** approval → synthetic projected 24% reduction | LIVE only after integration/authorization |
| **02 · El escudo** | Block a critical-risk proposal | **DENY** without downstream approval or deployment | LIVE only after integration/authorization |
| **03 · La reversión** | Quality regression and recovery | **Fictional** canary, regression and rollback | REPLAY |

The 24%, 0.42 USD, 0.94 quality score and 1850 ms p95 are **illustrative synthetic fixtures**, not observed costs or telemetry.

## ⚡ Quickstart — no Azure subscription needed

**Prerequisites:** .NET 10 SDK, Node.js 22+, npm. No Azure credentials, Foundry API keys or Copilot Studio tenant are required for local execution.

~~~bash
git clone https://github.com/davidop/netcoreconf-barcelona-2026.git
cd netcoreconf-barcelona-2026

# Terminal 1 — .NET 10 local API
dotnet run --project src/MerlinOps.Api
~~~

~~~bash
# Terminal 2 — interactive slides
cd slides
npm install
npm run dev
~~~

Open **Mission Control (slide 6)**. Replay works without any backend; on localhost you can activate **SIMULATED SSE** to execute the real MAF workflow with deterministic local executors.

~~~bash
curl -X POST http://localhost:5080/api/runs/maf-simulate \
  -H 'Content-Type: application/json' \
  -d '{"scenario":"policy-block"}'

dotnet test tests/MerlinOps.Tests/MerlinOps.Tests.csproj
~~~

[Detailed quickstart](docs/quickstart.md) · [Presenter runbook](docs/demo-runbook.md)

## 🔐 Provenance is a feature, not a footnote

| Mode | Source | Available | Claim |
|---|---|---|---|
| **REPLAY** | Synthetic, version-controlled JSON | ✅ Public web | Reproducible narrative; **not real Azure evidence** |
| **SIMULATED** | Local .NET/MAF workflow + SSE | ✅ Localhost | Real workflow mechanics; **fictional data and approval** |
| **LIVE** | Identity-protected Foundry, Copilot Studio and Azure | ❌ Not implemented or authorized | Future integration after separate approval and verification |

> [!CAUTION]
> A fictional approval never authorizes a deployment. Default policy denies cloud writes. GitHub Actions does **not** run Terraform apply.

## 📚 Documentation and navigation

| Resource | Use it for |
|---|---|
| [Getting started](docs/quickstart.md) | Prerequisites, commands and local checks |
| [Architecture](docs/architecture.md) and [six diagrams](docs/diagrams/README.md) | C4, modes, sequence, Guardian orchestration and trust boundaries |
| [Scenario playbook](docs/scenarios.md) | FinOps, critical-risk denial, and rollback |
| [Security](docs/security.md) | Threat boundaries, known gaps and governance |
| [Implementation status](docs/status.md) | Verified, simulated, scaffold and unverified features |
| [Design decision register](docs/decisions.md) | All **78 approved decisions** |
| [Presenter runbook](docs/demo-runbook.md) | Érika and David's 45-minute script |
| [Roadmap](docs/roadmap.md) | Backlog and graduation gates |
| [Media kit](docs/media-kit.md) and [brand guide](docs/brand-guidelines.md) | Original reusable visual assets and trademark conditions |
| [Agent engineering](docs/engineering.md) | GitHub Copilot, Claude Code and Codex specialization |
| [Contributing](CONTRIBUTING.md) · [Support](SUPPORT.md) | PR workflow and reporting channels |

## 🗂️ Repository map

~~~text
assets/                 Hero, social cards, wizard, illustrations and attributed logos
docs/diagrams/          C4 context, event sequence, execution modes and security
slides/                 Slidev, Mission Control and procedural MerlinOps 3D
src/MerlinOps.Api/      .NET 10 MAF simulation API, YAML policy, SQLite, SSE
src/MerlinOps.AppHost/  Local .NET Aspire orchestration
src/MerlinOps.AgentFrameworkAdapter/   SDK smoke test
tests/                  xUnit behavior and guardrail tests
policies/               Versioned fail-closed YAML rules
tools/                  Replay candidate sanitization with manual review
infra/                  Opt-in Terraform template, never automatically applied
skills/                 Engineering, governance and presenter UX agent skills
.github/workflows/      CI, E2E screenshot evidence, manual Pages publication
~~~

## 🤝 Community sample and contributions

This repository documents **78 approved decisions**, not 78 completed features. Follow the [roadmap](docs/roadmap.md) and [GitHub Issues](https://github.com/davidop/netcoreconf-barcelona-2026/issues); changes must preserve truthful labels, pass quality gates, and never create or modify Azure resources without separate user authorization.

The project is a community conference sample by **Érika Cepeda and David Oliva Paredes**, **not a Microsoft product or official Microsoft sample**.

**License:** [MIT for original code and original artwork](LICENSE). Third-party logos and trademarks remain subject to [their owners' conditions](assets/brand/README.md). [Social preview PNG](assets/banners/social-preview.png) · [Repository presentation setup](docs/repository-setup.md) · [Changelog](CHANGELOG.md) · [Contributing](CONTRIBUTING.md) · [Code of Conduct](CODE_OF_CONDUCT.md) · [Security reporting](SECURITY.md).

<p align="center"><strong>Magia sí. Observabilidad siempre. Cambios sin autorización, nunca. 🪄</strong></p>
