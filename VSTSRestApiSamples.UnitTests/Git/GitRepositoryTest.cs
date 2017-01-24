using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VstsRestApiSamples.Git;
using System.Net;

namespace VstsRestApiSamples.Tests.Git
{
    [TestClass]
    public class GitRepositoryTest
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

        [TestMethod, TestCategory("REST API")]
        public void Git_Repository_GetAllRepositories_Success()
        {
            //arrange
            GitRepository request = new GitRepository(_configuration);

            //act
            var response = request.GetAllRepositories();

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);

            request = null;
        }

        [TestMethod, TestCategory("REST API")]
        public void Git_Repository_GetAllRepositoryById_Success()
        {
            //arrange
            GitRepository request = new GitRepository(_configuration);

            //act
            var response = request.GetRepositoryById(_configuration.GitRepositoryId);

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);

            request = null;
        }

        [TestMethod, TestCategory("REST API")]
        public void Git_Repository_GetFolderAndChildren_Success()
        {
            //arrange
            GitRepository request = new GitRepository(_configuration);

            //act
            var response = request.GetFolderAndChildren(_configuration.GitRepositoryId, "/");

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);

            request = null;
        }

        [TestMethod, TestCategory("REST API")]
        public void Git_Repository_GetCommitsByRepositoryId_Success()
        {
            //arrange
            GitRepository request = new GitRepository(_configuration);

            //act
            var response = request.GetCommitsByRepositoryId(_configuration.GitRepositoryId);

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);

            request = null;
        }

        [TestMethod, TestCategory("REST API")]
        public void Git_Repository_GetDiffsByRepositoryId_Success()
        {
            //arrange
            GitRepository request = new GitRepository(_configuration);

            //act
            var response = request.GetDiffsByRepositoryId(
                _configuration.GitRepositoryId,
                _configuration.GitTargetVersionBranch,
                _configuration.GitBaseVersionBranch);

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode); // This is throwing a bad request, but as far as I can tell it's following the examples online.
        }

        [TestMethod, TestCategory("REST API")]
        public void Git_Repository_GetStatsByRepositoryId_Success()
        {
            //arrange
            GitRepository request = new GitRepository(_configuration);

            //act
            var response = request.GetStatsByRepositoryId(
                _configuration.GitRepositoryId);

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);
        }

    }
}