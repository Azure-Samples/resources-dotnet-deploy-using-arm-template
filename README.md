---
page_type: sample
languages:
- csharp
products:
- azure
extensions:
  services: Resource-Manager
  platforms: dotnet
---

# Getting started on deploying using an ARM template in C# #

 Azure Resource sample for deploying resources using an ARM template.


## Running this Sample ##

To run this sample, first you need to set up a way to authenticate to Azure with Azure Identity.

Some options are:
- Through the [Azure CLI Login](https://docs.microsoft.com/cli/azure/authenticate-azure-cli).
- Via [Visual Studio](https://docs.microsoft.com/dotnet/api/overview/azure/identity-readme?view=azure-dotnet#authenticating-via-visual-studio).
- Setting [Environment Variables](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/resourcemanager/Azure.ResourceManager/docs/AuthUsingEnvironmentVariables.md).

```bash
git clone https://github.com/Azure-Samples/resources-dotnet-deploy-using-arm-template.git
cd resources-dotnet-deploy-using-arm-template
dotnet run
```

## More information ##

[Azure Management Libraries for C#](https://github.com/Azure/azure-sdk-for-net)
[Azure .Net Developer Center](https://azure.microsoft.com/en-us/develop/net/)
If you don't have a Microsoft Azure subscription you can get a FREE trial account [here](http://go.microsoft.com/fwlink/?LinkId=330212)

---

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.