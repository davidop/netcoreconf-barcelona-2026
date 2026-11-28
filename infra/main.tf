# Single opt-in root deployment. Default configuration creates ZERO resources.
# Foundry is optional, MAF runs locally without infrastructure, and ACA/ACR
# are legacy hosting options not needed for this conference demo.

locals {
  provision_resource_group = var.deploy_enabled && (
    var.foundry_mode == "create" || var.enable_observability || var.enable_containerapps_hosting
  )
  provision_log_analytics = var.deploy_enabled && (
    var.enable_observability || var.enable_containerapps_hosting
  )
  provision_containerapps = var.deploy_enabled && var.enable_containerapps_hosting
}

resource "azurerm_resource_group" "demo" {
  count    = local.provision_resource_group ? 1 : 0
  name     = format("rg-%s", var.name_prefix)
  location = var.location
  tags     = var.tags
}

resource "azurerm_log_analytics_workspace" "demo" {
  count               = local.provision_log_analytics ? 1 : 0
  name                = format("law-%s", var.name_prefix)
  location            = azurerm_resource_group.demo[0].location
  resource_group_name = azurerm_resource_group.demo[0].name
  sku                 = "PerGB2018"
  retention_in_days   = 30
  tags                = var.tags
}

resource "azurerm_application_insights" "demo" {
  count               = var.deploy_enabled && var.enable_observability ? 1 : 0
  name                = format("appi-%s", var.name_prefix)
  location            = azurerm_resource_group.demo[0].location
  resource_group_name = azurerm_resource_group.demo[0].name
  workspace_id        = azurerm_log_analytics_workspace.demo[0].id
  application_type    = "web"
  tags                = var.tags
}

# Explicitly optional legacy hosting scaffolding.
# It is NOT a prerequisite to Microsoft Agent Framework or Foundry.
resource "azurerm_container_app_environment" "demo" {
  count                      = local.provision_containerapps ? 1 : 0
  name                       = format("cae-%s", var.name_prefix)
  location                   = azurerm_resource_group.demo[0].location
  resource_group_name        = azurerm_resource_group.demo[0].name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.demo[0].id
  tags                       = var.tags
}

resource "azurerm_container_registry" "demo" {
  count               = local.provision_containerapps ? 1 : 0
  name                = var.container_registry_name
  location            = azurerm_resource_group.demo[0].location
  resource_group_name = azurerm_resource_group.demo[0].name
  sku                 = "Basic"
  admin_enabled       = false
  tags                = var.tags
}
