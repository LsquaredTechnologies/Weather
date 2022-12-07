resource "azurerm_linux_web_app" "weather" {
  name = "weather-lsquared"

  location            = var.location
  resource_group_name = azurerm_resource_group.default.name
  service_plan_id     = azurerm_service_plan.linux.id

  https_only = true

  site_config {
    http2_enabled = true
  }

  app_settings = {
    "ASPNETCORE_ENVIRONMENT"     = var.environment
    "APPCONFIG_CONNECTIONSTRING" = azurerm_app_configuration.appconf.primary_read_key.0.connection_string
  }

  tags = {
    Environment = var.environment
  }
}
