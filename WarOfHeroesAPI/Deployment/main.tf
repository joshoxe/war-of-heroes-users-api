# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      version = ">= 2.26"
    }
  }

backend "azurerm" {
    resource_group_name   = "tfstate"
    storage_account_name  = "tfstate28596"
    container_name        = "tfstate"
    key                   = "tf.tfstate"
  }
}

variable "username" {
  type        = string
  sensitive = true
}
variable "password" {
  type        = string
  sensitive = true
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "warOfHeroes"
  location = "UK South"
}

resource "azurerm_resource_group" "db_rg" {
  name     = "warOfHeroesDBs"
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

resource "azurerm_app_service" "users_app_service" {
    name                = "warOfHeroesUsers"
    location            = azurerm_resource_group.rg.location
    resource_group_name = azurerm_resource_group.rg.name
    app_service_plan_id = azurerm_app_service_plan.app_service_plan.id
    https_only = true
    site_config {
        windows_fx_version = "DOTNETCORE|3.1"
    }
}

resource "azurerm_app_service" "heroes_app_service" {
    name                = "warOfHeroesHeroes"
    location            = azurerm_resource_group.rg.location
    resource_group_name = azurerm_resource_group.rg.name
    app_service_plan_id = azurerm_app_service_plan.app_service_plan.id
    https_only = true
    site_config {
        windows_fx_version = "DOTNETCORE|3.1"
    }
}
  
resource "azurerm_app_service" "frontend_app_service" {
  name                = "warOfHeroes"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_app_service_plan.app_service_plan.id
  https_only = true
  site_config {
      windows_fx_version = "DOTNETCORE|3.1"
      default_documents = ["index.html"]
  }
}

resource "azurerm_sql_server" "user_db" {
  name                         = "UsersDb"
  resource_group_name          = azurerm_resource_group.db_rg.name
  location                     = azurerm_resource_group.db_rg.location
  version                      = "12.0"
  administrator_login          = var.username
  administrator_login_password = var.password

  tags = {
    environment = "production"
  }
}

resource "azurerm_sql_server" "heroes_db" {
  name                         = "HeroesDb"
  resource_group_name          = azurerm_resource_group.db_rg.name
  location                     = azurerm_resource_group.db_rg.location
  version                      = "12.0"
  administrator_login          = var.username
  administrator_login_password = var.password

  tags = {
    environment = "production"
  }
}