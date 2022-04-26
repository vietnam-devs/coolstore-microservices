param environmentName string
param webApiGatewayAppFqdn string

param location string = resourceGroup().location
param minReplicas int = 0

// Web App Service
param webAppName string = 'webapp'
param webAppImage string = 'ghcr.io/vietnam-devs/coolstore-microservices/webapp-v6:0.1.0'
param webAppPort int = 3000

// Identity App Service
param identityAppName string = 'identityapp'
param identityAppImage string = 'ghcr.io/vietnam-devs/coolstore-microservices/identityapp-v6:0.1.0'
param identityAppPort int = 80

resource environment 'Microsoft.App/managedEnvironments@2022-01-01-preview' existing = {
  name: environmentName
}

// Identity App
module identityApp 'modules/container-http.bicep' = {
  name: '${deployment().name}--${identityAppName}'
  dependsOn: [
    environment
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
        value: webApiGatewayAppFqdn
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
  ]
  params: {
    enableIngress: true
    isExternalIngress: true
    location: location
    environmentName: environmentName
    containerAppName: webAppName
    containerImage: webAppImage
    containerPort: webAppPort
    minReplicas: minReplicas
    env: [
      {
        name: 'API_URL'
        value: webApiGatewayAppFqdn
      }
    ]
    secrets: []
  }
}

output identityAppFqdn string = identityApp.outputs.fqdn
output webAppFqdn string = webApp.outputs.fqdn
