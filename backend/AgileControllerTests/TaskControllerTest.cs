using AgileApp.Models.Tasks;
using AgileApp.Services.Tasks;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AgileControllerTests
{
    public class TaskControllerTests
    {
        [Fact]
        public void GetAllTasks_WithValidCookie_ReturnsOkResult()
        {
            // Arrange
            var taskServiceMock = new Mock<ITaskService>();
            var cookieHelperMock = new Mock<ICookieHelper>();

            var controller = new TaskController(taskServiceMock.Object, cookieHelperMock.Object);

            var reverseTokenResult = new ReverseTokenResult { IsValid = true };
            cookieHelperMock.Setup(helper => helper.ReverseJwtFromRequest(controller.HttpContext))
                            .Returns(reverseTokenResult);

            var tasks = new List<TaskResponse>
            {
                new TaskResponse { Id = 1, Name = "Task 1" },
                new TaskResponse { Id = 2, Name = "Task 2" }
            };
            taskServiceMock.Setup(service => service.GetAllTasks())
                           .Returns(tasks);

            // Act
            var result = controller.GetAllTasks();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<List<TaskResponse>>(okObjectResult.Value);
            Assert.Equal(tasks, response);
        }

        [Fact]
        public void GetAllTasks_WithInvalidCookie_ReturnsForbidResult()
        {
            // Arrange
            var taskServiceMock = new Mock<ITaskService>();
            var cookieHelperMock = new Mock<ICookieHelper>();

            var controller = new TaskController(taskServiceMock.Object, cookieHelperMock.Object);

            var reverseTokenResult = new ReverseTokenResult { IsValid = false };
            cookieHelperMock.Setup(helper => helper.ReverseJwtFromRequest(controller.HttpContext))
                            .Returns(reverseTokenResult);

            // Act
            var result = controller.GetAllTasks();

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public void GetTaskById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var taskServiceMock = new Mock<ITaskService>();
            var cookieHelperMock = new Mock<ICookieHelper>();

            var controller = new TaskController(taskServiceMock.Object, cookieHelperMock.Object);

            var reverseTokenResult = new ReverseTokenResult { IsValid = true };
            cookieHelperMock.Setup(helper => helper.ReverseJwtFromRequest(controller.HttpContext))
                            .Returns(reverseTokenResult);

            var task = new TaskResponse { Id = 1, Name = "Task 1" };
            taskServiceMock.Setup(service => service.GetTaskById(1))
                           .Returns(task);

            // Act
            var result = controller.GetTaskById(1);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(task, okObjectResult.Value);
        }

        [Fact]
        public void GetTaskById_WithInvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            var taskServiceMock = new Mock<ITaskService>();
            var cookieHelperMock = new Mock<ICookieHelper>();

            var controller = new TaskController(taskServiceMock.Object, cookieHelperMock.Object);

            // Act
            var result = controller.GetTaskById(-1);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        // More tests for other actions in the TaskController
    }
}
