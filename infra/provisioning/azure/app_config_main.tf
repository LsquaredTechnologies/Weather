resource "azurerm_app_configuration" "appconf" {
  name = module.names.app_configuration.name_unique

  location            = var.location
  resource_group_name = azurerm_resource_group.default.name
}
