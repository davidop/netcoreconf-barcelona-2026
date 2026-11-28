output "bootstrap_enabled" {
  value       = var.bootstrap_enabled
  description = "False by default. No identity/resources are created."
}

output "github_federation_subject" {
  value       = local.oidc_subject
  description = "OIDC identity restriction, NOT a broad repository wildcard."
}

output "plan_identity_client_id" {
  value       = var.bootstrap_enabled ? azurerm_user_assigned_identity.plan[0].client_id : null
  description = "Copy privately to GitHub protected environment AZURE_CLIENT_ID after approved deployment."
  sensitive   = true
}

output "plan_identity_reader_scope" {
  value       = var.bootstrap_enabled && var.grant_reader_role ? var.reader_target_resource_group_id : null
  description = "Approved resource group only, if an RBAC administrator separately granted Reader."
}

output "rbac_grant_requested" {
  value       = var.grant_reader_role
  description = "False by default; Contributor alone cannot grant roles."
}
