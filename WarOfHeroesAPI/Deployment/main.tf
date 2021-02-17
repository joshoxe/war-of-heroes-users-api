# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = ">= 2.26"
    }
  }

backend "azurerm" {
  container_name = "tfstate28596"
  key            = "tf.tfstate"
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "warOfHeroes"
  location = "UK South"
}

resource "azurerm_app_service_plan" "app_service_plan" {
    name                = "warOfHeroes-appservice"
    location            = azurerm_resource_group.rg.location
    resource_group_name = azurerm_resource_group.rg.name

    sku {
        tier = "Standard"
        size = "S1"
    }
}

resource "azurerm_app_service" "app_service" {
    name                = "warOfHeroes"
    location            = azurerm_resource_group.rg.location
    resource_group_name = azurerm_resource_group.rg.name
    app_service_plan_id = azurerm_app_service_plan.app_service_plan.id
    https_only = true
    site_config {
        windows_fx_version = "DOTNETCORE|3.1"
    }
}