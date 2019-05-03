using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.FormInput;
using Microsoft.VisualStudio.Services.ServiceEndpoints.WebApi;
using Microsoft.VisualStudio.Services.WebApi;

namespace Microsoft.Azure.DevOps.ClientSamples.Serviceendpoint
{
    /// <summary>
    /// 
    /// Samples for interacting with Azure DevOps service endpoints (also known as service connections)
    ///
    /// Package: https://www.nuget.org/packages/Microsoft.VisualStudio.Services.ServiceEndpoints.WebApi
    /// 
    /// </summary>
    [ClientSample(ServiceEndpointResourceIds.AreaName, ServiceEndpointResourceIds.EndpointResource.Name)]
    public class EndpointsSample : ClientSample
    {
        [ClientSampleMethod]
        public IEnumerable<ServiceEndpointType> ListEndpointTypes()
        {
            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient endpointClient = connection.GetClient<ServiceEndpointHttpClient>();

            // Get a list of all available service endpoint types
            List<ServiceEndpointType> types = endpointClient.GetServiceEndpointTypesAsync().Result;

            // Show details about each type in the console
            foreach(ServiceEndpointType t in types)
            {
                Console.WriteLine(t.Name);

                Console.WriteLine("Inputs:");
                foreach (InputDescriptor input in t.InputDescriptors)
                {
                    Console.WriteLine("- {0}", input.Id);
                }

                Console.WriteLine("Schemes:");
                foreach (ServiceEndpointAuthenticationScheme scheme in t.AuthenticationSchemes)
                {
                    Console.WriteLine("- {0}", scheme.Scheme);
                    Console.WriteLine("  Inputs:");
                    foreach (InputDescriptor input in scheme.InputDescriptors)
                    {
                        Console.WriteLine("  - {0}", input.Id);
                    }
                }

                Console.WriteLine("================================");
            }

            return types;
        }

        [ClientSampleMethod]
        public ServiceEndpoint CreateGenericEndpoint()
        {
            TeamProjectReference project = ClientSampleHelpers.FindAnyProject(this.Context);

            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient endpointClient = connection.GetClient<ServiceEndpointHttpClient>();
           
            // Create a generic service endpoint 
            ServiceEndpoint endpoint = endpointClient.CreateServiceEndpointAsync(project.Id, new ServiceEndpoint()
            {
                Name = "MyServiceEndpoint",
                Type = ServiceEndpointTypes.Generic,
                Url = new Uri("https://myserver"),
                Authorization = new EndpointAuthorization()
                {
                    Scheme = EndpointAuthorizationSchemes.UsernamePassword,
                    Parameters = new Dictionary<string, string>()
                    {
                        { "username", "myusername" },
                        { "password", "mysecretpassword" }
                    }
                }
            }).Result;

            Console.WriteLine("Created endpoint: {0} {1} in {2}", endpoint.Id, endpoint.Name, project.Name);

            return endpoint;
        }

        [ClientSampleMethod]
        public IEnumerable<ServiceEndpoint> ListEndpoints()
        {
            TeamProjectReference project = ClientSampleHelpers.FindAnyProject(this.Context);

            // Get a service endpoint client instance
            VssConnection connection = Context.Connection;
            ServiceEndpointHttpClient endpointClient = connection.GetClient<ServiceEndpointHttpClient>();

            // Get a list of all "generic" service endpoints in the specified project
            List<ServiceEndpoint> endpoints = endpointClient.GetServiceEndpointsAsync(
                project: project.Id,
                type: ServiceEndpointTypes.Generic).Result;

            // Show the endpoints
            Console.WriteLine("Endpoints in project: {0}", project.Name);
            foreach (ServiceEndpoint endpoint in endpoints)
            {
                Console.WriteLine("- {0} {1}", endpoint.Id.ToString().PadLeft(6), endpoint.Name);
            }

            return endpoints;           
        }
    }
}

