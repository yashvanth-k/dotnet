using System.Configuration;

namespace VstsRestApiSamples.Tests
{
    public static class InitHelper
    {
        public static IConfiguration GetConfiguration(IConfiguration configuration)
        {
            configuration.PersonalAccessToken = ConfigurationManager.AppSettings["appsetting.pat"].ToString();
            configuration.Project = ConfigurationManager.AppSettings["appsetting.project"].ToString();
            configuration.MoveToProject = ConfigurationManager.AppSettings["appsetting.movetoproject"].ToString();
            configuration.Query = ConfigurationManager.AppSettings["appsetting.query"].ToString();
            configuration.Identity = ConfigurationManager.AppSettings["appsetting.identity"].ToString();
            configuration.UriString = ConfigurationManager.AppSettings["appsetting.uri"].ToString();   
            configuration.WorkItemIds = ConfigurationManager.AppSettings["appsetting.workitemids"].ToString();
            configuration.WorkItemId = ConfigurationManager.AppSettings["appsetting.workitemid"].ToString();
            configuration.ProcessId = ConfigurationManager.AppSettings["appsetting.processid"].ToString();
            configuration.PickListId = ConfigurationManager.AppSettings["appsetting.picklistid"].ToString();
            configuration.QueryId = ConfigurationManager.AppSettings["appsetting.queryid"].ToString();

            return configuration;
        }
    }
}
