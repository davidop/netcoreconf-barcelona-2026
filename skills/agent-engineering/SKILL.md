---
name: agent-engineering
description: Implement .NET 10 Microsoft Agent Framework workflow adapters, contracts and durable state checks without cloud writes.
---
# Agent Engineering
Read AGENTS.md and docs/decisions.md. Pin preview SDKs. Write unit tests for every event/state transition. Dynamic planning and parallel delegation must not bypass deterministic policies or human authorization. Use idempotent operations, checkpoints, replay correlation and inspectable outcomes. Cloud adapters stay inert until user explicitly authorizes Azure operations.
