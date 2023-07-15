using AgileApp.Controllers;
using AgileApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AgileControllerTests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(loggerMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(loggerMock.Object);

            // Act
            var result = controller.Privacy();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsViewResult_WithModelError()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(loggerMock.Object);

            // Setup HttpContext
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = controller.Error() as ViewResult;
            var model = result.ViewData.Model;

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsType<ErrorViewModel>(model);
        }
    }
}
