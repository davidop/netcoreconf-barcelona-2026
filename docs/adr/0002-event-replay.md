# ADR-0002: Semantic events + correlated future OTel

**Accepted 2026-11-21.** The event contract includes ordered event sequence, agent, action kind, message, Quality Gate and run-scoped trace correlation. Currently SQLite persists a run snapshot for synthetic execution; this is **not yet a production append-only event store**. Future real event sourcing and OpenTelemetry exports must be correlated, immutable, redactable and retained under private access control. GitHub may contain only approved, sanitized replay datasets.
