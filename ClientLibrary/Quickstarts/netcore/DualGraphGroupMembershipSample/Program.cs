using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.Graph.Client;
using System.Collections.Generic;
using Microsoft.VisualStudio.Services.Common;
using Newtonsoft.Json;
using Microsoft.VisualStudio.Services.Graph;
using Microsoft.VisualStudio.Services.OAuth;

namespace DualGraphGroupMembershipSample
{
    class Program
    {
        //============= Config [Edit these with your settings] =====================
        internal const string vstsCollectionUrl = "https://myaccount.visualstudio.com";  //change to the URL of your VSTS account; NOTE: This must use HTTPS
        internal const string clientId = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";         //change to your app registration's Application ID
        internal const string replyUri = "adalsample";                                   //change to your app registration's reply URI
        internal const string groupDisplayName = "vsts group display name";              //change to the group you want to get all users principal names
        //==========================================================================

        internal const string VstsResourceId = "499b84ac-1321-427f-aa17-267ca6975798"; //Constant value to target VSTS. Do not change
        internal const string graphResourceId = "https://graph.microsoft.com";         //Constant value to target Microsoft Graph API. Do not change

        public static void Main(string[] args)
        {
            AuthenticationContext ctx = GetAuthenticationContext(null);
            AuthenticationResult result = null;
            try
            {
                //PromptBehavior.RefreshSession will enforce an authn prompt every time. NOTE: Auto will take your windows login state if possible
                result = ctx.AcquireTokenAsync(VstsResourceId, clientId, new Uri(replyUri), new PlatformParameters(PromptBehavior.Always)).Result;

                //connect to VSTS Graph API with ADAL access token
                VssConnection connection = new VssConnection(new Uri(vstsCollectionUrl), new VssOAuthAccessTokenCredential(result.AccessToken));
                GraphHttpClient graphClient = connection.GetClient<GraphHttpClient>();

                //get the VSTS group
                GraphGroup group = GetGraphGroupFromString(graphClient, groupDisplayName);

                //Loop through VSTS group and return all nested users and AAD groups underneath "groupDisplayName" group
                Tuple<List<GraphUser>, List<GraphGroup>> usersAndAadGroups = ExpandVstsGroup(graphClient, group);
                List<GraphUser> vstsGroupUsers = usersAndAadGroups.Item1;
                List<GraphGroup> aadGroups = usersAndAadGroups.Item2;

                //exchange VSTS token for Microsoft graph token
                AuthenticationResult graphAuthResult = ctx.AcquireTokenAsync(graphResourceId, clientId, new Uri(replyUri), new PlatformParameters(PromptBehavior.Auto)).Result;

                //use Microsoft graph token to return all users in AAD Groups
                List<AadGroupMember> aadGroupUsers = ExpandAadGroups(graphAuthResult.AccessToken, aadGroups);

                //print vsts and aad user lists to console
                foreach (GraphUser vstsGroupUser in vstsGroupUsers)
                {
                    Console.WriteLine(vstsGroupUser.PrincipalName);
                }
                foreach (AadGroupMember aadGroupUser in aadGroupUsers)
                {
                    Console.WriteLine(aadGroupUser.userPrincipalName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: {1}", ex.GetType(), ex.Message);
            }
        }

        //================ VSTS graph api helper code ===========================================================

        private static GraphGroup GetGraphGroupFromString(GraphHttpClient graphClient, string groupDisplayname)
        {
            PagedGraphGroups groups = graphClient.GetGroupsAsync().Result;

            GraphGroup selectedGroup = null;
            foreach (var group in groups.GraphGroups)
            {
                if (group.DisplayName.Equals(groupDisplayName))
                {
                    return selectedGroup = group;
                }
            }
            return null;
        }

        private static Tuple<List<GraphUser>, List<GraphGroup>> ExpandVstsGroup(GraphHttpClient graphClient, GraphGroup group)
        {
            //List of user principle names
            List<GraphUser> users = new List<GraphUser>();
            //List of Graph subjects for AAD groups
            List<GraphGroup> aadGroups = new List<GraphGroup>();

            List<GraphMembership> memberships = graphClient.GetMembershipsAsync(group.Descriptor, Microsoft.VisualStudio.Services.Graph.GraphTraversalDirection.Down).Result;
            while (memberships.Count != 0)
            {
                List<GraphSubjectLookupKey> lookupKeys = new List<GraphSubjectLookupKey>();
                foreach (var membership in memberships)
                {
                    lookupKeys.Add(new GraphSubjectLookupKey(membership.MemberDescriptor));
                }
                memberships.Clear();
                IReadOnlyDictionary<SubjectDescriptor, GraphSubject> memberDict = graphClient.LookupSubjectsAsync(new GraphSubjectLookup(lookupKeys)).Result;
                foreach (GraphSubject member in memberDict.Values)
                {
                    switch (member.Descriptor.SubjectType)
                    {
                        //member is an AAD user
                        case Constants.SubjectType.AadUser:
                            users.Add((GraphUser)member);
                            break;
                        //member is an MSA user
                        case Constants.SubjectType.MsaUser:
                            users.Add((GraphUser)member);
                            break;
                        //member is a nested AAD group
                        case Constants.SubjectType.AadGroup:
                            aadGroups.Add((GraphGroup)member);
                            break;
                        //member is a nested VSTS group
                        case Constants.SubjectType.VstsGroup:
                            memberships.AddRange(graphClient.GetMembershipsAsync(member.Descriptor, Microsoft.VisualStudio.Services.Graph.GraphTraversalDirection.Down).Result);
                            break;
                        default:
                            throw new Exception("shouldn't be here");
                    }
                }
            }
            return new Tuple<List<GraphUser>, List<GraphGroup>>(users, aadGroups);
        }

        //================ Microsoft graph api helper code ===========================================================

        private static List<AadGroupMember> ExpandAadGroups(string accessToken, List<GraphGroup> aadGroups)
        {
            //List of users in an AAD group
            List<AadGroupMember> aadUsers = new List<AadGroupMember>();

            //Getting all members in all groups and nesteed groups
            List<AadGroupMember> aadMembers = new List<AadGroupMember>();
            foreach (GraphGroup aadGroup in aadGroups)
            {
                aadMembers.AddRange(getAadGroupMembers(accessToken, aadGroup.OriginId));
            }
            while (aadMembers.Count != 0)
            {
                List<AadGroupMember> nestedAadgroups = new List<AadGroupMember>();
                foreach (var aadMember in aadMembers)
                {
                    switch (aadMember.type)
                    {
                        //member is a user
                        case "#microsoft.graph.user":
                            aadUsers.Add(aadMember);
                            break;
                        //member is a nested AAD group
                        case "#microsoft.graph.group":
                            nestedAadgroups.AddRange(getAadGroupMembers(accessToken, aadMember.id));
                            break;
                        default:
                            throw new Exception("shouldn't be here");
                    }
                }
                aadMembers.Clear();
                aadMembers.AddRange(nestedAadgroups);
            }
            return aadUsers;
        }

        private static List<AadGroupMember> getAadGroupMembers(string accessToken, string aadGroupId)
        {
            AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", accessToken);
            // use the httpclient
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://graph.microsoft.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "GraphGroupMembershipSample");
                client.DefaultRequestHeaders.Add("X-TFS-FedAuthRedirect", "Suppress");
                client.DefaultRequestHeaders.Authorization = authHeader;

                // connect to the REST endpoint            
                HttpResponseMessage response = client.GetAsync("v1.0/groups/" + aadGroupId + "/members").Result;

                // check to see if we have a succesfull respond
                if (response.IsSuccessStatusCode)
                {
                    string responseJsonStr = response.Content.ReadAsStringAsync().Result;
                    AadGroupMembers groupMembers = JsonConvert.DeserializeObject<AadGroupMembers>(responseJsonStr);
                    return groupMembers.members;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        //================ Other helper code ===========================================================

        private static AuthenticationContext GetAuthenticationContext(string tenant)
        {
            AuthenticationContext ctx = null;
            if (tenant != null)
                ctx = new AuthenticationContext("https://login.microsoftonline.com/" + tenant);
            else
            {
                ctx = new AuthenticationContext("https://login.windows.net/common");
                if (ctx.TokenCache.Count > 0)
                {
                    string homeTenant = ctx.TokenCache.ReadItems().First().TenantId;
                    ctx = new AuthenticationContext("https://login.microsoftonline.com/" + homeTenant);
                }
            }
            return ctx;
        }
    }

    //=================================================================================================
    //================ Json deserialization ===========================================================
    //=================================================================================================
    public class AadGroupMembers
    {
        [JsonProperty("@odata.context")]
        public string groupType { get; set; }
        [JsonProperty("value")]
        public List<AadGroupMember> members { get; set; }
    }
    public class AadGroupMember
    {
        [JsonProperty("@odata.type")]
        public string type { get; set; }
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("businessPhones")]
        public List<string> businessPhones { get; set; }
        [JsonProperty("displayName")]
        public string displayName { get; set; }
        [JsonProperty("givenName")]
        public string givenName { get; set; }
        [JsonProperty("jobTitle")]
        public string jobTitle { get; set; }
        [JsonProperty("mail")]
        public string mail { get; set; }
        [JsonProperty("mobilePhone")]
        public string mobilePhone { get; set; }
        [JsonProperty("officeLocation")]
        public string officeLocation { get; set; }
        [JsonProperty("preferredLanguage")]
        public string preferredLanguage { get; set; }
        [JsonProperty("surname")]
        public string surname { get; set; }
        [JsonProperty("userPrincipalName")]
        public string userPrincipalName { get; set; }
    }
}
