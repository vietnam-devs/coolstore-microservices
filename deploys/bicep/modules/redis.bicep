param redisName string
param location string

resource redis 'Microsoft.Cache/redis@2020-12-01' = {
  name: redisName
  location: location
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 0
    }
    enableNonSslPort: true
  }
}

output redisHost string = '${redis.properties.hostName}:${redis.properties.port},password=${redis.listKeys().primaryKey},ssl=False,abortConnect=False'
