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
