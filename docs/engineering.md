# Multi-agent development engineering guide

The GitHub Copilot, Claude Code and Codex agents are **developer tools**, distinct from MerlinOps and its eventual runtime Guardian agents.

## Suggested split of work
- Copilot/architect: Microsoft Agent Framework preview adapters, SDK pinning and workflow contracts.
- Codex/runtime: C# API, SQLite, idempotency, policy enforcement, tests.
- Claude Code/experience: Slidev, Mission Control, expressive 3D MerlinOps, accessible UX.
- Governance reviewer: safety, no cloud writes, data provenance, traces, CI security.

## Working agreement
1. Read AGENTS.md, docs/decisions.md and docs/security.md.
2. Implement disjoint components in parallel with shared event and policy contracts.
3. Produce observable acceptance tests, logs and CI evidence; no fabricated pass claims.
4. Do not mix untrusted agent reasoning with backend authorization.
5. Initial direct-to-main autonomous integration is approved; collaboration should use PR review.
6. Source code and technical documentation in English; stage copy in Spanish.
7. Document preview dependency versions and failure modes explicitly.

## Ready-to-implement next steps
Finish preview adapter build, real Agent Framework runtime, durable checkpoint transitions, OTel correlation, Entra-backed approval and authenticated Live adapter in a separately authorized test subscription, production-grade storage adapter, Playwright/axe checks and honest Replay archival workflow.
