using AgileApp.Controllers;
using AgileApp.Models.Common;
using AgileApp.Models.Projects;
using AgileApp.Models.Tasks;
using AgileApp.Services.Projects;
using AgileApp.Services.Tasks;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AgileControllerTests
{
    public class ProjectControllerTests
    {
        [Fact]
        public void AddProject_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            var cookieHelperMock = new Mock<ICookieHelper>();
            var taskServiceMock = new Mock<ITaskService>();

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            var request = new AddProjectRequest
            {
                Name = "Project 1"
            };

            // Act
            var result = controller.AddProject(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void AddProject_WithNullRequest_ReturnsBadRequestResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            var cookieHelperMock = new Mock<ICookieHelper>();
            var taskServiceMock = new Mock<ITaskService>();

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            // Act
            var result = controller.AddProject(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void AddTask_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            var cookieHelperMock = new Mock<ICookieHelper>();
            var taskServiceMock = new Mock<ITaskService>();

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            var request = new AddTaskRequest
            {
                Name = "Task 1"
            };

            // Act
            var result = controller.AddTask(1, request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void AddTask_WithNullRequest_ReturnsBadRequestResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            var cookieHelperMock = new Mock<ICookieHelper>();
            var taskServiceMock = new Mock<ITaskService>();

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            // Act
            var result = controller.AddTask(1, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetAllProjects_WithValidCookie_ReturnsOkResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            var cookieHelperMock = new Mock<ICookieHelper>();
            var taskServiceMock = new Mock<ITaskService>();

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            var reverseTokenResult = new ReverseTokenResult
            {
                IsValid = true,
                Claims = new List<ReverseTokenClaim>
                {
                    new ReverseTokenClaim
                    {
                        Type = System.Security.Claims.ClaimTypes.Hash,
                        Value = "hash"
                    }
                }
            };
            cookieHelperMock.Setup(helper => helper.ReverseJwtFromRequest(controller.HttpContext))
                            .Returns(reverseTokenResult.TokenResult(reverseTokenResult));

            var projects = new List<ProjectResponse>
            {
                new ProjectResponse { Id = 1, Name = "Project 1" },
                new ProjectResponse { Id = 2, Name = "Project 2" }
            };
            projectServiceMock.Setup(service => service.GetAllProjects())
                              .Returns(projects);

            // Act
            var result = controller.GetAllProjects();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<List<ProjectResponse>>>(okObjectResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(projects, response.Data);
        }

        [Fact]
        public void GetAllProjects_WithInvalidCookie_ReturnsUnauthorizedResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            var cookieHelperMock = new Mock<ICookieHelper>();
            var taskServiceMock = new Mock<ITaskService>();

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            var reverseTokenResult = new ReverseTokenResult { IsValid = false };
            cookieHelperMock.Setup(helper => helper.ReverseJwtFromRequest(controller.HttpContext))
                            .Returns(reverseTokenResult);

            // Act
            var result = controller.GetAllProjects();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void GetProjectById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            var cookieHelperMock = new Mock<ICookieHelper>();
            var taskServiceMock = new Mock<ITaskService>();

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            var reverseTokenResult = new ReverseTokenResult { IsValid = true };
            cookieHelperMock.Setup(helper => helper.ReverseJwtFromRequest(controller.HttpContext))
                            .Returns(reverseTokenResult);

            var project = new ProjectResponse { Id = 1, Name = "Project 1" };
            projectServiceMock.Setup(service => service.GetProjectById(1))
                              .Returns(project);

            // Act
            var result = controller.GetProjectById(1);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(project, okObjectResult.Value);
        }

        [Fact]
        public void GetProjectById_WithInvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            var cookieHelperMock = new Mock<ICookieHelper>();
            var taskServiceMock = new Mock<ITaskService>();

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            // Act
            var result = controller.GetProjectById(-1);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        // More tests for other actions in the ProjectController
    }
}

