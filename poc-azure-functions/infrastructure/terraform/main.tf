# 1. Create a Resource Group
resource "azurerm_resource_group" "rg" {
  name     = "rg-fullstack-poc-dev"
  location = "South India"
}

# 2. Create a globally unique Storage Account
resource "random_integer" "ri" {
  min = 20000
  max = 89999
}

resource "azurerm_storage_account" "sa" {
  name                     = "stfuncpocdev${random_integer.ri.result}"
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

# 3. Create the Consumption Plan (Serverless / Free tier friendly)
resource "azurerm_service_plan" "asp" {
  name                = "asp-fullstack-poc-dev"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  os_type             = "Windows"
  sku_name            = "Y1" # Y1 is the dynamic Consumption tier
}

# 4. Create the actual Function App
resource "azurerm_windows_function_app" "func" {
  name                       = "func-fullstack-poc-dev" # EXACTLY matches your YAML variable
  resource_group_name        = azurerm_resource_group.rg.name
  location                   = azurerm_resource_group.rg.location
  service_plan_id            = azurerm_service_plan.asp.id
  storage_account_name       = azurerm_storage_account.sa.name
  storage_account_access_key = azurerm_storage_account.sa.primary_access_key

  site_config {
    application_stack {
      use_dotnet_isolated_runtime = true
      dotnet_version              = "v8.0" # Azure officially supports v8.0 for isolated workers right now
    }
  }
}