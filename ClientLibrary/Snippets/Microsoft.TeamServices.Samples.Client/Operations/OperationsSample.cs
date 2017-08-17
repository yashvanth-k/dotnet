using System;
using Microsoft.VisualStudio.Services.Notifications.WebApi;
using Microsoft.VisualStudio.Services.Operations;
using Microsoft.VisualStudio.Services.WebApi;

namespace Microsoft.TeamServices.Samples.Client.Operations
{
    /// <summary>
    /// 
    /// Sample showing how to use the Operation client to get the status of an operation.
    /// 
    /// For more details, see https://www.visualstudio.com/docs/integrate/api/operations/operations
    /// 
    /// </summary>
    [ClientSample(OperationsResourceIds.AreaName, OperationsResourceIds.OperationsResource)]
    public class OperationsSample : ClientSample
    {
        /// <summary>
        /// Returns the operation for a known job (Team Foundation Server Cleanup).
        /// </summary>
        /// <returns></returns>
        [ClientSampleMethod]
        public Operation GetOperation()
        {
            VssConnection connection = Context.Connection;
            OperationsHttpClient operationsClient = connection.GetClient<OperationsHttpClient>();

            Operation operation = operationsClient.GetOperation(Guid.Parse("B19DDD28-9A95-42E2-9697-966FD822F1CD")).Result;

            return operation;
        }
    }
}
