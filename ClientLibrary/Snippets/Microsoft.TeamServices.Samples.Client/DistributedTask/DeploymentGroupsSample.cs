using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.TeamServices.Samples.Client.DistributedTask
{
    [ClientSample(TaskResourceIds.AreaName, TaskResourceIds.DeploymentGroupsResource)]
    public class DeploymentGroupsSample : DeploymentGroupsSamplesBase
    {
        [ClientSampleMethod]
        public DeploymentGroup CreateDeploymentGroup()
        {
            // Initialzie - Cleanup default deployment group to avoid conflict errors
            this.CleanupDefaultDeploymentGroup();

            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();
            
            // Create deployment groups
            DeploymentGroup deploymentGroup = new DeploymentGroup()
            {
                Name = "MyDeploymentGroup1",
                Description = "This deployment group is created to demnostrate the client usage"
            };
            DeploymentGroup addedDeploymentGroup = dgClient.AddDeploymentGroupAsync(this.ProjectName, deploymentGroup).Result;

            DeploymentGroupId = addedDeploymentGroup.Id;
            return addedDeploymentGroup;
        }

        [ClientSampleMethod]
        public DeploymentGroup GetDeploymentGroupById()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get deployment group by ID
            DeploymentGroup deploymentGroup = dgClient.GetDeploymentGroupAsync(this.ProjectName, this.DeploymentGroupId).Result;

            return deploymentGroup;
        }

        [ClientSampleMethod]
        public DeploymentGroup GetDeploymentGroupByName()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get deployment group by name
            DeploymentGroup deploymentGroup = dgClient.GetDeploymentGroupsAsync(this.ProjectName, "MyDeploymentGroup1").Result.FirstOrDefault();

            return deploymentGroup;
        }

        [ClientSampleMethod]
        public IList<DeploymentGroup> GetDeploymentGroupsByIds()
        {
            // Initialize - create few deployment groups
             IEnumerable<int> deploymentGroupIds = InitializeDemoDeploymentGroups(2).Select(x => x.Id);

            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // List all deployment groups
            IList<DeploymentGroup> deploymentGroups = dgClient.GetDeploymentGroupsAsync(this.ProjectName, ids: deploymentGroupIds).Result;

            return deploymentGroups;
        }

        [ClientSampleMethod]
        public IList<DeploymentGroup> ListAllDeploymentGroups()
        {
            // Initialize - create few deployment groups
            InitializeDemoDeploymentGroups(2);

            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // List all deployment groups
            IList<DeploymentGroup> deploymentGroups = dgClient.GetDeploymentGroupsAsync(this.ProjectName).Result;

            return deploymentGroups;
        }

        [ClientSampleMethod]
        public IList<DeploymentGroup> GetDeploymentGroupsInPages()
        {
            // Initialize - create few deployment groups
            InitializeDemoDeploymentGroups(10);

            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get 2 deployment groups >= MyDeploymentGroup12
            IList<DeploymentGroup> deploymentGroups = dgClient.GetDeploymentGroupsAsync(this.ProjectName, continuationToken: "MyDeploymentGroup12", top: 2).Result;

            return deploymentGroups;
        }

        [ClientSampleMethod]
        public DeploymentGroup UpdateDeploymentGroup()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Get deployment group and change properties to be updated
            DeploymentGroup deploymentGroup = dgClient.GetDeploymentGroupAsync(this.ProjectName, this.DeploymentGroupId).Result;
            deploymentGroup.Name = "MyDeploymentGroup1" + "-Update1";
            deploymentGroup.Description = "Description of this deployment group is updated";

            // Update deployment group
            DeploymentGroup updatedDeploymentGroup = dgClient.UpdateDeploymentGroupAsync(this.ProjectName, this.DeploymentGroupId, deploymentGroup).Result;

            return updatedDeploymentGroup;
        }

        [ClientSampleMethod]
        public void DeleteDeploymentGroup()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Delete deployment group by ID
            dgClient.DeleteDeploymentGroupAsync(this.ProjectName, DeploymentGroupId).SyncResult();
        }

        private void CleanupDefaultDeploymentGroup()
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Delete the default deployment group
            DeploymentGroup deploymentGroup = dgClient.GetDeploymentGroupsAsync(this.ProjectName, "MyDeploymentGroup1").Result.FirstOrDefault();
            if(deploymentGroup != null)
            {
                dgClient.DeleteDeploymentGroupAsync(this.ProjectName, deploymentGroup.Id).SyncResult();
            }
        }

        private IList<DeploymentGroup> InitializeDemoDeploymentGroups(int count)
        {
            // Get a connection and client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();

            // Create requested number of demo deployment groups if they don't exist
            IList<DeploymentGroup> addedDeploymentGroups = new List<DeploymentGroup>();
            DeploymentGroup existingDeploymentGroup = null;
            DeploymentGroup newDeploymentGroup = null;
            for (int i = 10; i < 10 + count; i++)
            {
                string deploymentGroupName = "MyDeploymentGroup" + i;
                existingDeploymentGroup = dgClient.GetDeploymentGroupsAsync(this.ProjectName, deploymentGroupName).Result.FirstOrDefault();

                if (existingDeploymentGroup == null)
                {
                    DeploymentGroup deploymentGroup = new DeploymentGroup()
                    {
                        Name = deploymentGroupName,
                        Description = "This deployment group is created to demnostrate the client usage"
                    };
                    newDeploymentGroup = dgClient.AddDeploymentGroupAsync(this.ProjectName, deploymentGroup).Result;
                    addedDeploymentGroups.Add(newDeploymentGroup);
                }
                else
                {
                    addedDeploymentGroups.Add(existingDeploymentGroup);
                }
            }

            // Remove any additional demo deployment groups
            for (int i = 10 + count; i < 20; i++)
            {
                string deploymentGroupName = "MyDeploymentGroup" + i;
                DeploymentGroup deploymentGroup = dgClient.GetDeploymentGroupsAsync(this.ProjectName, deploymentGroupName).Result.FirstOrDefault();

                if (deploymentGroup != null)
                {
                    dgClient.DeleteDeploymentGroupAsync(this.ProjectName, deploymentGroup.Id).SyncResult();
                }
            }

            return addedDeploymentGroups;
        }

        private int DeploymentGroupId
        {
            get
            {
                return m_deploymentGroupId;
            }
            set
            {
                m_deploymentGroupId = value;
            }
        }

        private int m_deploymentGroupId;
    }
}
