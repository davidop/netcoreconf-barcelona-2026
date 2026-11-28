variable "subscription_id" {
  type        = string
  description = "Only for separately authorized Azure usage"
  default     = ""
}

variable "deploy_enabled" {
  type        = bool
  description = "Always false until explicit subsequent authorization"
  default     = false
}

variable "location" {
  type    = string
  default = "swedencentral"
}

variable "name_prefix" {
  type    = string
  default = "merlinops-ncc26"
}

variable "tags" {
  type = map(string)
  default = {
    project     = "netcoreconf-barcelona-2026"
    environment = "demo"
    managed_by  = "terraform"
  }
}

# Existing project (recommended): reference only. Terraform will NOT adopt or
# modify an existing Foundry resource unless it is separately imported.
variable "foundry_mode" {
  description = "none (default), existing (reuse without managing), or create (only with deploy_enabled)."
  type        = string
  default     = "none"

  validation {
    condition     = contains(["none", "existing", "create"], var.foundry_mode)
    error_message = "foundry_mode must be none, existing, or create."
  }
}

variable "existing_foundry_project_id" {
  description = "Optional ARM project ID for existing mode; never commit customer-specific values."
  type        = string
  default     = ""
}

variable "existing_foundry_project_endpoint" {
  description = "Optional existing Foundry project endpoint; no request is issued by Terraform."
  type        = string
  default     = ""
}

variable "foundry_account_name" {
  type        = string
  description = "Unique account name; only used when creating a NEW Foundry account after approval."
  default     = ""
}

variable "foundry_project_name" {
  type        = string
  description = "Project name for explicitly authorized create mode."
  default     = "merlinops-dev"
}

variable "foundry_model_deployments" {
  description = "Reviewed, region/sku/quota-supported model deployments. Empty by default."
  type = map(object({
    model_format  = string
    model_name    = string
    model_version = string
    sku_name      = string
    capacity      = number
  }))
  default = {}

  validation {
    condition     = alltrue([for d in values(var.foundry_model_deployments) : d.capacity > 0])
    error_message = "Foundry model deployment capacity must be positive."
  }
}

variable "enable_observability" {
  type        = bool
  description = "Optional Azure Monitor resources. Off for local rehearsal."
  default     = false
}

variable "enable_containerapps_hosting" {
  type        = bool
  description = "Optional legacy ACA Environment and ACR; not needed for MerlinOps demo."
  default     = false
}

variable "container_registry_name" {
  type        = string
  description = "Globally unique ACR name; used only when optional hosting is enabled."
  default     = ""
}
