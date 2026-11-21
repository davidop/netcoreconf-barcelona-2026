# Security policy

## Scope

This is a **community presentation sample**, not a production-ready service. It uses local deterministic MAF workflows and synthetic Replay fixtures. Production Entra authentication, backend RBAC, protected approvals, private telemetry and Azure integrations are **not implemented**. Never expose the sample as a privileged public API.

## Reporting a vulnerability

Please **do not open a public Issue** containing credentials, private data or exploit details. Use GitHub's private vulnerability reporting / Security Advisory feature if this repository exposes that control. Otherwise contact the maintainers privately through a channel already trusted by you; do not paste sensitive material into discussion boards.

## Current hard constraints

- No Azure resource creation or modification until a **new explicit user authorization**.
- No GitHub Actions terraform apply or cloud identity credentials.
- Critical-risk proposals fail closed; UI-only checks cannot authorize side effects.
- Public Replay data must be synthetic or explicitly approved sanitized evidence; the provided sanitizer creates unpublished REVIEW_REQUIRED candidates.
- Treat model output as untrusted, and do not promote it directly to an execution or privilege decision.

See [docs/security.md](docs/security.md) for the complete boundary and missing production controls.
