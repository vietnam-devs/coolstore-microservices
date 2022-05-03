param environmentName string
param redisName string

param shoppingCartAppName string

resource redis 'Microsoft.Cache/redis@2020-12-01' existing = {
  name: redisName
}

// Dapr statestore component
resource stateDaprComponent 'Microsoft.App/managedEnvironments/daprComponents@2022-01-01-preview' = {
  name: '${environmentName}/statestore'
  properties: {
    componentType: 'state.redis'
    version: 'v1'
    secrets: [
      {
        name: 'redis-password'
        value: listKeys(redis.id, redis.apiVersion).primaryKey
      }
    ]
    metadata: [
      {
        name: 'redisHost'
        value: '${redis.properties.hostName}:${redis.properties.port}'
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
      shoppingCartAppName
    ]
  }
}
