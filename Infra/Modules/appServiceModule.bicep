@description('Specify the name of the app service plan')
param appServicePlanName string

@description('Specify the location of the app service plan')
param appServicePlanLocation string

@description('Specify the sku of the app service plan')
param sku string

@description('Specify the name of the app service')
param appServiceName string

@description('Specify the linuxFxVersion of the app service')
param linuxFxVersion string

resource identity 'Microsoft.ManagedIdentity/userAssignedIdentities@2018-11-30' = {
  name: 'uai-app-service-eastus2'
  location: appServicePlanLocation
}

resource appServicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: appServicePlanName
  location: appServicePlanLocation
  properties: {
    reserved: true
  }
  sku: {
    name: sku
  }
  kind: 'linux'
}
resource appService 'Microsoft.Web/sites@2020-06-01' = {
  name: appServiceName
  location: appServicePlanLocation
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${identity.id}': {}
    }
  }
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: linuxFxVersion
     }
  }
}

output appServicePrincipalId string = identity.properties.principalId
