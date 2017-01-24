using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Common;

namespace VstsClientLibrariesSamples.Tests
{
    public static class InitHelper
    {
        public static IConfiguration GetConfiguration(IConfiguration configuration)
        {
            configuration.CollectionId = ConfigurationSettings.AppSettings["appsetting.collectionid"].ToString();
            configuration.PersonalAccessToken = ConfigurationSettings.AppSettings["appsetting.pat"].ToString();
            configuration.Project = ConfigurationSettings.AppSettings["appsetting.project"].ToString();
            configuration.Team = ConfigurationSettings.AppSettings["appsetting.team"].ToString();
            configuration.MoveToProject = ConfigurationSettings.AppSettings["appsetting.movetoproject"].ToString();
            configuration.Query = ConfigurationSettings.AppSettings["appsetting.query"].ToString();
            configuration.Identity = ConfigurationSettings.AppSettings["appsetting.identity"].ToString();
            configuration.UriString = ConfigurationSettings.AppSettings["appsetting.uri"].ToString();   
            configuration.WorkItemIds = ConfigurationSettings.AppSettings["appsetting.workitemids"].ToString();
            configuration.WorkItemId = ConfigurationSettings.AppSettings["appsetting.workitemid"].IsNullOrEmpty() ? 
                0 : 
                Convert.ToInt32(ConfigurationSettings.AppSettings["appsetting.workitemid"].ToString());
            configuration.FilePath = ConfigurationSettings.AppSettings["appsetting.filepath"].ToString();
            configuration.GitRepositoryId = ConfigurationSettings.AppSettings["appsetting.git.repositoryid"].ToString();
            configuration.GitTargetVersionBranch = ConfigurationSettings.AppSettings["appsetting.git.targetVersionBranch"].ToString();
            configuration.GitBaseVersionBranch = ConfigurationSettings.AppSettings["appsetting.git.baseVersionBranch"].ToString();

            return configuration;
        }
    }
}
