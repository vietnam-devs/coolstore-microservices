param location string = resourceGroup().location
param environmentName string = 'env-${uniqueString(resourceGroup().id)}'
param appInsightsName string = 'app-insights-${uniqueString(resourceGroup().id)}'
param logAnalyticsWorkspaceName string = 'log-analytics-workspace-${uniqueString(resourceGroup().id)}'

param minReplicas int = 0

// Web Api Gateway Service
param webApiGatewayAppName string = 'webapigatewayapp'
param webApiGatewayAppImage string = 'ghcr.io/vietnam-devs/coolstore-microservices/webapigatewayapp-v6:0.1.0'
param webApiGatewayAppPort int = 80

// Inventory Service
param inventoryAppName string = 'inventoryapp'
param inventoryAppImage string = 'ghcr.io/vietnam-devs/coolstore-microservices/inventoryapp-v6:0.1.0'
param inventoryAppPort int = 5002

param productCatalogAppName string = 'productcatalogapp'
param productCatalogAppImage string = 'ghcr.io/vietnam-devs/coolstore-microservices/productcatalogapp-v6:0.1.0'
param productCatalogAppPort int = 5003

param shoppingCartAppName string = 'shoppingcartapp'
param shoppingCartAppImage string = 'ghcr.io/vietnam-devs/coolstore-microservices/shoppingcartapp-v6:0.1.0'
param shoppingCartAppPort int = 5004

param saleAppName string = 'saleapp'
param saleAppImage string = 'ghcr.io/vietnam-devs/coolstore-microservices/saleapp-v6:0.1.0'
param saleAppPort int = 5005

// Web App Service
param webAppName string = 'webapp'
param webAppImage string = 'ghcr.io/vietnam-devs/coolstore-microservices/webapp-v6:0.1.0'
param webAppPort int = 3000

// Identity App Service
param identityAppName string = 'identityapp'
param identityAppImage string = 'ghcr.io/vietnam-devs/coolstore-microservices/identityapp-v6:0.1.0'
param identityAppPort int = 80

param inventoryPostgresHost string
param productCategoryPostgresHost string

@secure()
param postgresDbPassword string
//redis
param redisConnection string
param redisHost string
param redisPassword string

// Container Apps Environment
module environment 'modules/environment.bicep' = {
  name: '${deployment().name}--environment'
  params: {
    environmentName: environmentName
    appInsightsName: appInsightsName
    logAnalyticsWorkspaceName: logAnalyticsWorkspaceName
    location: location
  }
}

// Dapr statestore component
resource stateDaprComponent 'Microsoft.App/managedEnvironments/daprComponents@2022-01-01-preview' = {
  name: '${environmentName}/statestore'
  dependsOn: [
    identityApp
    webApiGatewayApp
    environment
    inventoryApp
    productCatalogApp
    shoppingCartApp
    saleApp
  ]
  properties: {
    componentType: 'state.redis'
    version: 'v1'
    secrets: [
      {
        name: 'redis-password'
        value: redisPassword
      }
    ]
    metadata: [
      {
        name: 'redisHost'
        value: redisHost
      }
      {
        name: 'redisPassword'
        secretRef: 'redis-password'
      }
      {
        name: 'actorStateStore'
        value: 'false'
      }
    ]
    scopes: [
      identityAppName
      webApiGatewayAppName
      inventoryAppName
      productCatalogAppName
      shoppingCartAppName
      saleAppName
    ]
  }
}

// Dapr pubsub component
resource pubsubDaprComponent 'Microsoft.App/managedEnvironments/daprComponents@2022-01-01-preview' = {
  name: '${environmentName}/pubsub'
  dependsOn: [
    environment
    identityApp
    webApiGatewayApp
    environment
    inventoryApp
    productCatalogApp
    shoppingCartApp
    saleApp
  ]
  properties: {
    componentType: 'pubsub.in-memory'
    version: 'v1'
    scopes: [
      identityAppName
      webApiGatewayAppName
      inventoryAppName
      productCatalogAppName
      shoppingCartAppName
      saleAppName
    ]
  }
}

// Inventory App
module inventoryApp 'modules/container-http.bicep' = {
  name: '${deployment().name}--${inventoryAppName}'
  dependsOn: [
    environment
  ]
  params: {
    enableIngress: true
    isExternalIngress: true
    location: location
    environmentName: environmentName
    containerAppName: inventoryAppName
    containerImage: inventoryAppImage
    containerPort: inventoryAppPort
    minReplicas: minReplicas
    env: [
      {
        name: 'INVENTORY_HOST'
        value: '0.0.0.0'
      }
      {
        name: 'INVENTORY_PORT'
        value: '5002'
      }
      {
        name: 'PG_HOST'
        value: inventoryPostgresHost
      }
      {
        name: 'PG_PORT'
        value: '5432'
      }
      {
        name: 'PG_USER'
        value: 'coolstore'
      }
      {
        name: 'PG_PASSWORD'
        value: postgresDbPassword
      }
      {
        name: 'PG_INVENTORY_DATABASE'
        value: 'postgres'
      }
      {
        name: 'RUST_LOG'
        value: 'sqlx::query=error,tower_http=debug,info'
      }
      {
        name: 'RUST_BACKTRACE'
        value: '1'
      }
    ]
    secrets: []
  }
}

// Product Catalog App
module productCatalogApp 'modules/container-http.bicep' = {
  name: '${deployment().name}--${productCatalogAppName}'
  dependsOn: [
    environment
    inventoryApp
  ]
  params: {
    enableIngress: true
    isExternalIngress: true
    location: location
    environmentName: environmentName
    containerAppName: productCatalogAppName
    containerImage: productCatalogAppImage
    containerPort: productCatalogAppPort
    minReplicas: minReplicas
    env: [
      {
        name: 'PRODUCT_CATALOG_HOST'
        value: '0.0.0.0'
      }
      {
        name: 'PRODUCT_CATALOG_PORT'
        value: '5003'
      }
      {
        name: 'PG_HOST'
        value: productCategoryPostgresHost
      }
      {
        name: 'PG_PORT'
        value: '5432'
      }
      {
        name: 'PG_USER'
        value: 'coolstore'
      }
      {
        name: 'PG_PASSWORD'
        value: postgresDbPassword
      }
      {
        name: 'PG_PRODUCT_CATALOG_DATABASE'
        value: 'postgres'
      }
      {
        name: 'RUST_LOG'
        value: 'sqlx::query=error,tower_http=debug,info'
      }
      {
        name: 'RUST_BACKTRACE'
        value: '1'
      }
      {
        name: 'INVENTORY_CLIENT_URI'
        value: 'https://${inventoryApp.outputs.fqdn}'
      }
    ]
    secrets: []
  }
}

// Shopping Cart App
module shoppingCartApp 'modules/container-http.bicep' = {
  name: '${deployment().name}--${shoppingCartAppName}'
  dependsOn: [
    environment
  ]
  params: {
    enableIngress: true
    isExternalIngress: true
    location: location
    environmentName: environmentName
    containerAppName: shoppingCartAppName
    containerImage: shoppingCartAppImage
    containerPort: shoppingCartAppPort
    minReplicas: minReplicas
    env: [
      {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: 'Development'
      }
      {
        name: 'ASPNETCORE_URLS'
        value: 'http://+:5004'
      }
    ]
    secrets: []
  }
}

// Sale App
module saleApp 'modules/container-http.bicep' = {
  name: '${deployment().name}--${saleAppName}'
  dependsOn: [
    environment
  ]
  params: {
    enableIngress: true
    isExternalIngress: true
    location: location
    environmentName: environmentName
    containerAppName: saleAppName
    containerImage: saleAppImage
    containerPort: saleAppPort
    minReplicas: minReplicas
    env: []
    secrets: []
  }
}

// WebApiGateway App
// TODO: we need to modify this gateway to add configurations for identityapp and webapp as below
// az containerapp update `
//  --name webapigatewayapp `
//  --resource-group $rgName `
//  --set-env-vars 'OpenIdConnect__Authority=https://<identityapp output>' 'ReverseProxy__Clusters__appCluster__Destinations__destination1__Address=<webapp output>'
module webApiGatewayApp 'modules/container-http.bicep' = {
  name: '${deployment().name}--${webApiGatewayAppName}'
  dependsOn: [
    environment
    inventoryApp
    productCatalogApp
    shoppingCartApp
    saleApp
  ]
  params: {
    enableIngress: true
    isExternalIngress: true
    location: location
    environmentName: environmentName
    containerAppName: webApiGatewayAppName
    containerImage: webApiGatewayAppImage
    containerPort: webApiGatewayAppPort
    minReplicas: minReplicas
    env: [
      {
        name: 'Redis'
        value: redisConnection
      }
      {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: 'Development'
      }
      {
        name: 'ReverseProxy__Clusters__inventoryApiCluster__Destinations__destination1__Address'
        value: 'https://${inventoryApp.outputs.fqdn}'
      }
      {
        name: 'ReverseProxy__Clusters__productCatalogApiCluster__Destinations__destination1__Address'
        value: 'https://${productCatalogApp.outputs.fqdn}'
      }
      {
        name: 'ReverseProxy__Clusters__shoppingCartApiCluster__Destinations__destination1__Address'
        value: 'https://${shoppingCartApp.outputs.fqdn}'
      }
      {
        name: 'ReverseProxy__Clusters__saleApiCluster__Destinations__destination1__Address'
        value: 'https://${saleApp.outputs.fqdn}'
      }
    ]
    secrets: []
  }
}

// Identity App
module identityApp 'modules/container-http.bicep' = {
  name: '${deployment().name}--${identityAppName}'
  dependsOn: [
    environment
    webApiGatewayApp
  ]
  params: {
    enableIngress: true
    isExternalIngress: true
    location: location
    environmentName: environmentName
    containerAppName: identityAppName
    containerImage: identityAppImage
    containerPort: identityAppPort
    minReplicas: minReplicas
    env: [
      {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: 'Development'
      }
      {
        name: 'PublicClientUrl'
        value: 'https://${webApiGatewayApp.outputs.fqdn}'
      }
      {
        name: 'InternalClientUrl'
        value: 'https://localhost'
      }
    ]
    secrets: []
  }
}

// Web App
module webApp 'modules/container-http.bicep' = {
  name: '${deployment().name}--${webAppName}'
  dependsOn: [
    environment
    webApiGatewayApp
  ]
  params: {
    enableIngress: true
    isExternalIngress: true
    enableDapr: false
    location: location
    environmentName: environmentName
    containerAppName: webAppName
    containerImage: webAppImage
    containerPort: webAppPort
    minReplicas: minReplicas
    env: [
      {
        name: 'API_URL'
        value: 'https://${webApiGatewayApp.outputs.fqdn}'
      }
    ]
    secrets: []
  }
}

output identityAppFqdn string = identityApp.outputs.fqdn
output saleAppFqdn string = saleApp.outputs.fqdn
output environmentName string = environmentName
output webApiGatewayAppFqdn string = webApiGatewayApp.outputs.fqdn
output webAppFqdn string = webApp.outputs.fqdn
output inventoryAppFqdn string = inventoryApp.outputs.fqdn
output productCatalogAppFqdn string = productCatalogApp.outputs.fqdn
output shoppingCartAppFqdn string = shoppingCartApp.outputs.fqdn
