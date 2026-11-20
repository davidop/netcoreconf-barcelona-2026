# Optional scaffolding only; CI never runs apply.
resource "azurerm_resource_group" "demo" {
  count    = var.deploy_enabled ? 1 : 0
  name     = format("rg-%s", var.name_prefix)
  location = var.location
  tags     = var.tags
}
resource "azurerm_log_analytics_workspace" "demo" {
  count               = var.deploy_enabled ? 1 : 0
  name                = format("law-%s", var.name_prefix)
  location            = azurerm_resource_group.demo[0].location
  resource_group_name = azurerm_resource_group.demo[0].name
  sku                 = "PerGB2018"
  retention_in_days   = 30
  tags                = var.tags
}
resource "azurerm_application_insights" "demo" {
  count               = var.deploy_enabled ? 1 : 0
  name                = format("appi-%s", var.name_prefix)
  location            = azurerm_resource_group.demo[0].location
  resource_group_name = azurerm_resource_group.demo[0].name
  workspace_id        = azurerm_log_analytics_workspace.demo[0].id
  application_type    = "web"
  tags                = var.tags
}
resource "azurerm_container_app_environment" "demo" {
  count                      = var.deploy_enabled ? 1 : 0
  name                       = format("cae-%s", var.name_prefix)
  location                   = azurerm_resource_group.demo[0].location
  resource_group_name        = azurerm_resource_group.demo[0].name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.demo[0].id
  tags                       = var.tags
}
resource "azurerm_container_registry" "demo" {
  count               = var.deploy_enabled ? 1 : 0
  name                = "acrmerlinopsncc26"
  location            = azurerm_resource_group.demo[0].location
  resource_group_name = azurerm_resource_group.demo[0].name
  sku                 = "Basic"
  admin_enabled       = false
  tags                = var.tags
}
