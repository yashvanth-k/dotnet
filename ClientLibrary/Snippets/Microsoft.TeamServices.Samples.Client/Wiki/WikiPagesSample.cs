using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.Wiki.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.TeamServices.Samples.Client.Wiki
{
    [ClientSample(WikiConstants.AreaName, WikiConstants.PagesResourceName)]
    public class WikiPagesSample : ClientSample
    {
        [ClientSampleMethod]
        public WikiPageResponse CreateWikiPage()
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
            WikiPageCreateOrUpdateParameters parameters = new WikiPageCreateOrUpdateParameters()
            {
                Content = "Wiki page content"
            };

            WikiPageResponse wikiPageResponse = wikiClient.CreateOrUpdatePageAsync(
                parameters,
                project: wiki.ProjectId,
                wikiIdentifier: wiki.Id,
                path: "SamplePage" + new Random().Next(1, 999),
                Version: null).SyncResult();

            Console.WriteLine("Create page '{0}' in wiki '{1}'", wikiPageResponse.Page.Path, wiki.Name);

            return wikiPageResponse;
        }

        [ClientSampleMethod]
        public WikiPageResponse GetWikiPageJson()
        {
            VssConnection connection = this.Context.Connection;
            WikiHttpClient wikiClient = connection.GetClient<WikiHttpClient>();

            Guid projectId = ClientSampleHelpers.FindAnyProject(this.Context).Id;

            List<WikiV2> wikis = wikiClient.GetAllWikisAsync(projectId).SyncResult();

            if (wikis == null || wikis.Count == 0)
            {
                Console.WriteLine("No wikis exist to get wiki page");

                return null;
            }

            WikiV2 wiki = wikis[0];
            WikiPage rootPage = wikiClient.GetPageAsync(
                project: wiki.ProjectId,
                wikiIdentifier: wiki.Name,
                path: "/",
                recursionLevel: VersionControlRecursionType.OneLevel).SyncResult().Page;

            string somePagePath = rootPage.SubPages[0].Path;
            WikiPageResponse somePageResponse = wikiClient.GetPageAsync(
                project: wiki.ProjectId,
                wikiIdentifier: wiki.Name,
                path: somePagePath).SyncResult();

            Console.WriteLine("Retrieved page '{0}' as JSON in wiki '{1}'", somePagePath, wiki.Name);

            return somePageResponse;
        }

        [ClientSampleMethod]
        public WikiPageResponse GetWikiPageJsonWithContent()
        {
            VssConnection connection = this.Context.Connection;
            WikiHttpClient wikiClient = connection.GetClient<WikiHttpClient>();

            Guid projectId = ClientSampleHelpers.FindAnyProject(this.Context).Id;

            List<WikiV2> wikis = wikiClient.GetAllWikisAsync(projectId).SyncResult();

            if (wikis == null || wikis.Count == 0)
            {
                Console.WriteLine("No wikis exist to get wiki page");

                return null;
            }

            WikiV2 wiki = wikis[0];
            WikiPage rootPage = wikiClient.GetPageAsync(
                project: wiki.ProjectId,
                wikiIdentifier: wiki.Id,
                path: "/",
                recursionLevel: VersionControlRecursionType.OneLevel).SyncResult().Page;

            string somePagePath = rootPage.SubPages[0].Path;
            WikiPageResponse WikiPageResponse = wikiClient.GetPageAsync(
                project: wiki.ProjectId,
                wikiIdentifier: wiki.Id,
                path: somePagePath,
                includeContent: true).SyncResult();

            Console.WriteLine("Retrieved page '{0}' as JSON in wiki '{1}' with content '{2}'", WikiPageResponse.Page.Path, wiki.Name, WikiPageResponse.Page.Content);

            return WikiPageResponse;
        }

        [ClientSampleMethod]
        public WikiPageResponse GetWikiPageAndSubPages()
        {
            VssConnection connection = this.Context.Connection;
            WikiHttpClient wikiClient = connection.GetClient<WikiHttpClient>();

            Guid projectId = ClientSampleHelpers.FindAnyProject(this.Context).Id;

            List<WikiV2> wikis = wikiClient.GetAllWikisAsync(projectId).SyncResult();

            if (wikis == null || wikis.Count == 0)
            {
                Console.WriteLine("No wikis exist to get wiki page");

                return null;
            }

            WikiV2 wiki = wikis[0];
            WikiPageResponse rootPageResponse = wikiClient.GetPageAsync(
                project: wiki.ProjectId,
                wikiIdentifier: wiki.Name,
                path: "/",
                recursionLevel: VersionControlRecursionType.OneLevel).SyncResult();

            Console.WriteLine("Retrieved the following subpages for the root page:");
            foreach (WikiPage subPage in rootPageResponse.Page.SubPages)
            {
                Console.WriteLine("Sub-page : '{0}'", subPage.Path);
            }

            return rootPageResponse;
        }

        [ClientSampleMethod]
        public string GetWikiPageText()
        {
            VssConnection connection = this.Context.Connection;
            WikiHttpClient wikiClient = connection.GetClient<WikiHttpClient>();

            Guid projectId = ClientSampleHelpers.FindAnyProject(this.Context).Id;

            List<WikiV2> wikis = wikiClient.GetAllWikisAsync(projectId).SyncResult();

            if (wikis == null || wikis.Count == 0)
            {
                Console.WriteLine("No wikis exist to get wiki page");

                return null;
            }

            WikiV2 wiki = wikis[0];
            WikiPage rootPage = wikiClient.GetPageAsync(
                project: wiki.ProjectId,
                wikiIdentifier: wiki.Id,
                path: "/",
                recursionLevel: VersionControlRecursionType.OneLevel).SyncResult().Page;

            string somePagePath = rootPage.SubPages[0].Path;
            
            using (var reader = new StreamReader(wikiClient.GetPageTextAsync(
                project: wiki.ProjectId,
                wikiIdentifier: wiki.Id,
                path: somePagePath).SyncResult()))
            {
                string pageContent = reader.ReadToEnd();
                Console.WriteLine("Retrieved page '{0}' in wiki '{1}' with content '{2}'", somePagePath, wiki.Name, pageContent);

                return pageContent;
            }
        }

        [ClientSampleMethod]
        public WikiPageResponse EditWikiPage()
        {
            VssConnection connection = this.Context.Connection;
            WikiHttpClient wikiClient = connection.GetClient<WikiHttpClient>();

            Guid projectId = ClientSampleHelpers.FindAnyProject(this.Context).Id;

            List<WikiV2> wikis = wikiClient.GetAllWikisAsync(projectId).SyncResult();

            if (wikis == null || wikis.Count == 0)
            {
                Console.WriteLine("No wikis exist to get wiki page");

                return null;
            }

            WikiV2 wiki = wikis[0];
            WikiPage rootPage = wikiClient.GetPageAsync(
                project: wiki.ProjectId,
                wikiIdentifier: wiki.Name,
                path: "/",
                recursionLevel: VersionControlRecursionType.OneLevel).SyncResult().Page;

            string somePagePath = rootPage.SubPages[0].Path;
            WikiPageResponse pageResponse = wikiClient.GetPageAsync(
                project: wiki.ProjectId,
                wikiIdentifier: wiki.Name,
                path: somePagePath,
                includeContent: true).SyncResult();

            WikiPage somePage = pageResponse.Page;

            Console.WriteLine("Retrieved page '{0}' as JSON in wiki '{1}' with content '{2}'", somePage.Path, wiki.Name, somePage.Content);

            var originalContent = somePage.Content;
            var originalVersion = pageResponse.ETag.ToList()[0];

            WikiPageCreateOrUpdateParameters parameters = new WikiPageCreateOrUpdateParameters()
            {
                Content = "New content for page"
            };

            WikiPageResponse editedPageResponse = wikiClient.CreateOrUpdatePageAsync(
                parameters: parameters,
                project: wiki.ProjectId,
                wikiIdentifier: wiki.Name,
                path: somePagePath,
                Version: originalVersion).SyncResult();

            var updatedContent = editedPageResponse.Page.Content;
            var updatedVersion = editedPageResponse.ETag.ToList()[0];

            Console.WriteLine("Before editing --> Page path: {0}, version: {1}, content: {2}", somePage.Path, originalVersion, originalContent);
            Console.WriteLine("After editing --> Page path: {0}, version: {1}, content: {2}", somePage.Path, updatedVersion, updatedContent);

            return editedPageResponse;
        }

        [ClientSampleMethod]
        public WikiPageResponse DeleteWikiPage()
        {
            VssConnection connection = this.Context.Connection;
            WikiHttpClient wikiClient = connection.GetClient<WikiHttpClient>();

            Guid projectId = ClientSampleHelpers.FindAnyProject(this.Context).Id;

            List<WikiV2> wikis = wikiClient.GetAllWikisAsync(projectId).SyncResult();

            if (wikis == null || wikis.Count == 0)
            {
                Console.WriteLine("No wikis exist to get wiki page");

                return null;
            }

            WikiV2 wiki = wikis[0];
            WikiPage rootPage = wikiClient.GetPageAsync(
                project: wiki.ProjectId,
                wikiIdentifier: wiki.Id,
                path: "/",
                recursionLevel: VersionControlRecursionType.OneLevel).SyncResult().Page;

            string somePagePath = rootPage.SubPages[0].Path;
            WikiPageResponse somePageResponse = wikiClient.DeletePageAsync(
                project: wiki.ProjectId,
                wikiIdentifier: wiki.Id,
                path: somePagePath).SyncResult();

            Console.WriteLine("Deleted page '{0}' from wiki '{1}'", somePagePath, wiki.Name);

            return somePageResponse;
        }
    }
}
