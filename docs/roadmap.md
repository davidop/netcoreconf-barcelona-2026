# Roadmap — from conference demo to reference architecture

[← Back to README](../README.md) · [Current status](status.md) · [Open Issues](https://github.com/davidop/netcoreconf-barcelona-2026/issues)

The **78 approved design decisions** describe the target architecture; delivered code is intentionally a safe, repeatable local slice. This roadmap is ordered by evidence and trust boundaries, **not** by marketing polish.

## Level 0 — public educational reference (implemented, independently tested in CI)

- [x] Slidev public interactive Replay and MerlinOps 3D visualization
- [x] .NET 10 API and real Microsoft Agent Framework fan-out over four **deterministic** Guardian executors
- [x] Local SQLite synthetic run snapshots and SSE
- [x] YAML fail-closed quality/security gates and synthetic approvals labelled as fiction
- [x] xUnit tests, fixture tests, replay sanitization tests, browser Playwright checks
- [x] No Azure credentials or resources required
- [x] Visual assets, technology icons, docs navigation and architecture gallery
- [ ] Authentic stage screenshots integrated into repository (current illustrations are labelled)
- [ ] Offline package, accessibility audit and speaker dress rehearsal completed and recorded

## Level 1 — robust local agent engineering

- [ ] [#1 Real model-backed Guardian architecture](https://github.com/davidop/netcoreconf-barcelona-2026/issues/1): replace deterministic planners with policy-restricted model-driven agents
- [ ] [#5 Durable execution and OTel](https://github.com/davidop/netcoreconf-barcelona-2026/issues/5): append-only events, checkpoints, idempotence, correlation and storage contracts
- [ ] [#6 Sanitization and human review](https://github.com/davidop/netcoreconf-barcelona-2026/issues/6): improve the privacy pipeline; keep authentic raw traces private
- [ ] [#7 Conference readiness and visuals](https://github.com/davidop/netcoreconf-barcelona-2026/issues/7): full cross-browser accessibility/offline testing and 45-minute rehearsal

## Level 2 — authenticated cloud adapters (requires separately authorized validation)

- [ ] [#2 Entra-backed LIVE and RBAC](https://github.com/davidop/netcoreconf-barcelona-2026/issues/2): issuer/audience validation, role-scoped backend access, tenant boundary
- [ ] [#3 Copilot Studio SDK compatibility](https://github.com/davidop/netcoreconf-barcelona-2026/issues/3): independently verify Microsoft 365 Agents SDK / GitHub Copilot Harness interoperability
- [ ] [#4 Foundry FinOps evidence](https://github.com/davidop/netcoreconf-barcelona-2026/issues/4): real tokens, latency, evals and actual-versus-projected costs
- [ ] [#8 Azure deployment authorization](https://github.com/davidop/netcoreconf-barcelona-2026/issues/8): explicitly approved Terraform plan, resource isolation, rollback and budget controls

**No Azure creation or modification is authorized.** A green CI workflow does not change this constraint.

## Acceptance for LIVE

A LIVE label may be used only when every claimed operation is supported by actual authenticated integration, backend policy enforcement, genuine human consent, observability and a defensible rollback plan. Synthetic metrics, approval events and replay files must never satisfy that gate.
