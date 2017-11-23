using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.TeamServices.Samples.Client.DistributedTask
{
    [ClientSample(TaskResourceIds.AreaName, TaskResourceIds.DeploymentTargetJobRequestsResource)]
    public class DeploymentTargetJobRequestsSample: DeploymentGroupsSamplesBase
    {
        [ClientSampleMethod]
        public IList<TaskAgentJobRequest> GetJobRequestsOfADeploymentTarget()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get ID of one deployment target in DemoDeploymentGroup
            int deploymentTargetId = dgClient.GetDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId).Result.First().Id;

            IList<TaskAgentJobRequest> jobRequests = dgClient.GetAgentRequestsForDeploymentTargetAsync(this.ProjectName, this.DemoDeploymentGroupId, deploymentTargetId).Result;

            return jobRequests;
        }

        [ClientSampleMethod]
        public IList<TaskAgentJobRequest> GetJobRequestsOfAllDeploymentTargets()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            IList<TaskAgentJobRequest> jobRequests = dgClient.GetAgentRequestsForDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId).Result;

            return jobRequests;
        }

        [ClientSampleMethod]
        public IList<TaskAgentJobRequest> GetJobRequestsOfFewDeploymentTargets()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get IDs of offline deployment targets in DemoDeploymentGroup
            IEnumerable<int> deploymentTargetIds = dgClient.GetDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId).Result.Select(x => x.Id);

            IList<TaskAgentJobRequest> jobRequests = dgClient.GetAgentRequestsForDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId, deploymentTargetIds).Result;

            return jobRequests;
        }
    }
}
