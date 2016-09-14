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

        [TestMethod, TestCategory("REST API"), TestCategory("GIT REST API")]
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

        [TestMethod, TestCategory("REST API"), TestCategory("GIT REST API")]
        public void Git_Repository_GetAllRepositoryById_Success()
        {
            //arrange
            GitRepository request = new GitRepository(_configuration);

            //act
            var response = request.GetRepositoryById("de6d058e-e19f-471d-b6f1-02234761f316");

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);

            request = null;
        }

        [TestMethod, TestCategory("REST API"), TestCategory("GIT REST API")]
        public void Git_Repository_GetFolderAndChildren_Success()
        {
            //arrange
            GitRepository request = new GitRepository(_configuration);

            //act
            var response = request.GetFolderAndChildren("de6d058e-e19f-471d-b6f1-02234761f316", "/");

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);

            request = null;
        }
    }
}