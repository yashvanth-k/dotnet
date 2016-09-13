using System;
using System.Net.Http;
using System.Net.Http.Headers;
using VstsRestApiSamples.ViewModels.Build;

namespace VstsRestApiSamples.Build2
{
    public class Build
    {
        readonly IConfiguration _configuration;
        readonly string _credentials;  

        public Build(IConfiguration configuration)
        {
            _configuration = configuration;

            if (String.IsNullOrEmpty(_configuration.PersonalAccessToken))
                throw new Exception("Please enter the Personal Access Token [appsetting.pat]");

            _credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", _configuration.PersonalAccessToken)));
        }

        public BuildGetListofBuildDefinitionsResponse.Definitions GetListOfBuildDefinitions(string project)
        {
            BuildGetListofBuildDefinitionsResponse.Definitions viewModel = new BuildGetListofBuildDefinitionsResponse.Definitions();

            //using (var client = new HttpClient())
            using (var client = Util.CreateConnection(_configuration, _credentials))
            {
                //client.BaseAddress = new Uri(_configuration.UriString);
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _credentials);

                HttpResponseMessage response = client.GetAsync(project + "/_apis/build/definitions?api-version=2.0").Result;

                if (response.IsSuccessStatusCode)
                {
                    viewModel = response.Content.ReadAsAsync<BuildGetListofBuildDefinitionsResponse.Definitions>().Result;
                }

                viewModel.HttpStatusCode = response.StatusCode;

                return viewModel;
            }
        }
    }
}
