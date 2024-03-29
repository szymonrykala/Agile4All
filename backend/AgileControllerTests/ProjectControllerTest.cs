using System.Collections.Generic;
using System.Security.Claims;
using AgileApp.Controllers;
using AgileApp.Enums;
using AgileApp.Models.Common;
using AgileApp.Models.Jwt;
using AgileApp.Models.Projects;
using AgileApp.Models.Tasks;
using AgileApp.Models.Users;
using AgileApp.Services.Projects;
using AgileApp.Services.Tasks;
using AgileApp.Services.Users;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Http;
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
            projectServiceMock.Setup(x => x.GetProjectById(It.IsAny<int>()))
                              .Returns(new ProjectResponse { Id = 1 });
            projectServiceMock.Setup(x => x.AddUserToProject(It.IsAny<ProjectUserRequest>()))
                              .Returns(new Response { IsSuccess = true });

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetAllUsers())
                           .Returns(new List<GetAllUsersResponse>());
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>()))
                           .Returns(new GetAllUsersResponse { Id = 1 });

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult
                            {
                                IsValid = true,
                                Claims = new[]
                                {
                                    new Claim(ClaimTypes.Role, ((int)UserRoleEnum.ADMIN).ToString()),
                                    new Claim(ClaimTypes.Hash, "1"),
                                }
                            });

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetAllTasks())
                           .Returns(new List<TaskResponse>());

            var controller = new ProjectController(projectServiceMock.Object, userServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            // Act
            var result = controller.AddProject(new AddProjectRequest { Name = "NewSuperIntelligentName" });

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void AddTask_WithValidRequestAndAuthorizedUser_ReturnsOkResult()
        {
            // Arrange
            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.GetAllProjects())
                              .Returns(new List<ProjectResponse>());

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.AddNewTask(It.IsAny<AddTaskRequest>()))
                           .Returns(new OkObjectResult(true));//new Response<int> { IsSuccess = true, Data = 1 });

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>()))
                           .Returns(new GetAllUsersResponse { Id = 1 });

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<HttpContext>()))
                            .Returns(new JwtReverseResult { IsValid = true });

            var controller = new ProjectController(projectServiceMock.Object, userServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            // Act
            var result = controller.AddTask(1, new AddTaskRequest { Name = "Super partia", Description = "Super Description", UserId = 1 });

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

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>()))
                           .Returns(new GetAllUsersResponse());

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult
                            {
                                IsValid = true,
                                Claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Hash, "hash")
                                }
                            });

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetTaskById(It.IsAny<int>()))
                           .Returns(new TaskResponse());

            var controller = new ProjectController(projectServiceMock.Object, userServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            // Setup HttpContext
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

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
                              .Returns(new ProjectResponse { Id = 1 });

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>()))
                           .Returns(new GetAllUsersResponse());

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult { IsValid = true });

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetTaskById(It.IsAny<int>()))
                           .Returns(new TaskResponse());

            var controller = new ProjectController(projectServiceMock.Object, userServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

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
                              .Returns(new Response<string> { IsSuccess = true });
            projectServiceMock.Setup(x => x.GetProjectById(It.IsAny<int>()))
                              .Returns(new ProjectResponse { Id = 1 });

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>()))
                           .Returns(new GetAllUsersResponse());

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult
                            {
                                IsValid = true,
                                Claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Role, ((int)UserRoleEnum.ADMIN).ToString())
                                }
                            });

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetTaskById(It.IsAny<int>()))
                           .Returns(new TaskResponse());

            var controller = new ProjectController(projectServiceMock.Object, userServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            // Setup HttpContext
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

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
            projectServiceMock.Setup(x => x.GetProjectById(It.IsAny<int>()))
                              .Returns(new ProjectResponse { Id = 1 });

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>()))
                           .Returns(new GetAllUsersResponse());

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult
                            {
                                IsValid = true,
                                Claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Role, ((int)UserRoleEnum.ADMIN).ToString())
                                }
                            });

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetTaskById(It.IsAny<int>()))
                           .Returns(new TaskResponse());

            var controller = new ProjectController(projectServiceMock.Object, userServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            // Setup HttpContext
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

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
            projectServiceMock.Setup(x => x.GetProjectById(It.IsAny<int>()))
                              .Returns(new ProjectResponse { Id = 1 });

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>()))
                           .Returns(new GetAllUsersResponse { Id = 1 });

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult
                            {
                                IsValid = true,
                                Claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Role, ((int)UserRoleEnum.ADMIN).ToString())
                                }
                            });

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetTaskById(It.IsAny<int>()))
                           .Returns(new TaskResponse());

            var controller = new ProjectController(projectServiceMock.Object, userServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            // Setup HttpContext
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

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
            projectServiceMock.Setup(x => x.GetProjectById(It.IsAny<int>()))
                              .Returns(new ProjectResponse { Id = 1 });

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>()))
                           .Returns(new GetAllUsersResponse { Id = 1 });

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult
                            {
                                IsValid = true,
                                Claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Role, ((int)UserRoleEnum.ADMIN).ToString())
                                }
                            });

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetTaskById(It.IsAny<int>()))
                           .Returns(new TaskResponse());

            var controller = new ProjectController(projectServiceMock.Object, userServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object);

            // Setup HttpContext
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = controller.RemoveUserFromProject(1, 1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
