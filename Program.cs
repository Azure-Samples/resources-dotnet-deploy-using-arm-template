// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;

namespace DeployUsingARMTemplate
{
    public class Program
    {
        private static ResourceIdentifier? _resourceGroupId = null;

        /**
         * Azure Resource sample for deploying resources using an ARM template.
         */
        public static async Task RunSample(ArmClient client)
        {
            var rgName = "rgRSAT"; // change the value here for your own resource group name
            var deploymentName = "dpRSAT"; // change the value here for your own arm deployment name
            try
            {
                //=============================================================
                // Create resource group.
                Console.WriteLine($"Creating a resource group with name: {rgName}");

                var subscription = await client.GetDefaultSubscriptionAsync();
                var rgLro = await subscription.GetResourceGroups().CreateOrUpdateAsync(WaitUntil.Completed, rgName, new ResourceGroupData(AzureLocation.WestUS));
                var resourceGroup = rgLro.Value;
                _resourceGroupId = resourceGroup.Id;

                Console.WriteLine($"Created a resource group: {_resourceGroupId}");
                //=============================================================
                // Create a deployment for an Azure App Service via an ARM
                // template.

                Console.WriteLine($"Starting a deployment for an Azure App Service: {deploymentName}");

                // tweak the values here to customize your web app
                var hostingPlanName = "hpRSAT";
                var webAppName = "wnRSAT";
                var webSkuName = "B1";
                var webSkuCapacity = 1;
                var templateContent = File.ReadAllText(Path.Combine(".", "Asset", "ArmTemplate.json")).TrimEnd();
                var deploymentContent = new ArmDeploymentContent(new ArmDeploymentProperties(ArmDeploymentMode.Incremental)
                {
                    Template = BinaryData.FromString(templateContent),
                    Parameters = BinaryData.FromObjectAsJson(new
                    {
                        hostingPlanName = new
                        {
                            value = hostingPlanName
                        },
                        webSiteName = new
                        {
                            value = webAppName
                        },
                        skuName = new
                        {
                            value = webSkuName
                        },
                        skuCapacity = new
                        {
                            value = webSkuCapacity
                        },
                    })
                });
                // we do not need the response of this deployment, therefore we just wait the deployment to complete here and discard the response.
                await resourceGroup.GetArmDeployments().CreateOrUpdateAsync(WaitUntil.Completed, deploymentName, deploymentContent);

                Console.WriteLine($"Completed the deployment: {deploymentName}");
            }
            finally
            {
                try
                {
                    if (_resourceGroupId is not null)
                    {
                        Console.WriteLine($"Deleting Resource Group: {_resourceGroupId}");
                        await client.GetResourceGroupResource(_resourceGroupId).DeleteAsync(WaitUntil.Completed);
                        Console.WriteLine($"Deleted Resource Group: {_resourceGroupId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public static async Task Main(string[] args)
        {
            try
            {
                //=================================================================
                // Authenticate
                var credential = new DefaultAzureCredential();

                var subscriptionId = Environment.GetEnvironmentVariable("AZURE_SUBSCRIPTION_ID");
                // you can also use `new ArmClient(credential)` here, and the default subscription will be the first subscription in your list of subscription
                var client = new ArmClient(credential, subscriptionId);

                await RunSample(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}