# Copilot Studio agent lifecycle — source-controlled, no cloud changes

**Goal:** MerlinOps in the owner's existing development Copilot Studio environment delegates a task to the protected .NET 10 Microsoft Agent Framework service over **A2A** if the environment supports the required authentication. Otherwise use a review-approved OAuth2-protected REST tool based on [the OpenAPI design contract](../contracts/copilot-delegation.openapi.yaml).

This directory describes **app-level declarative configuration**. It is NOT an Azure Terraform provider resource: Power Platform Copilot Studio agent topics, instructions, knowledge, actions, channel bindings and solution components are not created by the AzureRM/AzAPI control-plane resources in `../infra/`.

## Deployment ownership — no drift

- `../infra/`: Terraform for infrastructure: Foundry accounts/projects/model deployments, optional monitoring and optional infrastructure host. Existing `ai-omniroute-dev` is reused in read-only reference mode unless owner authorizes new resources or ownership transfer.
- `agent.spec.yaml`: reviewed **design contract**, not an importable Copilot Studio agent definition. Do not describe this as a runnable agent.
- **Native source of truth after environment capability validation:** export a Copilot Studio solution with Power Platform CLI (`pac`) or use the preview `msagent` CLI to obtain actual supported agent definition files. Commit exported definitions after sanitization; deploy only after explicit approval.
- Never commit tenant IDs, environment names, connection references, tokens or user/organization-specific endpoints to the public repository. Keep secrets in an approved vault or secure deployment variables.

## Controlled deployment process

1. Verify the selected environment's capabilities, licensing, connector support, and applicable A2A/REST authentication model without modifying it.
2. Confirm the MAF HTTPS hosting endpoint and authenticated service-to-service trust. **Localhost is not a production A2A endpoint**, and no public unauthenticated staging URL is acceptable.
3. Author the native Copilot Studio agent/solution definition in the existing dev environment as a separately approved change.
4. Export the supported agent/solution representation, review the Git diff, test a safe synthetic incident, validate correlation, and record actual usage with cost boundaries.
5. Promote through managed solutions or approved CLI deployment. Some Copilot Studio settings require specific post-deployment configuration and are not solution-aware.
6. Keep the projector UI read-only; Érika's approval is a distinct authenticated backend operation, never a Copilot text instruction.

## References
- [Copilot Studio ALM with solutions, GitHub Actions and Power Platform pipelines](https://learn.microsoft.com/en-us/microsoft-copilot-studio/guidance/alm)
- [Copilot Studio CLI preview, source-controlled agent definitions](https://learn.microsoft.com/en-us/microsoft-365/copilot-managed-runtime/developer/agents/cli/)
- [Official A2A external agents in Copilot Studio](https://learn.microsoft.com/en-us/microsoft-copilot-studio/agents-experience/add-agent-to-agent)

**Status: NOT CONNECTED.** No agent has been created, deployed, changed or published.
