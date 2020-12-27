provider "azurerm" {
  version = "=2.35.0"
  features {}
}

resource "azurerm_resource_group" "au-get-session" {
  name     = "au-get-session-func-rg"
  location = "westus2"
}

resource "random_id" "server" {
  keepers = {
    # Generate a new id each time we switch to a new Azure Resource Group
    rg_id = azurerm_resource_group.au-get-session.name
  }

  byte_length = 8
}

resource "azurerm_storage_account" "au-get-session" {
  name                     = "randomidserverhex"
  resource_group_name      = azurerm_resource_group.au-get-session.name
  location                 = azurerm_resource_group.au-get-session.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_app_service_plan" "au-get-session" {
  name                = "azure-functions-au-service-plan"
  location            = azurerm_resource_group.au-get-session.location
  resource_group_name = azurerm_resource_group.au-get-session.name
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_application_insights" "au-get-session" {
  name                = "au-get-session-insights"
  location            = azurerm_resource_group.au-get-session.location
  resource_group_name = azurerm_resource_group.au-get-session.name
  application_type    = "web"
}

resource "azurerm_function_app" "au-get-session" {
  name                      = "au-get-session-func"
  location                  = azurerm_resource_group.au-get-session.location
  resource_group_name       = azurerm_resource_group.au-get-session.name
  app_service_plan_id       = azurerm_app_service_plan.au-get-session.id
  storage_connection_string = azurerm_storage_account.au-get-session.primary_connection_string
  version                   = "~3"
  
  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY = azurerm_application_insights.au-get-session.instrumentation_key,
	FUNCTIONS_EXTENSION_VERSION: "~3",
	FUNCTIONS_EXTENSION_RUNTIME: "dotnet"
  }
  
  source_control {
	branch             = "master"
	manual_integration = true
	repo_url           = "https://github.com/Kazumz/Among-Us-Tracker-Get-Session"
	rollback_enabled   = false
	use_mercurial      = false
  }
}