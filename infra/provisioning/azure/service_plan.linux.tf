resource "azurerm_service_plan" "linux" {
  name = module.names.app_service_plan.name_unique

  location            = var.location
  resource_group_name = azurerm_resource_group.default.name

  os_type  = "Linux"
  sku_name = "B1"

  tags = {
    Environment = var.environment
  }
}
