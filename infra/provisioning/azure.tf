module "azure" {
  source = "./azure"

  environment = var.environment
  location    = var.location
  prefix      = "weather"
}
