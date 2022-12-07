locals {
  location = lower(replace(var.location, " ", ""))
  environment = lower(var.environments[lower(var.environment)])
}
