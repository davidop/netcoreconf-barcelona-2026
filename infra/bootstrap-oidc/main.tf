# Separate bootstrap Terraform root. No deployment until bootstrap_enabled=true
# AND an explicitly reviewed terraform apply is authorized.
#
# The read-only plan identity is deliberately NOT a Terraform deployment identity.
# Its Reader assignment is confined to this bootstrap resource group only.
locals {
  oidc_subject = format("repo:%s:environment:%s", var.github_repository, var.github_environment)
}

resource "azurerm_resource_group" "plan_identity" {
  count    = var.bootstrap_enabled ? 1 : 0
  name     = var.identity_resource_group_name
  location = var.location
  tags = {
    project    = "merlinops"
    purpose    = "github-oidc-readonly-plan"
    managed_by = "terraform"
  }
}

resource "azurerm_user_assigned_identity" "plan" {
  count               = var.bootstrap_enabled ? 1 : 0
  name                = var.identity_name
  location            = azurerm_resource_group.plan_identity[0].location
  resource_group_name = azurerm_resource_group.plan_identity[0].name
  tags                = azurerm_resource_group.plan_identity[0].tags
}

resource "azurerm_federated_identity_credential" "github_plan" {
  count               = var.bootstrap_enabled ? 1 : 0
  name                = "github-merlinops-plan"
  resource_group_name = azurerm_resource_group.plan_identity[0].name
  parent_id           = azurerm_user_assigned_identity.plan[0].id
  audience            = ["api://AzureADTokenExchange"]
  issuer              = "https://token.actions.githubusercontent.com"
  subject             = local.oidc_subject
}

resource "azurerm_role_assignment" "plan_reader" {
  count                = var.bootstrap_enabled ? 1 : 0
  scope                = azurerm_resource_group.plan_identity[0].id
  role_definition_name = "Reader"
  principal_id         = azurerm_user_assigned_identity.plan[0].principal_id
}
