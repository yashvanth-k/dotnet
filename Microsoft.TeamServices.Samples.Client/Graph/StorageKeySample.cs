using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Graph;
using Microsoft.VisualStudio.Services.Graph.Client;
using Microsoft.VisualStudio.Services.WebApi;
using System;

namespace Microsoft.TeamServices.Samples.Client.Graph
{
    [ClientSample(GraphResourceIds.AreaName, GraphResourceIds.StorageKeys.StorageKeysResourceName)]
    public class StorageKeySample : ClientSample
    {
        /// <summary>
        /// Get a storage key from a descriptor
        /// </summary>
        /// <returns></returns>
        [ClientSampleMethod]
        public void GetDescriptorById()
        {
            // Get the client
            VssConnection connection = Context.Connection;
            GraphHttpClient graphClient = connection.GetClient<GraphHttpClient>();

            //
            // Part 1: add the AAD user
            // 
            ClientSampleHttpLogger.SetOperationName(this.Context, "MaterializeAADUserByOIDWithVSID");
            GraphUserCreationContext addAADUserContext = new GraphUserOriginIdCreationContext
            {
                OriginId = "e97b0e7f-0a61-41ad-860c-748ec5fcb20b",
            };

            GraphUser newUser = graphClient.CreateUserAsync(addAADUserContext).Result;
            string userDescriptor = newUser.Descriptor;

            Context.Log("New user added! ID: {0}", userDescriptor);

            //
            // Part 2: get the storage key
            //
            //TODO: uncomment when the new client library is available
            /*ClientSampleHttpLogger.SetOperationName(this.Context, "GetStorageKeyBySubjectDescriptor");
            GraphStorageKey storageKey = graphClient.GetStorageKey(userDescriptor).Result;*/

            //
            // Part 3: remove the user
            // 
            graphClient.DeleteUserAsync(userDescriptor).SyncResult();

            // Try to get the deleted user
            try
            {
                GraphMembershipState membershipState = graphClient.GetMembershipStateAsync(userDescriptor).Result;
                if (membershipState.Active) throw new Exception();
            }
            catch (Exception e)
            {
                Context.Log("The deleted user is not disabled!");
            }
        }
    }
}
