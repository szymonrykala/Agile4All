using System.Collections.Generic;
using System.Security.Claims;
using AgileApp.Controllers;
using AgileApp.Enums;
using AgileApp.Models.Jwt;
using AgileApp.Models.Tasks;
using AgileApp.Models.Projects;
using AgileApp.Models.Users;
using AgileApp.Services.Tasks;
using AgileApp.Services.Projects;
using AgileApp.Services.Users;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AgileControllerTests
{
    public class TaskControllerTests
    {
        [Fact]
        public void GetAllTasks_WithAuthorizedUser_ReturnsOkResult()
        {
            // Arrange
            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetAllTasks())
                           .Returns(new List<TaskResponse>());

            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.GetProjectById(It.IsAny<int>()))
                              .Returns(new ProjectResponse());

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>()))
                           .Returns(new GetAllUsersResponse());

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new JwtReverseResult { IsValid = true });

            var controller = new TaskController(taskServiceMock.Object, projectServiceMock.Object, userServiceMock.Object, cookieHelperMock.Object);

            // Act
            var result = controller.GetAllTasks();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetTaskById_WithNotExistingTaskIdAndAuthorizedUser_ReturnsNotFoundResult()
        {
            // Arrange
            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetTaskById(It.IsAny<int>()))
                           .Returns(new TaskResponse());

            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.GetProjectById(It.IsAny<int>()))
                              .Returns(new ProjectResponse());

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

            var controller = new TaskController(taskServiceMock.Object, projectServiceMock.Object, userServiceMock.Object, cookieHelperMock.Object);

            // Act
            var result = controller.GetTaskById(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetTaskById_WithValidTaskIdAndAuthorizedUser_ReturnsOkResult()
        {
            // Arrange
            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetTaskById(It.IsAny<int>()))
                           .Returns(new TaskResponse { Id = 1 });

            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.GetProjectById(It.IsAny<int>()))
                              .Returns(new ProjectResponse());

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

            var controller = new TaskController(taskServiceMock.Object, projectServiceMock.Object, userServiceMock.Object, cookieHelperMock.Object);

            // Act
            var result = controller.GetTaskById(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void UpdateTask_WithValidTaskIdAndAuthorizedUser_ReturnsOkResult()
        {
            // Arrange
            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.UpdateTask(It.IsAny<UpdateTaskRequest>()))
                           .Returns(true);
            taskServiceMock.Setup(x => x.GetTaskById(It.IsAny<int>()))
                           .Returns(new TaskResponse { Id = 1 });

            var projectServiceMock = new Mock<IProjectService>();
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

            var controller = new TaskController(taskServiceMock.Object, projectServiceMock.Object, userServiceMock.Object, cookieHelperMock.Object);

            // Setup HttpContext
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = controller.UpdateTask(1, new UpdateTaskRequest { Name = "And his name is John Cena!" });

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteTask_WithValidTaskIdAndAdminUser_ReturnsOkResult()
        {
            // Arrange
            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.DeleteTask(It.IsAny<int>()))
                           .Returns(true);
            taskServiceMock.Setup(x => x.GetTaskById(It.IsAny<int>()))
                           .Returns(new TaskResponse { Id = 1 });

            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.GetProjectById(It.IsAny<int>()))
                              .Returns(new ProjectResponse());

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>()))
                           .Returns(new GetAllUsersResponse());

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<HttpContext>()))
                            .Returns(new JwtReverseResult
                            {
                                IsValid = true,
                                Claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Role, ((int)UserRoleEnum.ADMIN).ToString())
                                }
                            });

            var controller = new TaskController(taskServiceMock.Object, projectServiceMock.Object, userServiceMock.Object, cookieHelperMock.Object);

            // Setup HttpContext
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = controller.DeleteTask(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
