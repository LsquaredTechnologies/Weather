resource "azurerm_linux_web_app" "weather" {
  name = "weather-lsquared"

  location            = var.location
  resource_group_name = azurerm_resource_group.default.name
  service_plan_id     = azurerm_service_plan.linux.id

  https_only = true

  site_config {
    http2_enabled = true
  }

  tags = {
    Environment = var.environment
  }
}
