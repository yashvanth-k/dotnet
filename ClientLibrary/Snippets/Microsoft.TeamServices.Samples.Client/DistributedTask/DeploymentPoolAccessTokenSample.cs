using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.WebApi;

namespace Microsoft.TeamServices.Samples.Client.DistributedTask
{
    [ClientSample(TaskResourceIds.AreaName, TaskResourceIds.DeploymentPoolAccessTokenResource)]
    public class DeploymentPoolAccessTokenSample : DeploymentGroupsSamplesBase
    {
        [ClientSampleMethod]
        public string GenerateDeploymentPoolAccessToken()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Generate access token for DemoDeploymentPool
            string accessToken = dgClient.GenerateDeploymentPoolAccessTokenAsync(this.DemoDeploymentPoolId).Result;

            return accessToken;
        }
    }
}
