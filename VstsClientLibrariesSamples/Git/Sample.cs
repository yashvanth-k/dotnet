using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;

namespace VstsClientLibrariesSamples.Git
{
    public class Sample
    {
        readonly IConfiguration _configuration;
        private VssBasicCredential _credentials;
        private Uri _uri;

        public Sample(IConfiguration configuration)
        {
            _configuration = configuration;
            _credentials = new VssBasicCredential("", _configuration.PersonalAccessToken);
            _uri = new Uri(_configuration.UriString);
        }

        public List<GitRepository> GetRepositories()
        {
            List<GitRepository> results;

            using (var client = new GitHttpClient(_uri, _credentials))
            {
                results = client.GetRepositoriesAsync(_configuration.Project)
                    .Result;
            }

            return results;
        }

        public GitCommitDiffs GetGitCommitDiffs()
        {
            GitCommitDiffs diffs;

            using (var client = new GitHttpClient(_uri, _credentials))
            {
                diffs = client.GetCommitDiffsAsync(
                    _configuration.Project,
                    _configuration.GitRepositoryId)
                    .Result;

            }

            return diffs;
        }

        public GitCommitDiffs GetGitCommitDiffsByBranch()
        {
            GitCommitDiffs diffs;

            using (var client = new GitHttpClient(_uri, _credentials))
            {
                var baseVersion = new GitBaseVersionDescriptor { Version = _configuration.GitBaseVersionBranch };
                var targetVersion = new GitTargetVersionDescriptor { Version = _configuration.GitTargetVersionBranch };

                diffs = client.GetCommitDiffsAsync(
                    _configuration.Project,
                    _configuration.GitRepositoryId,
                    baseVersionDescriptor: baseVersion,
                    targetVersionDescriptor: targetVersion)
                    .Result;

            }

            return diffs;
        }
    }
}
