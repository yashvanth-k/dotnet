using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.WebApi;

namespace Microsoft.TeamServices.Samples.Client.DistributedTask
{
    [ClientSample(TaskResourceIds.AreaName, TaskResourceIds.DeploymentGroupAccessTokenResource)]
    public class DeploymentGroupAccessTokenSample : DeploymentGroupsSamplesBase
    {
        [ClientSampleMethod]
        public string GenerateDeploymentGroupAccessToken()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();
            
            // Generate access token for DemoDeploymentGroup
            string accessToken = dgClient.GenerateDeploymentGroupAccessTokenAsync(this.ProjectName, this.DemoDeploymentGroupId).Result;

            return accessToken;
        }
    }
}
