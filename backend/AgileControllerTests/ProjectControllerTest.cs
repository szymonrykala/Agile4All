using System.Collections.Generic;
using AgileApp.Controllers;
using AgileApp.Models.Common;
using AgileApp.Models.Jwt;
using AgileApp.Models.Projects;
using AgileApp.Models.Tasks;
using AgileApp.Services.Projects;
using AgileApp.Services.Tasks;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AgileControllerTests
{
    public class ProjectControllerTests
    {
        [Fact]
        public void AddProject_WithValidRequestAndAdminUser_ReturnsOkResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.AddNewProject(It.IsAny<AddProjectRequest>()))
                              .Returns(new Response<int> { IsSuccess = true, Data = 1 });

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult { IsValid = true });

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, null);

            // Act
            var result = controller.AddProject(new AddProjectRequest());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void AddTask_WithValidRequestAndAuthorizedUser_ReturnsOkResult()
        {
            // Arrange
            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.AddNewTask(It.IsAny<AddTaskRequest>()))
                           .Returns("test");//new Response<int> { IsSuccess = true, Data = 1 });

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult { IsValid = true });

            var controller = new ProjectController(null, cookieHelperMock.Object, taskServiceMock.Object);

            // Act
            var result = controller.AddTask(1, new AddTaskRequest());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetAllProjects_WithAuthorizedUser_ReturnsOkResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.GetAllProjects())
                              .Returns(new List<ProjectResponse>());

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult { IsValid = true });

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, null);

            // Act
            var result = controller.GetAllProjects();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetProjectById_WithValidProjectIdAndAuthorizedUser_ReturnsOkResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.GetProjectById(It.IsAny<int>()))
                              .Returns(new ProjectResponse());

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult { IsValid = true });

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, null);

            // Act
            var result = controller.GetProjectById(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void UpdateProject_WithValidProjectIdAndAdminUser_ReturnsOkResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.UpdateProject(It.IsAny<UpdateProjectRequest>()))
                              .Returns(true);

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult { IsValid = true });

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, null);

            // Act
            var result = controller.UpdateProject(1, new UpdateProjectRequest());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteProject_WithValidProjectIdAndAdminUser_ReturnsOkResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.DeleteProject(It.IsAny<int>()))
                              .Returns(true);

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult { IsValid = true });

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, null);

            // Act
            var result = controller.DeleteProject(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void AddUserToProject_WithValidProjectIdUserIdAndAdminUser_ReturnsOkResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.AddUserToProject(It.IsAny<ProjectUserRequest>()))
                              .Returns(new Response { IsSuccess = true });

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult { IsValid = true });

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, null);

            // Act
            var result = controller.AddUserToProject(1, 1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void RemoveUserFromProject_WithValidProjectIdUserIdAndAdminUser_ReturnsOkResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.RemoveUserFromProject(It.IsAny<ProjectUserRequest>()))
                              .Returns(new Response { IsSuccess = true });

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult { IsValid = true });

            var controller = new ProjectController(projectServiceMock.Object, cookieHelperMock.Object, null);

            // Act
            var result = controller.RemoveUserFromProject(1, 1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
