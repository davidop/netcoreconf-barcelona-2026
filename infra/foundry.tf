# This configuration NEVER creates anything unless both deploy_enabled=true
# AND foundry_mode="create" are explicitly set in a reviewed plan.
#
# Reusing an existing Foundry project is the preferred conference setup:
# foundry_mode="existing" reads no resources, changes no existing resource
# and stores only a caller-supplied ID/endpoint in Terraform state.

locals {
  create_foundry = var.deploy_enabled && var.foundry_mode == "create"
}

resource "azapi_resource" "foundry_account" {
  count                     = local.create_foundry ? 1 : 0
  type                      = "Microsoft.CognitiveServices/accounts@2025-06-01"
  name                      = var.foundry_account_name
  parent_id                 = azurerm_resource_group.demo[0].id
  location                  = azurerm_resource_group.demo[0].location
  schema_validation_enabled = false
  tags                      = var.tags

  body = {
    kind = "AIServices"
    sku = {
      name = "S0"
    }
    identity = {
      type = "SystemAssigned"
    }
    properties = {
      customSubDomainName    = var.foundry_account_name
      allowProjectManagement = true
      disableLocalAuth       = true
    }
  }

  lifecycle {
    precondition {
      condition     = length(var.foundry_account_name) >= 2 && length(var.foundry_account_name) <= 64
      error_message = "A unique Foundry account name must be supplied and reviewed before provisioning."
    }
  }
}

resource "azapi_resource" "foundry_project" {
  count                     = local.create_foundry ? 1 : 0
  type                      = "Microsoft.CognitiveServices/accounts/projects@2025-06-01"
  name                      = var.foundry_project_name
  parent_id                 = azapi_resource.foundry_account[0].id
  location                  = azurerm_resource_group.demo[0].location
  schema_validation_enabled = false
  tags                      = var.tags

  body = {
    sku = {
      name = "S0"
    }
    identity = {
      type = "SystemAssigned"
    }
    properties = {
      displayName = "MerlinOps Conference Demo"
      description = "Optional Terraform-managed development Foundry project"
    }
  }
}

# Models are never selected automatically. Region, quota, cost, terms,
# model name/version, deployment SKU and capacity require a reviewed tfvars.
# No default model deployments => no inference usage incurred by IaC.
resource "azapi_resource" "foundry_deployments" {
  for_each  = local.create_foundry ? var.foundry_model_deployments : {}
  type      = "Microsoft.CognitiveServices/accounts/deployments@2025-06-01"
  name      = each.key
  parent_id = azapi_resource.foundry_account[0].id

  body = {
    sku = {
      name     = each.value.sku_name
      capacity = each.value.capacity
    }
    properties = {
      model = {
        format  = each.value.model_format
        name    = each.value.model_name
        version = each.value.model_version
      }
    }
  }
}
