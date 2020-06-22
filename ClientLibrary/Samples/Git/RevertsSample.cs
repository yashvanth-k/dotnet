using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.DevOps.ClientSamples.Git
{
    [ClientSample(GitWebApiConstants.AreaName, "reverts")]
    public class RevertsSample : ClientSample
    {
        [ClientSampleMethod]
        public GitRevert CreateRevert()
        {
            VssConnection connection = this.Context.Connection;
            GitHttpClient gitClient = connection.GetClient<GitHttpClient>();

            Guid projectId = ClientSampleHelpers.FindAnyProject(this.Context).Id;
            GitRepository repo = GitSampleHelpers.FindAnyRepository(this.Context, projectId);
            string branchName = repo.DefaultBranch;
            string branchNameWithoutRefsHeads = branchName.Remove(0, "refs/heads/".Length);

            // find the latest commit on default
            GitCommitRef latestCommitOnDefault = gitClient.GetCommitsAsync(repo.Id, new GitQueryCommitsCriteria()
            {
                ItemVersion = new GitVersionDescriptor()
                {
                    Version = branchNameWithoutRefsHeads,
                    VersionType = GitVersionType.Branch
                },
                Top = 1
            }).Result.First();

            // generate a unique name to suggest for the branch
            string suggestedBranchName = "refs/heads/azure-devops-dotnet-samples/" + GitSampleHelpers.ChooseRefsafeName();

            // write down the name for a later sample
            this.Context.SetValue<string>("$gitSamples.suggestedRevertBranchName", suggestedBranchName);

            // revert it relative to default branch
            GitRevert revert = gitClient.CreateRevertAsync(
                new GitAsyncRefOperationParameters()
                {
                    OntoRefName = branchName,
                    GeneratedRefName = suggestedBranchName,
                    Repository = repo,
                    Source = new GitAsyncRefOperationSource()
                    {
                        CommitList = new GitCommitRef[] { new GitCommitRef() { CommitId = latestCommitOnDefault.CommitId } }
                    },
                },
                projectId,
                repo.Id).Result;

            Console.WriteLine("Revert {0} created", revert.RevertId);

            // typically, the next thing you'd do is create a PR for this revert
            return revert;
        }

        [ClientSampleMethod]
        public GitRevert GetRevert()
        {
            VssConnection connection = this.Context.Connection;
            GitHttpClient gitClient = connection.GetClient<GitHttpClient>();

            Guid projectId = ClientSampleHelpers.FindAnyProject(this.Context).Id;
            GitRepository repo = GitSampleHelpers.FindAnyRepository(this.Context, projectId);

            // pull out the branch name we suggested before
            string branchName;
            if (this.Context.TryGetValue<string>("$gitSamples.suggestedRevertBranchName", out branchName))
            {

                GitRevert revert = gitClient.GetRevertForRefNameAsync(projectId, repo.Id, branchName).Result;

                Console.WriteLine("Revert {0} found with status {1}", revert.RevertId, revert.Status);
                return revert;
            }

            Console.WriteLine("(skipping sample; did not find a branch to check for reverts)");
            return null;
        }
    }
}
