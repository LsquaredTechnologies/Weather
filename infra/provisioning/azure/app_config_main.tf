resource "azurerm_app_configuration" "appconf" {
  name = module.names.app_configuration.name_unique

  location            = var.location
  resource_group_name = azurerm_resource_group.default.name
}

#resource "azurerm_role_assignment" "app_configuration_dataowner" {
#  role_definition_name = "App Configuration Data Owner"
#
#  scope        = azurerm_app_configuration.appconf.id
#  principal_id = data.azurerm_client_config.current.object_id
#}
