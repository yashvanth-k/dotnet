using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;
using VstsRestApiSamples.ProjectsAndTeams;
using VstsRestApiSamples.ViewModels.ProjectsAndTeams;

namespace VstsRestApiSamples.Tests.ProjectsAndTeams
{
    [TestClass]
    public class ProjectCollectionsTest
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
        public void ProjectsAndTeams_ProjectCollections_GetListOfProjectCollections_Success()
        {
            // arrange
            ProjectCollections request = new ProjectCollections(_configuration);

            // act
            var response = request.GetProjectCollections();

            // assert
            Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);

            request = null;
        }

        [TestMethod, TestCategory("REST API")]
        public void ProjectsAndTeams_ProjectCollections_GetSingleProjectCollection_Success()
        {
            // arrange
            ProjectCollections request = new ProjectCollections(_configuration);

            // act
            var listResponse = request.GetProjectCollections();                     // get list of project collections
            IList<ListofProjectCollectionsResponse.Value> vm = listResponse.value;     // bind to list
            string projectCollectionId = vm[0].id;                                    // get a project collection id so we can look that up

            var response = request.GetProjectCollection(projectCollectionId);

            // assert
            Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);

            request = null;
        }
    }
}
