using System;
using System.Net.Http;
using System.Net.Http.Headers;
using VstsRestApiSamples.ViewModels.ProjectsAndTeams;

namespace VstsRestApiSamples.ProjectsAndTeams
{
    public class ProjectCollections
    {
        readonly IConfiguration _configuration;
        readonly string _credentials;

        public ProjectCollections(IConfiguration configuration)
        {
            _configuration = configuration;
            _credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", _configuration.PersonalAccessToken)));
        }

        public ListofProjectCollectionsResponse.ProjectCollections GetProjectCollections()
        {
            // create a viewmodel that is a class that represents the returned json response
            ListofProjectCollectionsResponse.ProjectCollections viewModel = new ListofProjectCollectionsResponse.ProjectCollections();

            // use the httpclient
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.UriString);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _credentials);

                // connect to the REST endpoint            
                HttpResponseMessage response = client.GetAsync("_apis/projectcollections?stateFilter=All&api-version=2.2").Result;

                // check to see if we have a succesfull respond
                if (response.IsSuccessStatusCode)
                {
                    // set the viewmodel from the content in the response
                    viewModel = response.Content.ReadAsAsync<ListofProjectCollectionsResponse.ProjectCollections>().Result;                   
                }

                viewModel.HttpStatusCode = response.StatusCode;

                return viewModel;
            }

        }
        // / <summary>
        // / Get a Project Collection by Id
        // / </summary>
        // / <param name="projectCollectionId"></param>
        // / <returns>GetProjectCollectionResponse.ProjectCollection</returns>
        public GetProjectCollectionResponse.ProjectCollection GetProjectCollection(string projectCollectionId)
        {
            GetProjectCollectionResponse.ProjectCollection viewModel = new GetProjectCollectionResponse.ProjectCollection();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.UriString);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _credentials);

                HttpResponseMessage response = client.GetAsync("_apis/projectcollections/" + projectCollectionId + "?api-version=2.2").Result;

                if (response.IsSuccessStatusCode)
                {
                    viewModel = response.Content.ReadAsAsync<GetProjectCollectionResponse.ProjectCollection>().Result;
                }

                viewModel.HttpStatusCode = response.StatusCode;

                return viewModel;
            }
        }
    }
}
