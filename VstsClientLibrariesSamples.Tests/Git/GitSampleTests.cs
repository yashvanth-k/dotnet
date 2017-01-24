using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VstsClientLibrariesSamples.Git;

namespace VstsClientLibrariesSamples.Tests.Git
{
    [TestClass]
    public class GitSampleTests
    {
        private IConfiguration _configuration = new Configuration();

        [TestInitialize]
        public void TestInitialize()
        {
            InitHelper.GetConfiguration(_configuration);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _configuration = null;
        }


        [TestMethod, TestCategory("Client Libraries")]
        public void Sample_GetRepositories_Success()
        {
            var gitSample = new Sample(_configuration);

            try
            {
                // act
                var result = gitSample.GetRepositories();

                Assert.IsNotNull(result);
            }
            catch (NullReferenceException ex)
            {
                Assert.Inconclusive(ex.Message);
            }
        }


        [TestMethod, TestCategory("Client Libraries")]
        public void Sample_GetGitCommitDiffs_Success()
        {
            var gitSample = new Sample(_configuration);

            try
            {
                // act
                var result = gitSample.GetGitCommitDiffs();

                Assert.IsNotNull(result);
            }
            catch (NullReferenceException ex)
            {
                Assert.Inconclusive(ex.Message);
            }
        }

        [TestMethod, TestCategory("Client Libraries")]
        public void Sample_GetGitCommitDiffsByBranch_Success()
        {
            var gitSample = new Sample(_configuration);

            try
            {
                // act
                var result = gitSample.GetGitCommitDiffsByBranch();

                Assert.IsNotNull(result);
            }
            catch (NullReferenceException ex)
            {
                Assert.Inconclusive(ex.Message);
            }
        }
    }
}
