# Separate bootstrap Terraform root. No deployment until bootstrap_enabled=true
# AND an explicitly reviewed terraform apply is authorized.
#
# The read-only plan identity is deliberately NOT a Terraform deployment identity.
# The identity bootstrap has no RBAC assignment by default; a separate
# administrator-approved stage can grant Reader on one explicit resource group.
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

# Separate RBAC phase: a Contributor cannot perform this action.
# Never assign access at the subscription or tenant level from this module.
resource "azurerm_role_assignment" "plan_reader" {
  count                = var.bootstrap_enabled && var.grant_reader_role ? 1 : 0
  scope                = var.reader_target_resource_group_id
  role_definition_name = "Reader"
  principal_id         = azurerm_user_assigned_identity.plan[0].principal_id

  lifecycle {
    precondition {
      condition     = length(regexall("^/subscriptions/[0-9A-Fa-f-]{36}/resourceGroups/[^/]+$", var.reader_target_resource_group_id)) == 1
      error_message = "Reader scope must be a reviewed Azure resource group, never subscription-wide."
    }
  }
}
