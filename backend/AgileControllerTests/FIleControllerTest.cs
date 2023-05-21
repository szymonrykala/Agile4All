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
        public void GetFiles_WithValidQueryParameters_ReturnsOkResult()
        {
            // Arrange
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(service => service.GetFiles(It.IsAny<int>(), It.IsAny<int>()))
                           .Returns(new List<GetFileResponse>());

            var controller = new FileController(fileServiceMock.Object);

            // Act
            var result = controller.GetFiles(taskId: 1, projectId: 2);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetFiles_WithInvalidQueryParameters_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new FileController(Mock.Of<IFileService>());

            // Act
            var result = controller.GetFiles(taskId: -1, projectId: -1);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void UploadFile_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(service => service.UploadFile(It.IsAny<UploadFileRequest>()))
                           .Returns(new UploadFileResponse());

            var controller = new FileController(fileServiceMock.Object);

            // Act
            var result = controller.UploadFile(new UploadFileRequest());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetFileById_WithValidFileId_ReturnsFileResult()
        {
            // Arrange
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(service => service.GetFileById(It.IsAny<int>()))
                           .Returns("path/to/file");

            var controller = new FileController(fileServiceMock.Object);

            // Act
            var result = controller.GetFileById(1);

            // Assert
            Assert.IsType<FileResult>(result);
        }

        [Fact]
        public void GetFileById_WithInvalidFileId_ReturnsNotFoundResult()
        {
            // Arrange
            var controller = new FileController(Mock.Of<IFileService>());

            // Act
            var result = controller.GetFileById(-1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteFile_WithValidFileId_ReturnsOkResult()
        {
            // Arrange
            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(service => service.DeleteFile(It.IsAny<int>()))
                           .Returns(true);

            var controller = new FileController(fileServiceMock.Object);

            // Act
            var result = controller.DeleteFile(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
