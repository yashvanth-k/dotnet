using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System.Linq;

namespace Microsoft.TeamServices.Samples.Client.DistributedTask
{
    public abstract class DeploymentGroupsSamplesBase: ClientSample
    {
        protected string ProjectName
        {
            get
            {
                return "DeploymentGroupRESTSample";
            }
        }

        protected string DemoDeploymentGroupName
        {
            get
            {
                return "DefaultDemoDeploymentGroup";
            }
        }

        protected string DemoDeploymentPoolName
        {
            get
            {
                if (string.IsNullOrEmpty(m_demoDeploymentPoolName))
                {
                    VssConnection connection = Context.Connection;
                    TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();
                    DeploymentGroup deploymentGroup = dgClient.GetDeploymentGroupsAsync(this.ProjectName, name: this.DemoDeploymentGroupName).Result.First();
                    m_demoDeploymentPoolName = deploymentGroup.Pool.Name;
                }

                return m_demoDeploymentPoolName;
            }
        }

        protected int DemoDeploymentGroupId
        {
            get
            {
                if (m_demoDeploymentGroupId == 0)
                {
                    VssConnection connection = Context.Connection;
                    TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();
                    DeploymentGroup deploymentGroup = dgClient.GetDeploymentGroupsAsync(this.ProjectName, name: this.DemoDeploymentGroupName).Result.First();
                    m_demoDeploymentGroupId = deploymentGroup.Id;
                }

                return m_demoDeploymentGroupId;
            }
        }

        protected int DemoDeploymentPoolId
        {
            get
            {
                if (m_demoDeploymentPoolId == 0)
                {
                    VssConnection connection = Context.Connection;
                    TaskAgentHttpClient dgClient = connection.GetClient<TaskAgentHttpClient>();
                    DeploymentGroup deploymentGroup = dgClient.GetDeploymentGroupsAsync(this.ProjectName, name: this.DemoDeploymentGroupName).Result.First();
                    m_demoDeploymentPoolId = deploymentGroup.Pool.Id;
                }

                return m_demoDeploymentPoolId;
            }
        }

        private string m_demoDeploymentPoolName;
        private int m_demoDeploymentGroupId;
        private int m_demoDeploymentPoolId;
    }
}
