# ADR-0001: Simulation-first, no cloud operations

**Accepted 2026-11-21.** Conference value depends on deterministic demonstrations that work without Azure credentials. The user has prohibited Azure creation or modification until explicit further authorization. First ship a synthetic executable vertical slice with truthful provenance. Keep LIVE disabled, all external mutations absent, and Terraform opt-in but never applied. Actual Foundry cost/quality and rollback claims require future test evidence.
