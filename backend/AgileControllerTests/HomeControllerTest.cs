using AgileApp.Controllers;
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
        public void Error_ReturnsViewResultWithModelErrorViewModel()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(loggerMock.Object);

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.ViewData.Model);
            Assert.NotNull(model.RequestId);
        }
    }
}
