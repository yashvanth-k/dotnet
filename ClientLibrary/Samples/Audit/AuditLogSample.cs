using System;
using System.Linq;
using Microsoft.VisualStudio.Services.Audit.WebApi;
using Microsoft.VisualStudio.Services.WebApi;

namespace Microsoft.Azure.DevOps.ClientSamples.Audit
{
    [ClientSample(ResourceLocationIds.AuditAreaName, ResourceLocationIds.AuditLogResourceName)]
    public class AuditLogSample : ClientSample
    {
        [ClientSampleMethod]
        public void QueryLogSample()
        {
            try
            {
                // Get the audit log client
                VssConnection connection = Context.Connection;
                AuditHttpClient auditClient = connection.GetClient<AuditHttpClient>();

                // Query the audit log for the last day, take only 10 entries
                DateTime now = DateTime.UtcNow;
                AuditLogQueryResult logQueryResult = auditClient
                    .QueryLogAsync(startTime: now.AddDays(-1), endTime: now, batchSize: 10, continuationToken: null)
                    .SyncResult();

                Context.Log($"ContinuationToken:            {logQueryResult.ContinuationToken}");
                Context.Log($"HasMore:                      {logQueryResult.HasMore}");
                Context.Log($"DecoratedAuditLogEntries:     {logQueryResult.DecoratedAuditLogEntries.Count()}");
            }
            catch (Exception e)
            {
                Context.Log($"err: {e}");
            }
        }
    }
}
