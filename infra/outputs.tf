output "deployment_enabled" {
  value       = var.deploy_enabled
  description = "Must remain false until a separately reviewed deployment is authorized."
}

output "foundry_mode" {
  value       = var.foundry_mode
  description = "none / existing / create; existing never adopts cloud ownership."
}

output "foundry_project_id" {
  value = var.foundry_mode == "existing" ? (
    var.existing_foundry_project_id != "" ? var.existing_foundry_project_id : null
  ) : (
    local.create_foundry ? azapi_resource.foundry_project[0].id : null
  )
  description = "ARM ID for reference, not a claim of deployed/verified inference."
}

output "foundry_existing_endpoint_reference" {
  value       = var.foundry_mode == "existing" && var.existing_foundry_project_endpoint != "" ? var.existing_foundry_project_endpoint : null
  description = "Caller-supplied reference only; Terraform has not contacted or verified Foundry."
}

output "foundry_planned_model_names" {
  value       = local.create_foundry ? sort(keys(var.foundry_model_deployments)) : []
  description = "Names configured for new provisioning, NOT a model connectivity test."
}

output "container_environment_id" {
  value = local.provision_containerapps ? azurerm_container_app_environment.demo[0].id : null
}

output "application_insights_id" {
  value = var.deploy_enabled && var.enable_observability ? azurerm_application_insights.demo[0].id : null
}
