resource "azurerm_resource_group" "default" {
  name = module.names.resource_group.name_unique

  location = var.location

  tags = {
    Environment = var.environment
  }
}
