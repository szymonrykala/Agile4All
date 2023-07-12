using AgileApp.Controllers;
using AgileApp.Models.Common;
using AgileApp.Models.Files;
using AgileApp.Services.Files;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace AgileControllerTests
{
    public class FileControllerTests
    {
        [Fact]
        public void GetFiles_WithValidParameters_ReturnsOkResult()
        {
            // Arrange
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(x => x.GetFiles(It.IsAny<int>(), It.IsAny<int>()))
                           .Returns(new List<GetFileResponse>());

            var controller = new FileController(fileServiceMock.Object);

            // Act
            var result = controller.GetFiles(taskId: 1, projectId: 2);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void UploadFile_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(x => x.UploadFile(It.IsAny<UploadFileRequest>()))
                           .Returns(new Response<bool> { IsSuccess = true });

            var controller = new FileController(fileServiceMock.Object);

            // Act
            var result = controller.UploadFile(new UploadFileRequest());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteFile_WithValidFileId_ReturnsOkResult()
        {
            // Arrange
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(x => x.DeleteFile(It.IsAny<int>()))
                           .Returns(new Response { IsSuccess = true });

            var controller = new FileController(fileServiceMock.Object);

            // Act
            var result = controller.DeleteFile(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
