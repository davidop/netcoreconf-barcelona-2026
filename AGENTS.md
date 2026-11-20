# Engineering instructions for Copilot, Claude Code and Codex

Read docs/decisions.md (78 choices), docs/architecture.md and docs/security.md first.

**Never create or modify Azure resources. Never run terraform apply.** Explicit user approval is required later. Never commit real user data, credentials, private prompts, access tokens or raw telemetry.

- Runtime target: .NET 10, Microsoft Agent Framework preview pinned and independently compile-tested.
- Presentation: Slidev + Vue, MerlinOps wizard 3D, Mission Control modes labelled LIVE/REPLAY/SIMULATED, accessible static fallback.
- Initial runnable code is SYNTHETIC, not proof of authentic Foundry or Copilot Studio interoperability.
- All production-sensitive actions require server-side gates, Entra auth, RBAC and verified human approval. Simulated approval is not permission.
- New features need automated tests, CI and an evidence-linked status update.
- Code comments/docs English; presenter script/copy Spanish.
- Agent specialization details: skills/agent-engineering/SKILL.md, skills/governance-review/SKILL.md and skills/presenter-ux/SKILL.md.
