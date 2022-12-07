variable "location" {
  type    = string
  default = "West Europe"
}

variable "environment" {
  type    = string
  default = "Development"
}

variable "prefix" {
  type    = string
  default = ""
}

variable "environments" {
  type = map(any)
  default = {
    "development" = "dev"
    "staging"     = "stg"
    "production"  = "prd"
  }
}
