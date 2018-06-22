To experiment with reducing the cold start, this will experiment with Azure Event Grid subscriptions.

## Observations

### Turns out creating the Event Grid subscription was trial and error but it eventually worked.
Using AZ CLI:
```
eventgrid event-subscription create --name WhenBuildButtonAddsToBlob \
--resource-group <resource-group-name> \
--resource-id /subscriptions/<subscription-id>/resourceGroups/<resource-group-name> \
--subject-begins-with /blobServices/default/containers/<container-name>/ \
--endpoint https://<app-service-name>.azurewebsites.net/runtime/webhooks/EventGridExtensionConfig?functionName=<function-name>&code=<function-code>
```
The above will create an Event Grid subscription that will subscribe to all blob storage events that occur but filtered to the ones that begin with the provided `--subject-begins-with` filter. 

### Sometimes, well, most times this failed because...
Storage account had to be upgraded to type V2. Using AZ CLI:
```
az storage account update -g build-button -n leonsfirstiothubstorage --set kind=StorageV2
```
This even lit up the Events blade in the Azure portal.