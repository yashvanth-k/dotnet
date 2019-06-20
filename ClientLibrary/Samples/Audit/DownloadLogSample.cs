using System.IO;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.Audit.WebApi;

namespace Microsoft.Azure.DevOps.ClientSamples.Audit
{
    [ClientSample(ResourceLocationIds.AuditAreaName, ResourceLocationIds.AuditLogDownloadResourceName)]
    public class DownloadLogSample : ClientSample
    {
        [ClientSampleMethod]
        public void DownloadSample()
        {
            // Get the audit log client
            VssConnection connection = Context.Connection;
            AuditHttpClient auditClient = connection.GetClient<AuditHttpClient>();

            // Download the log to a file
            foreach (string format in new[] { "json", "csv" })
            {
                string fileName = $"{Path.GetTempFileName()}.{format}";
                using (FileStream fileStream = File.Create(fileName))
                using (Stream logStream = auditClient.DownloadLogAsync(format).SyncResult())
                {
                    logStream.CopyTo(fileStream);
                }
                Context.Log($"Log downloaded to {fileName}");
            }
        }
    }
}
