using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.Graph.Client;
using System.Collections.Generic;
using Microsoft.VisualStudio.Services.Common;
using Newtonsoft.Json;

namespace DualGraphGroupMembershipSample
{
    class Program
    {
        //============= Config [Edit these with your settings] =====================
        internal const string vstsCollectionUrl = "https://peaky.fstest10.tfsallin.net"; //"https://myaccount.visualstudio.com";  //change to the URL of your VSTS account; NOTE: This must use HTTPS
        internal const string clientId = "872cd9fa-d31f-45e0-9eab-6e460a02d1f1";//"xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";         //change to your app registration's Application ID
        internal const string replyUri = "urn:ietf:wg:oauth:2.0:oob";                           //change to your app registration's reply URI
        internal const string groupDisplayName = "test1";//"vsts group display name";              //change to the group you want to get all users principal names
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

                

                //connect to graph API with ADAL access token
                VssConnection connection = new VssConnection(new Uri(vstsCollectionUrl), new VssOAuthCredential(result.AccessToken));
                GraphHttpClient graphClient = connection.GetClient<GraphHttpClient>();

                //get the VSTS group
                GraphGroup group = GetGraphGroup(graphClient, groupDisplayName);

                //Loop though VSTS group and reutrn all nested users and AAD groups
                Tuple<List<string>, List<string>> usersAndAadGroups = GetNestedUsersAndAadGroups(graphClient, group);
                List<string> users = usersAndAadGroups.Item1;
                List<string> aadGroupIds = usersAndAadGroups.Item2;

                //exchange VSTS token for Microsoft graph token
                //AuthenticationResult graphAuthResult = ctx.AcquireTokenAsync(graphResourceId, clientId, new Uri(replyUri), new PlatformParameters(PromptBehavior.Auto)).Result;

                //use Microsoft graph token to return all users in AAD Groups
                //List<string> aadUsers = GetNestedAadGroupUsers(graphAuthResult.AccessToken, aadGroupIds);

                //Consolidate vsts and aad user lists and print to console
                //users.AddRange(aadUsers);
                //foreach (string userPrincipalName in users)
                //{
                //    Console.WriteLine(userPrincipalName);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: {1}", ex.GetType(), ex.Message);
            }
        }

        //================ VSTS graph api helper code ===========================================================

        private static GraphGroup GetGraphGroup(GraphHttpClient graphClient, string groupDisplayname)
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

        private static Tuple<List<string>, List<string>> GetNestedUsersAndAadGroups(GraphHttpClient graphClient, GraphGroup group)
        {
            //List of user principle names
            List<string> users = new List<string>();
            //List of Graph subjects for AAD groups
            List<string> aadGroups = new List<string>();

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
                    // member is a user
                    //switch (member.Descriptor.SubjectType)
                    //{
                    //    case "user":
                    //        users.Add(((GraphUser)member).PrincipalName);
                    //        break;
                    //    case 

                    //}
                    if (member.SubjectKind == "user")
                    {
                        users.Add(((GraphUser) member).PrincipalName);
                    }
                    // member is an AAD group
                    else if (member.SubjectKind == "group" && member.Origin == "aad")
                    {
                        aadGroups.Add(member.OriginId);
                    }
                    // member is a VSTS group
                    else if (member.SubjectKind == "group" && member.Origin == "vsts")
                    {
                        memberships.AddRange(graphClient.GetMembershipsAsync(member.Descriptor,Microsoft.VisualStudio.Services.Graph.GraphTraversalDirection.Down).Result);
                    }
                    // should never get here
                    else
                    {
                        throw new Exception("shouldn't be here");
                    }
                }
            }
            return new Tuple<List<string>, List<string>>(users, aadGroups);
        }

        private static AuthenticationContext GetAuthenticationContext(string tenant)
        {
            AuthenticationContext ctx = null;
            if (tenant != null)
                ctx = new AuthenticationContext("https://login.windows-ppe.net/" + tenant);//"https://login.microsoftonline.com/" + tenant);
            else
            {
                ctx = new AuthenticationContext("https://login.windows-ppe.net/common");//"https://login.windows.net/common");
                if (ctx.TokenCache.Count > 0)
                {
                    string homeTenant = ctx.TokenCache.ReadItems().First().TenantId;
                    ctx = new AuthenticationContext("https://login.windows-ppe.net/" + homeTenant);//"https://login.microsoftonline.com/" + homeTenant);
                }
            }

            return ctx;
        }

        //================ Microsoft graph api helper code ===========================================================

        private static List<string> GetNestedAadGroupUsers(string accessToken, List<string> aadGroupIds)
        {
            //List of aad user principal names. Will be added to VSTS user list
            List<string> aadUsers = new List<string>();

            //
            List<AadGroupMember> aadMembers = new List<AadGroupMember>();
            foreach (string id in aadGroupIds)
            {
                aadMembers.AddRange(getAadGroupMembers(accessToken, id));
            }
            while (aadMembers.Count != 0)
            {
                List<AadGroupMember> nestedAadgroups = new List<AadGroupMember>();
                foreach (var aadMember in aadMembers)
                {
                    if (aadMember.type == "#microsoft.graph.user")
                    {
                        aadUsers.Add(aadMember.userPrincipalName);
                    }
                    else if (aadMember.type == "#microsoft.graph.group")
                    {
                        nestedAadgroups.AddRange(getAadGroupMembers(accessToken, aadMember.id));
                    }
                    else
                    {
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
    }

    // for Json deserialization
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
