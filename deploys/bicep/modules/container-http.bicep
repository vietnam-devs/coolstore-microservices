param containerAppName string
param location string
param environmentName string
param containerImage string
param containerPort int
param isExternalIngress bool
param enableDapr bool = true
param enableIngress bool
param minReplicas int = 1
param secrets array = []
param env array = []

resource environment 'Microsoft.App/managedEnvironments@2022-01-01-preview' existing = {
  name: environmentName
}

resource containerApp 'Microsoft.App/containerApps@2022-01-01-preview' = {
  name: containerAppName
  location: location
  properties: {
    managedEnvironmentId: environment.id
    configuration: {
      secrets: secrets
      registries: null
      ingress: enableIngress ? {
        external: isExternalIngress
        targetPort: containerPort
        transport: 'auto'
      } : null
      dapr: enableDapr ? {
        enabled: true
        appPort: containerPort
        appId: containerAppName
        appprotocol: 'http'
      } : null
    }
    template: {
      containers: [
        {
          image: containerImage
          name: containerAppName
          env: env
        }
      ]
      scale: {
        minReplicas: minReplicas
        maxReplicas: 1
      }
    }
  }
}

output fqdn string = enableIngress ? containerApp.properties.configuration.ingress.fqdn : 'Ingress not enabled'
