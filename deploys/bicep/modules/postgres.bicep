param databaseName string
param postgresDbUserName string = 'coolstore'
param postgresDbPassword string
param location string

resource postgresDbHost 'Microsoft.DBforPostgreSQL/flexibleServers@2021-06-01' = {
  name: databaseName
  location: location
  sku: {
    name: 'Standard_B1ms'
    tier: 'Burstable'
  }
  properties: {
    administratorLogin: postgresDbUserName
    administratorLoginPassword: postgresDbPassword
    highAvailability: {
      mode: 'Disabled'
    }
    storage: {
      storageSizeGB: 32
    }
    version: '13'
  }
}

resource sqlfirewall 'Microsoft.DBforPostgreSQL/flexibleServers/firewallRules@2021-06-01' = {
  name: '${databaseName}-AllowAllWindowsAzureIps'
  parent: postgresDbHost
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}

output postgresDbHost string = postgresDbHost.properties.fullyQualifiedDomainName
output postgresDbUserName string = postgresDbHost.properties.administratorLogin
