using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.TeamServices.Samples.Client.DistributedTask
{
    [ClientSample(TaskResourceIds.AreaName, TaskResourceIds.DeploymentGroupsMetricsResource)]
    class DeploymentGroupsMetricsSample : DeploymentGroupsSamplesBase
    {
        [ClientSampleMethod]
        public IList<DeploymentGroupMetrics> ListAllDeploymentGroupMetrics()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get all deployment groups' metrics
            IList<DeploymentGroupMetrics> metrics = dgClient.GetDeploymentGroupsMetricsAsync2(this.ProjectName).SyncResult();

            return metrics;
        }

        [ClientSampleMethod]
        public DeploymentGroupMetrics GetDeploymentGroupMetricsByName()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get deployment group's metrics by name            
            DeploymentGroupMetrics metrics = dgClient.GetDeploymentGroupsMetricsAsync2(this.ProjectName, this.DemoDeploymentGroupName).SyncResult().FirstOrDefault();

            return metrics;
        }
    }
}
