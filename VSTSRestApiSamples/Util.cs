using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VstsRestApiSamples
{
    public class Util
    {
        public static HttpClient CreateConnection(IConfiguration _configuration, string _credentials)
        {
            if(String.IsNullOrEmpty(_configuration.UriString))
                throw new Exception("Please enter the Uri String [appsetting.uri]");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_configuration.UriString);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _credentials);
            return client;
        }
    }
}
