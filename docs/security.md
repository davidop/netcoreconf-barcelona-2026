# Security and privacy boundaries

- **Cloud:** no Azure resource may be created or modified. Terraform is inert by default, and CI does not run apply.
- **LIVE:** disabled. Do not add publicly callable model/Foundry/Teams/Azure mutation endpoints without Entra authentication, audience/issuer validation, backend RBAC, policy and human approval.
- **Public data:** versioned synthetic fixtures. Authentic events require automatic minimization, secret/PII scanning, integrity review and human approval before GitHub publication.
- **Policy:** fail closed on missing guardrails; critical-risk proposals must not continue to an approval or deployment event.
- **Human approval:** approval.simulated is solely a presentational marker and never an authorization token.
- **Secrets:** no access keys, bearer tokens, client data, private prompts or raw telemetry in this public repository.
- **Model risks:** prompt-injection mitigations, least-privilege tools, allowlists and output validation must be added before external tool use.
- **Known gaps:** Entra and OPA/Rego production controls, Managed Identity, Azure Policy and OpenTelemetry redaction/export remain planned, not implemented.

The local simulation API is not a hardened public service. Do not internet-expose it with expanded capabilities.
