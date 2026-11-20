# Terraform is OPTIONAL / DO NOT APPLY

Only future opt-in scaffolding for resource group, Log Analytics, Application Insights, Container Apps Environment and ACR. Cloud writes are NOT authorized. CI only formats/validates the config and has no Azure credentials or apply step.

This is NOT a complete production deployment; managed identity, secure ingress, network, budgets, Azure storage, RBAC and release/rollback workflows are pending. deploy_enabled is false by default.
