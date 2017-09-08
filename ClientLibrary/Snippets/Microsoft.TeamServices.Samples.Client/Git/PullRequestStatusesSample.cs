using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;

namespace Microsoft.TeamServices.Samples.Client.Git
{
    [ClientSample(GitWebApiConstants.AreaName, "pullRequestStatuses")]
    public class PullRequestStatusesSample : ClientSample
    {
        [ClientSampleMethod]
        public GitPullRequestStatus CreatePullRequestStatus()
        {
            VssConnection connection = this.Context.Connection;
            GitHttpClient gitClient = connection.GetClient<GitHttpClient>();

            TeamProjectReference project = ClientSampleHelpers.FindAnyProject(this.Context);
            GitRepository repo = GitSampleHelpers.FindAnyRepository(this.Context, project.Id);
            GitPullRequest pullRequest = GitSampleHelpers.CreatePullRequest(this.Context, repo);
            
            Console.WriteLine("project {0}, repo {1}, pullRequestId {2}", project.Name, repo.Name, pullRequest.PullRequestId);

            GitPullRequestStatus status = GitSampleHelpers.GenerateSampleStatus();

            GitPullRequestStatus createdStatus = gitClient.CreatePullRequestStatusAsync(status, repo.Id, pullRequest.PullRequestId).Result;

            Console.WriteLine($"{createdStatus.Description}({createdStatus.Context.Genre}/{createdStatus.Context.Name}) with id {createdStatus.Id} created");

            GitSampleHelpers.AbandonPullRequest(this.Context, repo, pullRequest.PullRequestId);

            return createdStatus;
        }

        [ClientSampleMethod]
        public GitPullRequestStatus CreatePullRequestStatusWithIterationInBody()
        {
            VssConnection connection = this.Context.Connection;
            GitHttpClient gitClient = connection.GetClient<GitHttpClient>();

            TeamProjectReference project = ClientSampleHelpers.FindAnyProject(this.Context);
            GitRepository repo = GitSampleHelpers.FindAnyRepository(this.Context, project.Id);
            GitPullRequest pullRequest = GitSampleHelpers.CreatePullRequest(this.Context, repo);

            Console.WriteLine("project {0}, repo {1}, pullRequestId {2}", project.Name, repo.Name, pullRequest.PullRequestId);

            GitPullRequestStatus status = GitSampleHelpers.GenerateSampleStatus(1);

            GitPullRequestStatus createdStatus = gitClient.CreatePullRequestStatusAsync(status, repo.Id, pullRequest.PullRequestId).Result;

            Console.WriteLine($"{createdStatus.Description}({createdStatus.Context.Genre}/{createdStatus.Context.Name}) with id {createdStatus.Id} created"+
                $" on iteration {createdStatus.IterationId}");

            GitSampleHelpers.AbandonPullRequest(this.Context, repo, pullRequest.PullRequestId);

            return createdStatus;
        }

        [ClientSampleMethod]
        public GitPullRequestStatus CreatePullRequestIterationStatus()
        {
            VssConnection connection = this.Context.Connection;
            GitHttpClient gitClient = connection.GetClient<GitHttpClient>();

            TeamProjectReference project = ClientSampleHelpers.FindAnyProject(this.Context);
            GitRepository repo = GitSampleHelpers.FindAnyRepository(this.Context, project.Id);
            GitPullRequest pullRequest = GitSampleHelpers.CreatePullRequest(this.Context, repo);

            Console.WriteLine("project {0}, repo {1}, pullRequestId {2}", project.Name, repo.Name, pullRequest.PullRequestId);

            GitPullRequestStatus status = GitSampleHelpers.GenerateSampleStatus();

            GitPullRequestStatus createdStatus = gitClient.CreatePullRequestIterationStatusAsync(status, repo.Id, pullRequest.PullRequestId, 1).Result;

            Console.WriteLine($"{createdStatus.Description}({createdStatus.Context.Genre}/{createdStatus.Context.Name}) with id {createdStatus.Id} created" +
                $" on iteration {createdStatus.IterationId}");

            GitSampleHelpers.AbandonPullRequest(this.Context, repo, pullRequest.PullRequestId);

            return createdStatus;
        }

        [ClientSampleMethod]
        public List<GitPullRequestStatus> GetPullRequestIterationStatuses()
        {
            VssConnection connection = this.Context.Connection;
            GitHttpClient gitClient = connection.GetClient<GitHttpClient>();

            TeamProjectReference project = ClientSampleHelpers.FindAnyProject(this.Context);
            GitRepository repo = GitSampleHelpers.FindAnyRepository(this.Context, project.Id);
            GitPullRequest pullRequest = GitSampleHelpers.CreatePullRequest(this.Context, repo);

            GitSampleHelpers.CreatePullRequestStatus(this.Context, repo.Id, pullRequest.PullRequestId, 1);
            GitSampleHelpers.CreatePullRequestStatus(this.Context, repo.Id, pullRequest.PullRequestId, 1);

            Console.WriteLine($"project {project.Name}, repo {repo.Name}, pullRequestId {pullRequest.PullRequestId}");

            List<GitPullRequestStatus> iterationStatuses = gitClient.GetPullRequestIterationStatusesAsync(repo.Id, pullRequest.PullRequestId, 1).Result;

            Console.WriteLine($"{iterationStatuses.Count} statuses found for pull request {pullRequest.PullRequestId} iteration {1}");
            foreach (var status in iterationStatuses)
            {
                Console.WriteLine($"{status.Description}({status.Context.Genre}/{status.Context.Name}) with id {status.Id}");
            }

            GitSampleHelpers.AbandonPullRequest(this.Context, repo, pullRequest.PullRequestId);

            return iterationStatuses;
        }

        [ClientSampleMethod]
        public List<GitPullRequestStatus> GetPullRequestStatuses()
        {
            VssConnection connection = this.Context.Connection;
            GitHttpClient gitClient = connection.GetClient<GitHttpClient>();

            TeamProjectReference project = ClientSampleHelpers.FindAnyProject(this.Context);
            GitRepository repo = GitSampleHelpers.FindAnyRepository(this.Context, project.Id);
            GitPullRequest pullRequest = GitSampleHelpers.CreatePullRequest(this.Context, repo);

            GitSampleHelpers.CreatePullRequestStatus(this.Context, repo.Id, pullRequest.PullRequestId);
            GitSampleHelpers.CreatePullRequestStatus(this.Context, repo.Id, pullRequest.PullRequestId);

            Console.WriteLine($"project {project.Name}, repo {repo.Name}, pullRequestId {pullRequest.PullRequestId}");

            List<GitPullRequestStatus> iterationStatuses = gitClient.GetPullRequestStatusesAsync(repo.Id, pullRequest.PullRequestId).Result;

            Console.WriteLine($"{iterationStatuses.Count} statuses found for pull request {pullRequest.PullRequestId}");
            foreach (var status in iterationStatuses)
            {
                Console.WriteLine($"{status.Description}({status.Context.Genre}/{status.Context.Name}) with id {status.Id}");
            }

            GitSampleHelpers.AbandonPullRequest(this.Context, repo, pullRequest.PullRequestId);

            return iterationStatuses;
        }

        [ClientSampleMethod]
        public GitPullRequestStatus GetPullRequestStatus()
        {
            VssConnection connection = this.Context.Connection;
            GitHttpClient gitClient = connection.GetClient<GitHttpClient>();

            TeamProjectReference project = ClientSampleHelpers.FindAnyProject(this.Context);
            GitRepository repo = GitSampleHelpers.FindAnyRepository(this.Context, project.Id);
            GitPullRequest pullRequest = GitSampleHelpers.CreatePullRequest(this.Context, repo);
            GitPullRequestStatus generatedStatus = GitSampleHelpers.CreatePullRequestStatus(this.Context, repo.Id, pullRequest.PullRequestId);

            Console.WriteLine($"project {project.Name}, repo {repo.Name}, pullRequestId {pullRequest.PullRequestId}");

            GitPullRequestStatus status = gitClient.GetPullRequestStatusAsync(repo.Id, pullRequest.PullRequestId, generatedStatus.Id).Result;

            Console.WriteLine($"{status.Description}({status.Context.Genre}/{status.Context.Name}) with id {status.Id}");

            GitSampleHelpers.AbandonPullRequest(this.Context, repo, pullRequest.PullRequestId);

            return status;
        }

        [ClientSampleMethod]
        public GitPullRequestStatus GetPullRequestIterationStatus()
        {
            VssConnection connection = this.Context.Connection;
            GitHttpClient gitClient = connection.GetClient<GitHttpClient>();

            TeamProjectReference project = ClientSampleHelpers.FindAnyProject(this.Context);
            GitRepository repo = GitSampleHelpers.FindAnyRepository(this.Context, project.Id);
            GitPullRequest pullRequest = GitSampleHelpers.CreatePullRequest(this.Context, repo);
            GitPullRequestStatus generatedStatus = GitSampleHelpers.CreatePullRequestStatus(this.Context, repo.Id, pullRequest.PullRequestId, 1);

            Console.WriteLine($"project {project.Name}, repo {repo.Name}, pullRequestId {pullRequest.PullRequestId}");

            GitPullRequestStatus status = gitClient.GetPullRequestIterationStatusAsync(repo.Id, pullRequest.PullRequestId, 1, generatedStatus.Id).Result;

            Console.WriteLine($"{status.Description}({status.Context.Genre}/{status.Context.Name}) with id {status.Id}");

            GitSampleHelpers.AbandonPullRequest(this.Context, repo, pullRequest.PullRequestId);

            return status;
        }

    }
}
