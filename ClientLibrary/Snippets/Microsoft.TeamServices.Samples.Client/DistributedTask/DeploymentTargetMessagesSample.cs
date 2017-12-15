using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.TeamServices.Samples.Client.DistributedTask
{
    [ClientSample(TaskResourceIds.AreaName, TaskResourceIds.DeploymentTargetMessagesResource)]
    class DeploymentTargetMessagesSample : DeploymentGroupsSamplesBase
    {
        [ClientSampleMethod]
        public void UpgradeDeploymentTargetsInDeploymentGroup()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // upgrade all deployment targets in demo deployment group
            dgClient.RefreshDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId).SyncResult();
        }
    }
}
