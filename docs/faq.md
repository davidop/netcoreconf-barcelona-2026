# Frequently asked questions

[← Back to README](../README.md)

### Is this an official Microsoft sample?
No. It is an independent community conference repository using Microsoft ecosystem technologies.

### Does the public Slidev website run Azure agents?
No. GitHub Pages is static. It runs a **synthetic Replay** only. A local runtime provides SIMULATED SSE.

### Does it use Microsoft Agent Framework?
Yes. The .NET 10 backend includes a **real MAF workflow graph** with deterministic local Guardian executors. It does not call model-backed agents in Microsoft Foundry.

### Are the cost numbers real?
No. All displayed cost, latency, quality, projected savings and approval outcomes are invented **fixtures** used to explain architecture.

### Can I deploy Terraform?
Not under the current project authorization. The user explicitly prohibited creating or modifying Azure resources. The template is opt-in, not production-complete, and CI never applies it.

### Does Copilot Studio GitHub Copilot Harness work with the Agents SDK?
Compatibility is not established by this sample. It is explicitly in the roadmap for separate technical validation.

### Where is the screenshot of Mission Control?
The README contains a labelled **illustration**, not an authentic capture. Inspect the passing [Playwright E2E artifacts](https://github.com/davidop/netcoreconf-barcelona-2026/actions/workflows/e2e.yml) for captured browser evidence.

### Can I publish real traces?
Not automatically. The sanitizer creates a REVIEW_REQUIRED candidate; the human must independently examine privacy/legal suitability and approve promotion.

### Who are the speakers?
Érika Cepeda and David Oliva Paredes. The 45-minute script is in the [presenter runbook](demo-runbook.md).
