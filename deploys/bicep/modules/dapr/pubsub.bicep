param environmentName string
param serviceBusNamespaceName string

param shoppingCartAppName string
param saleAppName string

resource serviceBus 'Microsoft.ServiceBus/namespaces@2021-06-01-preview' existing = {
  name: serviceBusNamespaceName
}

// Dapr pubsub component
resource pubsubDaprComponent 'Microsoft.App/managedEnvironments/daprComponents@2022-01-01-preview' = {
  name: '${environmentName}/pubsub'
  properties: {
    componentType: 'pubsub.azure.servicebus'
    version: 'v1'
    metadata: [
      {
        name: 'connectionString'
        secretRef: 'sb-root-connectionstring'
      }
    ]
    secrets: [
      {
        name: 'sb-root-connectionstring'
        value: listKeys('${serviceBus.id}/AuthorizationRules/RootManageSharedAccessKey', serviceBus.apiVersion).primaryConnectionString
      }
    ]
    scopes: [
      shoppingCartAppName
      saleAppName
    ]
  }
}
