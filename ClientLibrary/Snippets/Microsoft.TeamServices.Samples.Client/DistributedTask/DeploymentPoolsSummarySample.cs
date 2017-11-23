using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.TeamServices.Samples.Client.DistributedTask
{
    [ClientSample(TaskResourceIds.AreaName, TaskResourceIds.DeploymentPoolsSummaryResource)]
    class DeploymentPoolsSummarySample: DeploymentGroupsSamplesBase
    {
        [ClientSampleMethod]
        public IList<DeploymentPoolSummary> ListAllDeploymentPoolsSummaries()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get all deployment pools' summaries
            IList<DeploymentPoolSummary> summaries = dgClient.GetDeploymentPoolsSummaryAsync().Result;

            return summaries;
        }

        [ClientSampleMethod]
        public DeploymentPoolSummary GetDeploymentPoolSummaryByName()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get deployment pool's summary by name            
            DeploymentPoolSummary summary = dgClient.GetDeploymentPoolsSummaryAsync(poolName: this.DemoDeploymentPoolName).Result.FirstOrDefault();

            return summary;
        }
    }
}
