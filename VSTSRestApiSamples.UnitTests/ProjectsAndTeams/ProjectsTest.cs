using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net;
using VstsRestApiSamples.ProjectsAndTeams;
using VstsRestApiSamples.ViewModels.ProjectsAndTeams;

namespace VstsRestApiSamples.Tests.ProjectsAndTeams
{
    [TestClass]
    public class ProjectsTest
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
        public void ProjectsAndTeams_Projects_GetListOfProjects_Success()
        {
            // arrange
            Projects request = new Projects(_configuration);

            // act
            var response = request.GetProjects();

            // assert
            Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);

            request = null;
        }

        [TestMethod, TestCategory("REST API")]
        public void ProjectsAndTeams_Projects_GetSingleProject_Success()
        {
            // arrange
            Projects request = new Projects(_configuration);

            // act
            var listResponse = request.GetProjects();                       // get list of Projects
            IList<ListofProjectsResponse.Value> vm = listResponse.value;    // bind to list
            string projectId = vm[0].id;                                    // get a project id so we can look that up

            var response = request.GetProject(projectId);

            // assert
            Assert.AreEqual(HttpStatusCode.OK, response.HttpStatusCode);

            request = null;
        }
    }
}
