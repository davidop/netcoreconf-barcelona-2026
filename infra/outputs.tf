output "deployment_enabled" {
  value       = var.deploy_enabled
  description = "Only an explicit opt-in declares resources"
}
output "container_environment_id" {
  value = var.deploy_enabled ? azurerm_container_app_environment.demo[0].id : null
}
