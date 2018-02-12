using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.TeamServices.Samples.Client.DistributedTask
{
    [ClientSample(TaskResourceIds.AreaName, TaskResourceIds.DeploymentTargetsResource)]
    public class DeploymentTargetsSample : DeploymentGroupsSamplesBase
    {
        [ClientSampleMethod]
        public IList<DeploymentMachine> ListAllDeploymentTargets()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get deployment targets in DemoDeploymentGroup
            IList<DeploymentMachine> deploymentTargets = dgClient.GetDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId).Result;

            return deploymentTargets;
        }

        [ClientSampleMethod]
        public IList<DeploymentMachine> GetDeploymentTargetsWithGivenTags()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get deployment targets in DemoDeploymentGroup filtered by tags
            IList<string> tags = new[] { "web", "db" };
            IList<DeploymentMachine> deploymentTargets = dgClient.GetDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId, tags: tags).Result;

            return deploymentTargets;
        }

        [ClientSampleMethod]
        public IList<DeploymentMachine> GetDeploymentTargetsWithPartialNameMatch()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get deployment targets by partial name match
            string nameMatchPattern = "demo*get1";
            IList<DeploymentMachine> deploymentTargets = dgClient.GetDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId, name: nameMatchPattern, partialNameMatch: true).Result;

            return deploymentTargets;
        }

        [ClientSampleMethod]
        public IList<DeploymentMachine> GetDeploymentTargetsIncludingLastJobRequest()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get deployment targets in DemoDeploymentGroup including their last job request
            IList<DeploymentMachine> deploymentTargets = dgClient.GetDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId, name: "demoTarget1", expand: DeploymentTargetExpands.LastCompletedRequest).Result;

            return deploymentTargets;
        }

        [ClientSampleMethod]
        public IList<DeploymentMachine> GetDeploymentTargetsFilteredByAgentStatus()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get deployment targets that are online
            IList<DeploymentMachine> deploymentTargets = dgClient.GetDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId, agentStatus: TaskAgentStatusFilter.Online).Result;

            return deploymentTargets;
        }

        [ClientSampleMethod]
        public IList<DeploymentMachine> GetDeploymentTargetsInPages()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get 2 deployment targets >= demoTarget1
            IList<DeploymentMachine> deploymentTargets = dgClient.GetDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId, continuationToken: "demoTarget1", top: 2).Result;

            return deploymentTargets;
        }

        [ClientSampleMethod]
        public DeploymentMachine GetADeploymentTargetByItsID()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Pick an id of existing deployment target
            int targetId = dgClient.GetDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId).Result.First().Id;

            DeploymentMachine target = dgClient.GetDeploymentTargetAsync(this.ProjectName, this.DemoDeploymentGroupId, targetId).Result;

            return target;
        }

        [ClientSampleMethod]
        public IList<DeploymentMachine> UpdateTagsOfDeploymentTargets()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get deployment targets that are offline
            IList<DeploymentMachine> deploymentTargets = dgClient.GetDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId, agentStatus: TaskAgentStatusFilter.Offline).Result;

            // Reduce the unnecessary properties and update tags
            IList<DeploymentTargetUpdateParameter> targetUpdates = new List<DeploymentTargetUpdateParameter>();
            foreach (DeploymentMachine target in deploymentTargets)
            {
                DeploymentTargetUpdateParameter targetUpdate = new DeploymentTargetUpdateParameter
                {
                    Id = target.Id,
                    Agent = new DeploymentTargetUpdateParameterAgentProperty { Id = target.Agent.Id },
                    Tags = target.Tags
                };
                targetUpdate.Tags.Add("newTag" + DateTime.UtcNow.ToBinary());

                targetUpdates.Add(targetUpdate);
            }

            // Update the tags
            IList<DeploymentMachine> updatedDeploymentTargets = dgClient.UpdateDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId, targetUpdates).Result;

            return updatedDeploymentTargets;
        }

        [ClientSampleMethod]
        public void DeleteDeploymentTarget()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Create a dummy target
            // Generally targets are added by agent configuration. We do this for demo purpose
            DeploymentMachine target = CreateDummyTarget();

            // Delete the target
            dgClient.DeleteDeploymentTargetAsync(this.ProjectName, this.DemoDeploymentGroupId, target.Id).SyncResult();
        }

        private DeploymentMachine CreateDummyTarget()
        {
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            DeploymentMachine target = dgClient.GetDeploymentTargetsAsync(this.ProjectName, this.DemoDeploymentGroupId).Result.FirstOrDefault();
            DeploymentMachine newTarget = target.Clone();
            newTarget.Agent.Name = "deploymentTargetToBeDeleted";

            DeploymentMachine addedTarget = dgClient.AddDeploymentTargetAsync(this.ProjectName, this.DemoDeploymentGroupId, newTarget).Result;
            return addedTarget;
        }
    }
}
