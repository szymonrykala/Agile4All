using AgileApp.Controllers;
using AgileApp.Models.Common;
using AgileApp.Models.Files;
using AgileApp.Enums;
using AgileApp.Services.Files;
using AgileApp.Models.Tasks;
using AgileApp.Services.Tasks;
using AgileApp.Models.Projects;
using AgileApp.Services.Projects;
using AgileApp.Utils.Cookies;
using AgileApp.Models.Jwt;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace AgileControllerTests
{
    public class FileControllerTests
    {
        [Fact]
        public void GetFiles_WithValidParameters_ReturnsOkResult()
        {
            // Arrange
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

            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.GetAllProjects())
                              .Returns(new List<ProjectResponse>());

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetAllTasks())
                           .Returns(new List<TaskResponse>());
            
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(x => x.GetFiles(It.IsAny<int>(), It.IsAny<int>()))
                           .Returns(new List<GetFileResponse>());

            var controller = new FileController(fileServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object, projectServiceMock.Object);

            // Act
            var result = controller.GetFiles(taskId: 1, projectId: 2);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void UploadFile_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
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

            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.GetAllProjects())
                              .Returns(new List<ProjectResponse>());

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetAllTasks())
                           .Returns(new List<TaskResponse>());

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(x => x.UploadFile(It.IsAny<UploadFileRequest>()))
                           .Returns(new Response<bool> { IsSuccess = true });

            var controller = new FileController(fileServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object, projectServiceMock.Object);

            // Act
            var result = controller.UploadFile(new UploadFileRequest());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteFile_WithValidFileId_ReturnsOkResult()
        {
            // Arrange
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

            var projectServiceMock = new Mock<IProjectService>();
            projectServiceMock.Setup(x => x.GetAllProjects())
                              .Returns(new List<ProjectResponse>());

            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetAllTasks())
                           .Returns(new List<TaskResponse>());

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(x => x.DeleteFile(It.IsAny<int>()))
                           .Returns(new Response { IsSuccess = true });

            var controller = new FileController(fileServiceMock.Object, cookieHelperMock.Object, taskServiceMock.Object, projectServiceMock.Object);

            // Act
            var result = controller.DeleteFile(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
