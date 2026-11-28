variable "bootstrap_enabled" {
  description = "False by default; creating a GitHub read-only federation needs an explicit, reviewed terraform apply."
  type        = bool
  default     = false
}

variable "subscription_id" {
  description = "Azure subscription ID, supplied through private tfvars or TF_VAR_subscription_id."
  type        = string
  default     = ""
}

variable "tenant_id" {
  description = "Microsoft Entra tenant ID, supplied privately. Never hardcode in public Git."
  type        = string
  default     = ""
}

variable "location" {
  description = "Identity resource group location. Confirm region before any cloud creation."
  type        = string
  default     = "swedencentral"
}

variable "identity_resource_group_name" {
  description = "Dedicated resource group, scoped to only the GitHub PLAN identity."
  type        = string
  default     = "rg-merlinops-github-plan"
}

variable "identity_name" {
  description = "User-assigned managed identity for GitHub OIDC plan/review only."
  type        = string
  default     = "id-merlinops-github-plan"
}

variable "github_repository" {
  type        = string
  description = "Exact GitHub repository permitted to request a federated token."
  default     = "davidop/netcoreconf-barcelona-2026"
}

variable "github_environment" {
  type        = string
  description = "GitHub protected environment name; matches workflow environment exactly."
  default     = "merlinops-plan"
}
