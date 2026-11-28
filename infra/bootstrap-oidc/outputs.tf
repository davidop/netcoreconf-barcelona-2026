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
  value       = var.bootstrap_enabled ? azurerm_resource_group.plan_identity[0].id : null
  description = "Limited to the dedicated identity group. No subscription-wide roles."
}
