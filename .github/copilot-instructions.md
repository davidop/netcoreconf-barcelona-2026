# Copilot repository instructions

- Always read AGENTS.md, docs/decisions.md and docs/security.md.
- Prefer C# .NET 10, typed events, SQLite local, fail-closed YAML policies and versioned SDK previews.
- No Azure creation/modification, no apply workflows or LIVE write operations.
- OpenTelemetry trace IDs alone are not evidence of a real exporter.
- The official M365 Agents SDK integration with Copilot Studio GitHub Copilot Harness is not guaranteed: isolate it behind a boundary and require verified compatibility.
- Preserve run provenance and test DENY/rollback ordering and safe approval semantics.
- Presenters speak Spanish; developer documentation/comments are English.
