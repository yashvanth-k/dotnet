using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VstsClientLibrariesSamples.Tests
{
    public static class InitHelper
    {
        public static IConfiguration GetConfiguration(IConfiguration configuration)
        {
            configuration.PersonalAccessToken = ConfigurationManager.AppSettings["appsetting.pat"].ToString();
            configuration.Project = ConfigurationManager.AppSettings["appsetting.project"].ToString();
            configuration.Team = ConfigurationManager.AppSettings["appsetting.team"].ToString();
            configuration.Query = ConfigurationManager.AppSettings["appsetting.query"].ToString();
            configuration.Identity = ConfigurationManager.AppSettings["appsetting.identity"].ToString();
            configuration.UriString = ConfigurationManager.AppSettings["appsetting.uri"].ToString();   
            configuration.WorkItemIds = ConfigurationManager.AppSettings["appsetting.workitemids"].ToString();
            configuration.WorkItemId = Convert.ToInt32(ConfigurationManager.AppSettings["appsetting.workitemid"].ToString());
            configuration.FilePath = ConfigurationManager.AppSettings["appsetting.filepath"].ToString();

            return configuration;
        }
    }
}
