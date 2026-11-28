# MerlinOps — GitHub Actions OIDC plan identity (bootstrap IaC)

This **separate Terraform root** describes a dedicated, *read-only* user-assigned managed identity (UAMI) trusted by one GitHub Actions protected environment. Its default `bootstrap_enabled=false` creates **zero resources**. It does NOT modify Copilot Studio, an existing Foundry project or model deployments.

## Why this separate identity

A human with Azure subscription access is **not the same security principal** as GitHub Actions. GitHub needs its own Entra workload identity federation and precisely scoped RBAC; **no client secret** is needed. Avoid sharing the owner's interactive credentials.

The trust is exact:
- Issuer: `https://token.actions.githubusercontent.com`
- Audience: `api://AzureADTokenExchange`
- Subject: `repo:davidop/netcoreconf-barcelona-2026:environment:merlinops-plan`

The bootstrap resources, **only after an approved apply**, would be:
1. One dedicated resource group for the plan identity.
2. One user-assigned managed identity.
3. One federated identity credential bound to the exact repository and protected GitHub environment.
4. A **Reader** role only on the newly created identity resource group, **not** subscription-wide Contributor/Owner.

**Limitation:** Reader on that group does **not** grant inspection of a separate resource group containing an existing Foundry project. Such access requires a reviewed, separately scoped Reader role assignment. Actual model inference needs independently approved data-plane permissions. For Terraform deployments, use a **separate**, more privileged identity only after an explicit authorization and protected approval path. Do not upgrade this read-only plan identity to Contributor.

## Required preparatory checks

- Confirm which existing Azure resource group, if any, will hold the demo resources. The bootstrap template would create a **new** dedicated identity RG only after approval.
- Confirm that the human bootstrapping can create resource groups/identities and can assign the scoped Reader role (for example via User Access Administrator or Owner). Subscription Reader alone is insufficient.
- Confirm the selected subscription and tenant while logged in. Neither identifier is checked into the repository.
- In GitHub, create a protected environment named **`merlinops-plan`** with required reviewer(s), restricted to main, before initiating the workflow.
- After bootstrap is explicitly approved and applied, set the following **environment secrets** (not repository files): `AZURE_CLIENT_ID` (UAMI client ID), `AZURE_TENANT_ID`, `AZURE_SUBSCRIPTION_ID`. These are identifiers, not credentials; keep them out of public logs.
- The [manual preflight workflow](../../.github/workflows/azure-oidc-preflight.yml) must first exist on the repository default branch, after a reviewed PR. It authenticates through OIDC and runs a **zero-change** Terraform plan; it has **no apply step**.

## Offline validation (safe now)

```bash
terraform -chdir=infra/bootstrap-oidc fmt -check -diff -recursive
terraform -chdir=infra/bootstrap-oidc init -backend=false -input=false
terraform -chdir=infra/bootstrap-oidc validate
```

## Explicitly unauthorized future steps

To prepare an Azure plan for review, the authorized operator would first place the subscription/tenant IDs in a **private, ignored tfvars file**; verify identity, cost impact and RBAC; arrange protected state storage; and only then, with specific approval, set `bootstrap_enabled=true` to run a reviewed `terraform plan` and `terraform apply`.

**No one has authorized this apply or identity creation yet.** The current repository only contains a template. A Terraform plan with deploy disabled cannot establish whether the user already has all RBAC permissions; the Azure role assignments must be checked separately.

### Sources

- [Microsoft Learn: GitHub Actions OIDC authentication](https://learn.microsoft.com/en-us/azure/developer/github/connect-from-azure-openid-connect)
- [Microsoft Learn: create federated credentials for a user-assigned managed identity](https://learn.microsoft.com/en-us/entra/workload-id/workload-identity-federation-create-trust-user-assigned-managed-identity)
- [Microsoft Learn: secure Terraform OIDC CI/CD sample](https://learn.microsoft.com/en-us/samples/azure-samples/github-terraform-oidc-ci-cd/github-terraform-oidc-ci-cd/)
