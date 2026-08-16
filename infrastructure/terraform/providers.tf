terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.0" # Standard stable version
    }
  }
}

provider "azurerm" {
  features {}
  # Because you are doing this locally right now, it will use your Azure CLI login
}