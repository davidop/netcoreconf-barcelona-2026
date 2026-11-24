# MerlinOps — declarative infrastructure, dry-run by default

**Terraform is the sole infrastructure-as-code authority for any Azure resources created for this demo.** Application/agent definitions live in Git too, but not all belong to the Azure Resource Manager control plane.

> Zero resources are provisioned with the checked-in defaults. CI runs `terraform fmt -check`, `init -backend=false` and `validate` only. There are **no Azure credentials**, `plan` against a real subscription or `apply` in CI.

## Why Foundry?

Foundry is **optional** to run MAF and the local stage demo. It is an **inference provider** for the intended Copilot Studio → MAF → Foundry LIVE session, providing managed model deployments, Entra identity and model usage evidence. It is not required for deterministic agents, tests, replay or policy enforcement.

The preferred path for this repo is **reuse** the existing approved Foundry project `ai-omniroute-dev`, not create a second one. Terraform can also template a *new* Foundry account/project/model deployments for a future isolated environment, but **never by default**.

## IaC settings and cloud impact

| `deploy_enabled` | `foundry_mode` | Other switches | Terraform-managed Azure writes if someone explicitly applies? |
|---|---|---|---|
| false | **none** (default) | all off | **Zero**, no subscriptions or resources queried by Terraform |
| false | existing | all off | **Zero**; references passed in as input, no import or data lookup |
| false | create | all off | **Zero**; create plan is gated |
| true | existing | all off | **Zero** from this root; existing project remains externally managed |
| true | create | all off | Creates a new resource group, Foundry account/project and only explicitly configured model deployments |
| true | none | observability enabled | Creates a resource group, Log Analytics and Application Insights |
| true | none | Container Apps hosting enabled | Creates a resource group, Log Analytics, Container Apps Environment and ACR — **not needed for conference** |

**Terraform alone cannot express all Copilot Studio agent authoring and ALM settings as Azure resources.** Keep agent source, solution manifests and connector definitions version-controlled in `copilot-studio/`; deploy them through reviewed Power Platform solutions/`pac` or the Copilot Studio CLI preview after testing its exact supported operations. Do **not** assume that creating an Azure resource creates a Copilot Studio agent.

## Option A — use existing `ai-omniroute-dev` (recommended)

1. Keep `deploy_enabled=false` and `foundry_mode="none"` for offline CI.
2. For a private, local **reference-only** configuration, set `foundry_mode="existing"`, provide existing project ARM ID and project endpoint in untracked `terraform.tfvars`. Terraform performs **no data lookups or writes** in this mode; these values are references, not verified connectivity.
3. Do not `terraform import` a pre-existing shared Foundry account/project into a new state merely to reference it; importing would transfer lifecycle ownership and requires a specific governance review.
4. The actual .NET runtime connection and Entra roles are a separate reviewed task. The endpoint is not a credential.

## Option B — fully managed new Foundry (requires approval)

Use `deploy_enabled=true` and `foundry_mode="create"` **only in an explicitly reviewed change window**. The account is `AIServices` with system-assigned identity and local API key authentication disabled; the project has a system-assigned identity. Models are specified in the `foundry_model_deployments` map and default to **none**. Validate region, model SKU/version, quota, RBAC, allowed networking, expected token costs, and any resource registration before executing `apply`.

The code is in [foundry.tf](foundry.tf), and examples in [terraform.tfvars.example](terraform.tfvars.example). It follows Microsoft's current Foundry Azure control-plane API (`Microsoft.CognitiveServices/accounts`, `accounts/projects` and `accounts/deployments`, 2025-06-01). This is a **baseline scaffold**, not a production-ready private network, identity, budget, RAI-policy or data residency setup. Those must be explicitly reviewed and added before production.

No ACR or Container Apps Environment is required for local MAF, or for a Foundry **model** call. If a future **hosted custom agent** is deployed inside Foundry, that is a **separate data-plane lifecycle**: Terraform AzAPI supports it with `azapi_data_plane_resource`; its container image/registry/RAI/network requirements must be validated independently. A .NET MAF application hosted in ACA/App Service is a different deployment option and should not be conflated with a Foundry-hosted agent.

## Local validation (no Azure provisioning)

```bash
terraform -chdir=infra fmt -check -diff -recursive
terraform -chdir=infra init -backend=false -input=false
terraform -chdir=infra validate
```

With a real subscription and RBAC, any future `plan` may read cloud state or contain sensitive values; require approval before using a real tenant. `apply`, imports and model/agent configuration changes remain **explicitly unauthorized** in this work.

## Controls before deployment

- Require a reviewed PR and explicit authorization specifying subscription, resource group, location, project/model SKU and costs.
- Migrate Terraform state to an encrypted, access-controlled remote backend rather than storing it in the repository.
- Bind runtime MAF identity to Foundry with Entra/RBAC; never commit credentials or secrets to state.
- Consider Azure Policy, budgets/alerts, private networking and audit logs as prerequisites for real usage.
- Preserve `SIMULATED` / `REPLAY` local paths without any Foundry connection.

### Official references
- [Microsoft Foundry Terraform guide](https://learn.microsoft.com/en-us/azure/foundry/how-to/create-resource-terraform)
- [Foundry hosted agent Terraform (separate data plane)](https://learn.microsoft.com/en-us/azure/foundry/agents/how-to/deploy-hosted-agent-terraform)
- [Copilot Studio ALM](https://learn.microsoft.com/en-us/microsoft-copilot-studio/guidance/alm)
- [Copilot Studio CLI preview](https://learn.microsoft.com/en-us/microsoft-365/copilot-managed-runtime/developer/agents/cli/)
