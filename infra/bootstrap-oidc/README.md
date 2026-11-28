# MerlinOps — GitHub Actions OIDC plan identity (bootstrap IaC)

This **separate Terraform root** describes a dedicated, *read-only* user-assigned managed identity (UAMI) trusted by one GitHub Actions protected environment. Its default `bootstrap_enabled=false` creates **zero resources**. It does NOT modify Copilot Studio, an existing Foundry project or model deployments.

## Why this separate identity

A human with Azure subscription access is **not the same security principal** as GitHub Actions. GitHub needs its own Entra workload identity federation and precisely scoped RBAC; **no client secret** is needed. Avoid sharing the owner's interactive credentials.

The trust is exact:
- Issuer: `https://token.actions.githubusercontent.com`
- Audience: `api://AzureADTokenExchange`
- Subject: `repo:davidop/netcoreconf-barcelona-2026:environment:merlinops-plan`

The bootstrap is deliberately split into **two independently authorized phases**:

1. **Phase A — Contributor-compatible identity bootstrap** (`bootstrap_enabled=true`, `grant_reader_role=false`): one dedicated resource group, one user-assigned managed identity and one GitHub environment-bound federated credential. **No Azure RBAC grants**.
2. **Phase B — RBAC administrator** (`bootstrap_enabled=true`, `grant_reader_role=true`): **Reader** on an explicitly specified `reader_target_resource_group_id`. The Terraform configuration rejects subscription-wide scopes. This requires `Microsoft.Authorization/roleAssignments/write`, which **Contributor does not include**.

Both phases still require a separately reviewed plan and explicit permission to `apply`. The present checked-in configuration disables both phases; no resources have been created.

**Limitation:** Reader on that group does **not** grant inspection of a separate resource group containing an existing Foundry project. Such access requires a reviewed, separately scoped Reader role assignment. Actual model inference needs independently approved data-plane permissions. For Terraform deployments, use a **separate**, more privileged identity only after an explicit authorization and protected approval path. Do not upgrade this read-only plan identity to Contributor.

## Required preparatory checks

- Confirm which existing Azure resource group, if any, will hold the demo resources. The bootstrap template would create a **new** dedicated identity RG only after approval.
- **Confirmed current human role: Contributor.** This is generally sufficient to create the identity and federated credential in Phase A, subject to Azure Policy and resource restrictions. It is **not sufficient for Phase B**. Ask an existing **Owner, User Access Administrator or Role Based Access Control Administrator** (scoped to the target resource group) to carry out the reviewed Reader assignment. Do not request subscription Owner if RG-scoped RBAC administration suffices.
- Confirm the selected subscription and tenant while logged in. Neither identifier is checked into the repository.
- In GitHub, create a protected environment named **`merlinops-plan`** with required reviewer(s), restricted to main, before initiating the workflow.
- Phase A creates a principal without Azure Reader on the target Foundry RG. Do not mistake successful OIDC token exchange for access to Foundry. Grant only the necessary target RG Reader in Phase B; model inference needs separate data-plane permissions later.
- After bootstrap is explicitly approved and applied, set the following **environment secrets** (not repository files): `AZURE_CLIENT_ID` (UAMI client ID), `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID`. These are identifiers, not credentials; keep them out of public logs.
- The [manual preflight workflow](../../.github/workflows/azure-oidc-preflight.yml) must first exist on the repository default branch, after a reviewed PR. It authenticates through OIDC and runs a **zero-change** Terraform plan; it has **no apply step**.

## Offline validation (safe now)

```bash
terraform -chdir=infra/bootstrap-oidc fmt -check -diff -recursive
terraform -chdir=infra/bootstrap-oidc init -backend=false -input=false
terraform -chdir=infra/bootstrap-oidc validate
```

## Explicitly unauthorized future steps

To prepare an Azure plan for review, the authorized operator would first place the subscription/tenant IDs in a **private, ignored tfvars file**; verify identity, cost impact and RBAC; arrange protected state storage; and only then, with specific approval, set `bootstrap_enabled=true` and `grant_reader_role=false` for Phase A. A separate privileged RBAC reviewer must approve Phase B by selecting `grant_reader_role=true` and `reader_target_resource_group_id` for the *actual* Foundry or demo group. Neither phase is approved for deployment yet.

**No one has authorized this apply or identity creation yet.** The current repository only contains a template. A Terraform plan with deploy disabled cannot establish whether the user already has all RBAC permissions; the Azure role assignments must be checked separately.

### Sources

- [Microsoft Learn: GitHub Actions OIDC authentication](https://learn.microsoft.com/en-us/azure/developer/github/connect-from-azure-openid-connect)
- [Microsoft Learn: create federated credentials for a user-assigned managed identity](https://learn.microsoft.com/en-us/entra/workload-id/workload-identity-federation-create-trust-user-assigned-managed-identity)
- [Microsoft Learn: secure Terraform OIDC CI/CD sample](https://learn.microsoft.com/en-us/samples/azure-samples/github-terraform-oidc-ci-cd/github-terraform-oidc-ci-cd/)
