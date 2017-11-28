
namespace Microsoft.TeamServices.Samples.Client.VariableGroups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
 
    using Microsoft.TeamFoundation.DistributedTask.WebApi;
    using Microsoft.VisualStudio.Services.WebApi;

    /// <summary>
    /// The variable groups sample
    /// </summary>
    [ClientSample(TaskResourceIds.AreaName, TaskResourceIds.VariableGroupsResource)]
    class VariableGroupsSample : ClientSample
    {
        /// <summary>
        /// The added variable group id.
        /// </summary>
        private int addedVariableGroupId;

        /// <summary>
        /// The add a variable group.
        /// </summary>
        /// <returns>
        /// The <see cref="VariableGroup">.
        /// </returns>
        [ClientSampleMethod]
        public VariableGroup CreateVariableGroup()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;

            // Get a task agent client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient taskClient = connection.GetClient<TaskAgentHttpClient>();

            // Variable group object
            VariableGroupParameters variableGroupParameters = new VariableGroupParameters()
            {
                Type = VariableGroupType.Vsts,
                Name = "TestVariableGroup1",
                Description = "A test variable group",
                Variables = new Dictionary<string, VariableValue>()
                {
                    { "key1", new VariableValue(){ IsSecret = false, Value = "value1" } },
                    { "key2", new VariableValue(){ IsSecret = true, Value = "value2" } }
                }
            };

            // Create variable group
            VariableGroup addedVariableGroup = taskClient.AddVariableGroupAsync(project: projectName, group: variableGroupParameters).Result;
            this.addedVariableGroupId = addedVariableGroup.Id; 

            // Show the added variable group
            Console.WriteLine("{0} {1}", addedVariableGroup.Id.ToString(), addedVariableGroup.Name);

            return addedVariableGroup;
        }

        /// <summary>
        /// The update variable group.
        /// </summary>
        /// <returns>
        /// The <see cref="VariableGroup">.
        /// </returns>
        [ClientSampleMethod]
        public VariableGroup UpdateVariableGroup()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;

            // Get a task agent client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient taskClient = connection.GetClient<TaskAgentHttpClient>();

            // Get variable group to update
            VariableGroup variableGroup = taskClient.GetVariableGroupAsync(projectName, this.addedVariableGroupId).Result;

            // Updated description in variable group
            variableGroup.Description = "Updated variable group";

            // Create variable group parameters
            VariableGroupParameters variableGroupParameters = new VariableGroupParameters()
            {
                Type = variableGroup.Type,
                Name = variableGroup.Name,
                Description = variableGroup.Description,
                Variables = variableGroup.Variables,
                ProviderData = variableGroup.ProviderData
            };

            VariableGroup updatedVariableGroup = taskClient.UpdateVariableGroupAsync(project: projectName, groupId: this.addedVariableGroupId, group: variableGroupParameters).Result;

            // Show the updated variable group
            Console.WriteLine("{0} {1}", updatedVariableGroup.Id.ToString(), updatedVariableGroup.Description);

            return updatedVariableGroup;
        }

        /// <summary>
        /// The get variable group.
        /// </summary>
        /// <returns>
        /// The <see cref="VariableGroup">.
        /// </returns>
        [ClientSampleMethod]
        public VariableGroup GetVariableGroup()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;

            // Get a task agent client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient taskClient = connection.GetClient<TaskAgentHttpClient>();

            // Get variable group for a given id
            VariableGroup variableGroup = taskClient.GetVariableGroupAsync(project: projectName, groupId: this.addedVariableGroupId).Result;
            
            // Show the received variable group
            Console.WriteLine("{0} {1}", variableGroup.Id.ToString(), variableGroup.Name);

            return variableGroup;
        }

        /// <summary>
        /// The get variable groups by id.
        /// </summary>
        /// <returns>
        /// The <see cref="VariableGroup">.
        /// </returns>
        [ClientSampleMethod]
        public List<VariableGroup> GetVariableGroupsById()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;

            // Get a task agent client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient taskClient = connection.GetClient<TaskAgentHttpClient>();

            // Create second variable group
            VariableGroupParameters variableGroupParameters = new VariableGroupParameters()
            {
                Type = VariableGroupType.Vsts,
                Name = "TestVariableGroup2",
                Description = "A test variable group",
                Variables = new Dictionary<string, VariableValue>()
                {
                    { "key1", new VariableValue(){ IsSecret = false, Value = "value1" } }
                }
            };

            VariableGroup addedVariableGroup = taskClient.AddVariableGroupAsync(project: projectName, group: variableGroupParameters).Result;

            // Create list of ids
            int variableGroup1Id = this.addedVariableGroupId;
            int variableGroup2Id = addedVariableGroup.Id;
            List<int> variableGroupIds = new List<int>() {variableGroup1Id, variableGroup2Id};

            // Get variable group for a given ids
            List<VariableGroup> variableGroups = taskClient.GetVariableGroupsByIdAsync(project: projectName, groupIds: variableGroupIds).Result;

            // Show the received variable groups
            variableGroups.ForEach((VariableGroup vg) =>
            {
                Console.WriteLine("{0} {1}", vg.Id.ToString(), vg.Name);
            });

            return variableGroups;
        }

        /// <summary>
        /// The get variable groups by group name.
        /// </summary>
        /// <returns>
        /// The <see cref="VariableGroup">.
        /// </returns>
        [ClientSampleMethod]
        public List<VariableGroup> GetVariableGroupsByGroupName()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;
            VariableGroupQueryOrder queryOrder = VariableGroupQueryOrder.IdDescending;

            // Get a task agent client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient taskClient = connection.GetClient<TaskAgentHttpClient>();

            // Get variable groups for a given group name
            string groupName = "Test*";
            List<VariableGroup> variableGroups = taskClient.GetVariableGroupsAsync(project: projectName, groupName: groupName, queryOrder: queryOrder).Result;

            // Show the received variable groups
            variableGroups.ForEach((VariableGroup vg) =>
            {
                Console.WriteLine("{0} {1}", vg.Id.ToString(), vg.Name);
            });

            return variableGroups;
        }

        /// <summary>
        /// The get variable groups by action filter.
        /// </summary>
        /// <returns>
        /// The <see cref="VariableGroup">.
        /// </returns>
        [ClientSampleMethod]
        public List<VariableGroup> GetVariableGroupsByActionFilter()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;
            int top = 2;
            int continuationToken = this.addedVariableGroupId + 1;
            VariableGroupQueryOrder queryOrder = VariableGroupQueryOrder.IdAscending;

            // Get a task agent client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient taskClient = connection.GetClient<TaskAgentHttpClient>();

            // Create third variable group
            VariableGroupParameters variableGroupParameters = new VariableGroupParameters()
            {
                Type = VariableGroupType.Vsts,
                Name = "TestVariableGroup3",
                Description = "A test variable group",
                Variables = new Dictionary<string, VariableValue>()
                {
                    { "key1", new VariableValue(){ IsSecret = false, Value = "value1" } }
                }
            };

            VariableGroup addedVariableGroup = taskClient.AddVariableGroupAsync(project: projectName, group: variableGroupParameters).Result;

            // Get variable groups for a given action filter
            VariableGroupActionFilter actionFilter = VariableGroupActionFilter.Use;
            List<VariableGroup> variableGroups = taskClient.GetVariableGroupsAsync(project: projectName, actionFilter: actionFilter, top: top, continuationToken: continuationToken, queryOrder: queryOrder).Result;

            // Show the received variable groups
            variableGroups.ForEach((VariableGroup vg) =>
            {
                Console.WriteLine("{0} {1}", vg.Id.ToString(), vg.Name);
            });

            return variableGroups;
        }

        /// <summary>
        /// The delete a variable group.
        /// </summary>
        [ClientSampleMethod]
        public void DeleteVariableGroup()
        {
            string projectName = ClientSampleHelpers.FindAnyProject(this.Context).Name;

            // Get a task agent client instance
            VssConnection connection = Context.Connection;
            TaskAgentHttpClient taskClient = connection.GetClient<TaskAgentHttpClient>();
            
            // Delete the already created task group
            taskClient.DeleteVariableGroupAsync(project: projectName, groupId: this.addedVariableGroupId).SyncResult();
        }

    }
}
