using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VstsRestApiSamples.ViewModels.Git;

namespace VstsRestApiSamples.Git
{
    public class GitRepository
    {
        readonly IConfiguration _configuration;
        readonly string _credentials;

        public GitRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            if (String.IsNullOrEmpty(_configuration.PersonalAccessToken))
                throw new Exception("Please enter the Personal Access Token [appsetting.pat]");

            _credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", _configuration.PersonalAccessToken)));
        }

        public GetAllRepositoriesResponse.Repositories GetAllRepositories()
        {
            GetAllRepositoriesResponse.Repositories viewModel = new GetAllRepositoriesResponse.Repositories();

            using (var client = Util.CreateConnection(_configuration, _credentials))
            {
                HttpResponseMessage response = client.GetAsync("/_apis/git/repositories/?api-version=2.0").Result;

                if (response.IsSuccessStatusCode)
                {
                    viewModel = response.Content.ReadAsAsync<GetAllRepositoriesResponse.Repositories>().Result;
                }

                viewModel.HttpStatusCode = response.StatusCode;

                return viewModel;
            }
        }

        public GetRepositoryByIdResponse.Repository GetRepositoryById(string repositoryId)
        {
            GetRepositoryByIdResponse.Repository viewModel = new GetRepositoryByIdResponse.Repository();

            using (var client = Util.CreateConnection(_configuration, _credentials))
            {
                HttpResponseMessage response = client.GetAsync("/_apis/git/repositories/" + repositoryId + "?api-version=2.0").Result;

                if (response.IsSuccessStatusCode)
                {
                    viewModel = response.Content.ReadAsAsync<GetRepositoryByIdResponse.Repository>().Result;
                }

                viewModel.HttpStatusCode = response.StatusCode;

                return viewModel;
            }
        }

        public GetFolderAndChildrenResponse.FolderAndChildren GetFolderAndChildren(string repositoryId, string scopePath)
        {
            GetFolderAndChildrenResponse.FolderAndChildren viewModel = new GetFolderAndChildrenResponse.FolderAndChildren();

            using (var client = Util.CreateConnection(_configuration, _credentials))
            {
                HttpResponseMessage response = client.GetAsync("/_apis/git/repositories/" + repositoryId + "/items?scopePath=" + scopePath + "&recursionLevel=Full&includeContentMetadata=true&api-version=2.0").Result;

                if (response.IsSuccessStatusCode)
                {
                    viewModel = response.Content.ReadAsAsync<GetFolderAndChildrenResponse.FolderAndChildren>().Result;
                }

                viewModel.HttpStatusCode = response.StatusCode;

                return viewModel;
            }
        }

        public GetCommitsByRepositoryIdResponse.Commits GetCommitsByRepositoryId(string repositoryId)
        {
            GetCommitsByRepositoryIdResponse.Commits viewModel = new GetCommitsByRepositoryIdResponse.Commits();

            using (var client = Util.CreateConnection(_configuration, _credentials))
            {
                HttpResponseMessage response = client.GetAsync("/_apis/git/repositories/" + repositoryId + "/commits?api-version=2.0").Result;

                if (response.IsSuccessStatusCode)
                {
                    viewModel = response.Content.ReadAsAsync<GetCommitsByRepositoryIdResponse.Commits>().Result;
                }

                viewModel.HttpStatusCode = response.StatusCode;

                return viewModel;
            }
        }
    }
}