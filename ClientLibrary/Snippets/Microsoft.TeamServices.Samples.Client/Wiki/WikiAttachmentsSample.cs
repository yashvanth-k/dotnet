using Microsoft.TeamFoundation.Wiki.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Microsoft.TeamServices.Samples.Client.Wiki
{
    [ClientSample(WikiConstants.AreaName, WikiConstants.AttachmentsResourceName)]
    public class WikiAttachmentsSample : ClientSample
    {
        [ClientSampleMethod]
        public WikiAttachmentResponse AddAttachment()
        {
            VssConnection connection = this.Context.Connection;
            WikiHttpClient wikiClient = connection.GetClient<WikiHttpClient>();

            Guid projectId = ClientSampleHelpers.FindAnyProject(this.Context).Id;

            List<WikiV2> wikis = wikiClient.GetAllWikisAsync(projectId).SyncResult();

            if (wikis == null || wikis.Count == 0)
            {
                Console.WriteLine("No wikis exist to create wiki page");

                return null;
            }

            WikiV2 wiki = wikis[0];
            Stream attachmentStream = File.OpenRead(
                Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    @"Content\Logo.png"));

            WikiAttachmentResponse attachmentResponse = wikiClient.CreateAttachmentAsync(
                uploadStream: attachmentStream.ConvertToBase64(),
                project: wiki.ProjectId,
                wikiIdentifier: wiki.Id,
                name: "Attachment" + new Random().Next(0, 999) + ".png").SyncResult();

            Console.WriteLine("Attachment '{0}' added to wiki '{1}'", attachmentResponse.Attachment.Name, wiki.Name);

            return attachmentResponse;
        }
    }
}
