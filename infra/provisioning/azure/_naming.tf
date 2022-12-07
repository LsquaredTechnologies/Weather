module "names" {
  source        = "github.com/Azure/terraform-azurerm-naming"
  prefix        = [var.prefix, ]
  suffix        = [local.location, local.environment]
  unique-length = 5
}
