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
        public void Git_Repository_GetAllRepositorys_Success()
        {
            //arrange
            GitRepository request = new GitRepository(_configuration);

            //act
            var response = request.GetAllRepositories();

            //assert
            Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);

            request = null;
        }
    }
}