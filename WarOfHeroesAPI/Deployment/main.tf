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
    container_name        = "tfstate-users"
    key                   = "tf.tfstate"
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "warOfHeroesUsers"
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
    connection_string {
      name = "UserDb"
      type = "SQLServer"
      value = "Server=tcp:${azurerm_sql_server.users_db_server.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_mssql_database.users_db.name};Persist Security Info=False;User ID=${azurerm_sql_server.users_db_server.administrator_login};Password=${azurerm_sql_server.users_db_server.administrator_login_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
    }
}


resource "azurerm_sql_server" "users_db_server" {
  name                         = "josh-users-server-123"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = "East US"
  version                      = "12.0"
  administrator_login          = var.DB_USERNAME
  administrator_login_password = var.DB_PASSWORD

  tags = {
    environment = "production"
  }
}

resource "azurerm_mssql_database" "users_db" {
  name           = "josh-users-db-123"
  server_id      = azurerm_sql_server.users_db_server.id
  sku_name       = "Basic"
}