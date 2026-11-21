# Quickstart — safe local conference demo

[← Back to README](../README.md) · [Presenter runbook](demo-runbook.md) · [Architecture](architecture.md)

> **Zero Azure credentials. Zero cloud resources.** Everything in this guide is either a local deterministic Microsoft Agent Framework workflow or a synthetic Replay fixture.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js 22+](https://nodejs.org/) with npm
- Git
- Optional: a recent desktop browser with WebGL for procedural MerlinOps 3D. A static SVG fallback is provided for reduced-motion users and unsupported environments.

No Docker, Azure subscription, Entra tenant, Copilot Studio environment, Foundry deployment, or Terraform is required for the local demo.

## 1. Get the sample

~~~bash
git clone https://github.com/davidop/netcoreconf-barcelona-2026.git
cd netcoreconf-barcelona-2026
dotnet --version
node --version
~~~

## 2. Run the local API

~~~bash
dotnet run --project src/MerlinOps.Api
~~~

The API listens at **http://localhost:5080** by default. Health returns SIMULATED and cloudWritesEnabled=false.

~~~bash
curl http://localhost:5080/health
curl http://localhost:5080/api/scenarios
~~~

## 3. Start the interactive presentation

In another terminal:

~~~bash
cd slides
npm install
npm run dev
~~~

Open the address Slidev prints (normally **http://localhost:3030**), then navigate to **slide 6: Mission Control**.

- **REPLAY**: click Reproducir; no backend required.
- **SIMULATED SSE**: with API running on localhost, choose the button to run the real Microsoft Agent Framework fan-out with deterministic Guardian results.
- **LIVE**: unavailable by design. The publicly hosted GitHub Pages site cannot call the local API.

## 4. Run the scenario API directly

~~~bash
curl -X POST http://localhost:5080/api/runs/maf-simulate \
  -H "Content-Type: application/json" \
  -d '{"scenario":"finops-success"}'
~~~

Replace the scenario identifier with **policy-block** or **quality-rollback**. The response contains a run ID, mode SIMULATED and ordered events. To replay it, call **GET /api/runs/{id}/events** or **GET /api/runs/{id}/stream** (Server-Sent Events).

On Windows PowerShell, use **Invoke-RestMethod**:

~~~powershell
Invoke-RestMethod -Uri "http://localhost:5080/api/runs/maf-simulate" -Method Post -ContentType "application/json" -Body '{"scenario":"policy-block"}'
~~~

## 5. Verify safety and behavior

~~~bash
dotnet test tests/MerlinOps.Tests/MerlinOps.Tests.csproj
python3 -m unittest discover -s tools/tests -v
cd slides
npm test
npm run build
~~~

Browser E2E (requires Playwright Chromium):

~~~bash
cd slides
npx playwright install chromium
npx playwright test
~~~

The automated CI jobs are [MerlinOps CI](https://github.com/davidop/netcoreconf-barcelona-2026/actions/workflows/ci.yml) and [Slidev Browser E2E](https://github.com/davidop/netcoreconf-barcelona-2026/actions/workflows/e2e.yml).

## Troubleshooting

| Symptom | Check |
|---|---|
| SSE button disabled on public Pages | Expected: only localhost can call the local API |
| SSE fails locally | API must be running on port 5080; localhost CORS is limited to the Slidev dev origin |
| Blank WebGL visual | Enable hardware acceleration or rely on static fallback / prefers-reduced-motion |
| SDK restore or build fails | Confirm .NET 10 SDK and resolved NuGet feeds; inspect CI for pinned package compatibility |
| Can't install Node modules | Use supported Node LTS and npm; rerun npm install, not npm ci without a committed lock file |

## Security boundary

This developer API is **not a hardened public service**. Keep it bound to local development and never expose real tenant credentials or add cloud write actions without implementing Entra authentication, RBAC, verified approval and separately authorizing Azure provisioning.
