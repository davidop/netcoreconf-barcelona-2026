# Read-only Azure Container Apps evidence — stage 2 preparation

[← Érika's first slice](erika-first-slice.md) · [Safety model](security.md)

This opt-in local script prepares **real Azure Container Apps configuration and basic Azure Monitor metric evidence** without deploying, modifying or deleting resources. It does **not** integrate Azure directly with the public Slidev site and does not issue publication approvals.

## Before running

- Have **PowerShell 7+**, current **Azure CLI** and the `containerapp` extension installed. The script refuses automatic extension installation.
- Use your own identity: `az login`. Never paste tokens, passwords or credentials into ChatGPT or GitHub Issues. Confirm you have read rights to the selected Container App and metric series.
- Obtain the **full, existing** resource ID, e.g. `/subscriptions/<subscription-guid>/resourceGroups/<resource-group>/providers/Microsoft.App/containerApps/<app-name>`.
- Do not use a production customer resource without the customer's permission for data collection, retention and conference demonstration.
- The probe is **not** available through public GitHub Pages or an unauthenticated .NET API endpoint.

## Validation without Azure calls

```powershell
./tools/azure-containerapps-readonly.ps1 `
  -ResourceId "/subscriptions/00000000-0000-0000-0000-000000000000/resourceGroups/demo/providers/Microsoft.App/containerApps/example" `
  -ValidateOnly
```

## Collect evidence from an existing authorized app

```powershell
./tools/azure-containerapps-readonly.ps1 `
  -ResourceId "/subscriptions/<subscription-guid>/resourceGroups/<resource-group>/providers/Microsoft.App/containerApps/<app-name>"
```

The script allows **only** these Azure CLI operations:
- `az containerapp show --ids …` with a restricted JMESPath projection; **never** uses `--show-secrets`;
- `az monitor metrics list --resource …` for `Requests`, `Replicas`, `UsageNanoCores`, `WorkingSetBytes`, for the previous 24 hours.

The generated `artifacts/private/containerapps-evidence.json` is intentionally ignored by Git. It stores an SHA-256 hash of the resource ID, not the subscription ID itself; it includes a subset of configuration and numeric metrics. **Treat it as private until reviewed** because even sanitized telemetry may contain confidential operational information. Never upload or commit it without explicit review.

### What the script does *not* claim

Azure Container Apps provides CPU, memory, replicas, requests and an average response-time Preview metric, but the basic metrics alone do **not** establish a reliable **p95 latency SLO**. The probe records `p95LatencyMs: null` and `INCOMPLETE_EVIDENCE`, requiring further verified telemetry (for example, an authorized Application Insights or Log Analytics query with a defined request/operation scope and time window) before a true optimization PASS.

A precise per-app actual billing figure is not assumed from basic metrics. The probe records `actualAppCostUsd: null` until a properly scoped, attributable billing method is validated. Missing permissions/series become `UNAVAILABLE`, never invented 0 values.

The simulation in [Érika's first slice](erika-first-slice.md) remains separate and reproducible. It does not silently consume real evidence, and its 9→4 local tool-call comparison is not a claim about live Azure savings.

**Only read-only operations are authorized in this preparation step. Azure resource creation, mutation, deployment and rollback remain prohibited.**

## Microsoft Learn references

- [Azure Container Apps CLI (show)](https://learn.microsoft.com/cli/azure/containerapp#az-containerapp-show)
- [Azure Monitor CLI metrics (list)](https://learn.microsoft.com/cli/azure/monitor/metrics#az-monitor-metrics-list)
- [Available Azure Container Apps metrics](https://learn.microsoft.com/azure/container-apps/metrics)
