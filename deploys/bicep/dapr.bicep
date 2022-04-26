param environmentName string

@secure()
param redisHost string
param redisPassword string

param identityAppName string = 'identityapp'
param webApiGatewayAppName string = 'webapigatewayapp'
param inventoryAppName string = 'inventoryapp'
param productcatalogAppName string = 'productcatalogapp'
param shoppingcartAppName string = 'shoppingcartapp'
param saleAppName string = 'saleapp'

resource environment 'Microsoft.App/managedEnvironments@2022-01-01-preview' existing = {
  name: environmentName
}

resource stateDaprComponent 'Microsoft.App/managedEnvironments/daprComponents@2022-01-01-preview' = {
  name: '${environmentName}/state'
  dependsOn: [
    environment
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
      productcatalogAppName
      shoppingcartAppName
      saleAppName
    ]
  }
}

resource pubsubDaprComponent 'Microsoft.App/managedEnvironments/daprComponents@2022-01-01-preview' = {
  name: '${environmentName}/pubsub'
  dependsOn: [
    environment
  ]
  properties: {
    componentType: 'pubsub.in-memory'
    version: 'v1'
    scopes: [
      identityAppName
      webApiGatewayAppName
      inventoryAppName
      productcatalogAppName
      shoppingcartAppName
      saleAppName
    ]
  }
}
