// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using Microsoft.Azure.Management.Samples.Common;

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
            var rgName = "rgRSAT";
            var deploymentName = "dpRSAT";
            try
            {
                //=============================================================
                // Create resource group.
                Utilities.Log("Creating a resource group with name: " + rgName);

                var subscription = await client.GetDefaultSubscriptionAsync();
                var rgLro = await subscription.GetResourceGroups().CreateOrUpdateAsync(WaitUntil.Completed, rgName, new ResourceGroupData(AzureLocation.WestUS));

                var resourceGroup = rgLro.Value;
                _resourceGroupId = resourceGroup.Id;

                Utilities.Log("Created a resource group with name: " + rgName);
                //=============================================================
                // Create a deployment for an Azure App Service via an ARM
                // template.

                Utilities.Log("Starting a deployment for an Azure App Service: " + deploymentName);

                var templateContent = File.ReadAllText(Path.Combine(Utilities.ProjectPath, "Asset", "ArmTemplate.json")).TrimEnd();
                var hostingPlanName = "hpRSAT";
                var webAppName = "wnRSAT";
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
                            value = "B1"
                        },
                        skuCapacity = new
                        {
                            value = 1
                        },
                    })
                });
                var deploymentLro = await resourceGroup.GetArmDeployments().CreateOrUpdateAsync(WaitUntil.Completed, deploymentName, deploymentContent);

                Utilities.Log("Completed the deployment: " + deploymentName);
            }
            finally
            {
                try
                {
                    if (_resourceGroupId is not null)
                    {
                        Utilities.Log("Deleting Resource Group: " + rgName);
                        await client.GetResourceGroupResource(_resourceGroupId).DeleteAsync(WaitUntil.Completed);
                        Utilities.Log("Deleted Resource Group: " + rgName);
                    }
                }
                catch (Exception ex)
                {
                    Utilities.Log(ex);
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
                var client = new ArmClient(credential, subscriptionId, new ArmClientOptions()
                {
                    Diagnostics =
                    {
                        IsLoggingEnabled = true,
                        IsLoggingContentEnabled = true,
                    }
                });

                await RunSample(client);
            }
            catch (Exception ex)
            {
                Utilities.Log(ex);
            }
        }
    }
}