// az group create -n rg-user-service-eastus2 -l eastus2
// az deployment group create --resource-group  rg-user-service-eastus2 --template-file main.bicep
// az group delete -n rg-user-service-eastus2

// Default Location
param defaultLocation string = resourceGroup().location

// Shared Variables
var workflowName = 'user-service'
var cosmosDefaultAccountName = 'ca-${workflowName}-<REGION>'
var KeyVaultDefaultAccountName = 'kv-${workflowName}-<REGION>'
var appServicePlanDefaultName = 'appsp-${workflowName}-<REGION>'
var appServiceDefaultName = 'apps-${workflowName}-<REGION>'

// Cosmos Account Variables
var cosmosAccountName = replace(cosmosDefaultAccountName, '<REGION>', defaultLocation)
var cosmosDatabaseName = 'db-user-service'
var cosmosCollectionName = 'user'

// Key Vault Variables
var keyVaultName = replace(KeyVaultDefaultAccountName, '<REGION>', defaultLocation)

// App Service Variables
var appServicePlanName = replace(appServicePlanDefaultName, '<REGION>', defaultLocation)
var appServiceName = replace(appServiceDefaultName, '<REGION>', defaultLocation)
var appServicePlanSku = 'B1'
var appServiceLinuxFxVersion = 'DOTNETCORE|6.0'

module appServiceModule 'Modules/appServiceModule.bicep' = {
  name: 'appServiceModule'
  params: {
    appServicePlanName: appServicePlanName
    sku: appServicePlanSku
    appServicePlanLocation: defaultLocation
    appServiceName: appServiceName
    linuxFxVersion: appServiceLinuxFxVersion
  }  
}

module cosmosAccountModule 'Modules/cosmosAccountModule.bicep' = {
  name: 'cosmosAccountModule'
  params: {
    cosmosAccountName: cosmosAccountName
    cosmosDatabaseName: cosmosDatabaseName
    cosmosCollectionName: cosmosCollectionName
    cosmosAccountLocation: defaultLocation
  }  
}

module keyVaultModule 'Modules/keyVaultModule.bicep' = {
  name: 'keyVaultModule'
  params: {
    keyVaultName: keyVaultName
    keyVaultLocation: defaultLocation
    principalId : appServiceModule.outputs.appServicePrincipalId
    cosmosSecret : listConnectionStrings(resourceId('Microsoft.DocumentDB/databaseAccounts', cosmosAccountName ), '2020-04-01').connectionStrings[0].connectionString
  }
  dependsOn:[
    cosmosAccountModule
  ]
}
