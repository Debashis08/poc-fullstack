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
  # name                     = "stfuncpocdev${random_integer.ri.result}"
  name                     = "stfuncpocdev20230821"
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
  sku_name            = "Y1" 
}

# 4. Create the actual Function App
resource "azurerm_windows_function_app" "func" {
  # name                       = "func-poc-dev-${random_integer.ri.result}"
  name                       = "func-poc-dev-20230821"
  resource_group_name        = azurerm_resource_group.rg.name
  location                   = azurerm_resource_group.rg.location
  service_plan_id            = azurerm_service_plan.asp.id
  storage_account_name       = azurerm_storage_account.sa.name
  storage_account_access_key = azurerm_storage_account.sa.primary_access_key

  site_config {
    application_stack {
      use_dotnet_isolated_runtime = true
      dotnet_version              = "v10.0" 
    }
  }

  app_settings = {
    # Notice: WEBSITE_RUN_FROM_PACKAGE is completely removed from here
    "FUNCTIONS_WORKER_RUNTIME" = "dotnet-isolated"
    "AzureWebJobsStorage"      = azurerm_storage_account.sa.primary_connection_string

    # Global Muzzle: Hide standard noisy host logs
    "AzureFunctionsJobHost__logging__logLevel__default"                          = "Warning"
    "AzureFunctionsJobHost__logging__logLevel__Host.Results"                     = "Warning"
    "AzureFunctionsJobHost__logging__logLevel__Function"                         = "Warning"
    "AzureFunctionsJobHost__logging__logLevel__Microsoft"                        = "Warning"
    "AzureFunctionsJobHost__logging__logLevel__Microsoft.Azure.Functions.Worker" = "Warning"

    # VIP Pass: Allow your custom namespace to log Information
    "AzureFunctionsJobHost__logging__logLevel__Ecommerce.Functions"              = "Information"
  }

  # Tell Terraform to ignore changes made by Azure DevOps
  lifecycle {
    ignore_changes = [
      app_settings["WEBSITE_RUN_FROM_PACKAGE"]
    ]
  }
}