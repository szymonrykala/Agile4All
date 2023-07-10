using AgileApp.Controllers;
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
        public void GetAllTasks_WithAuthorizedUser_ReturnsOkResult()
        {
            // Arrange
            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetAllTasks())
                           .Returns(new List<TaskResponse>());

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new ReverseJwtResult { IsValid = true });

            var controller = new TaskController(taskServiceMock.Object, cookieHelperMock.Object);

            // Act
            var result = controller.GetAllTasks();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetTaskById_WithValidTaskIdAndAuthorizedUser_ReturnsOkResult()
        {
            // Arrange
            var taskServiceMock = new Mock<ITaskService>();
            taskServiceMock.Setup(x => x.GetTaskById(It.IsAny<int>()))
                           .Returns(new TaskResponse());

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new ReverseJwtResult { IsValid = true });

            var controller = new TaskController(taskServiceMock.Object, cookieHelperMock.Object);

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

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new ReverseJwtResult { IsValid = true });

            var controller = new TaskController(taskServiceMock.Object, cookieHelperMock.Object);

            // Act
            var result = controller.UpdateTask(1, new UpdateTaskRequest());

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

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new ReverseJwtResult { IsValid = true });

            var controller = new TaskController(taskServiceMock.Object, cookieHelperMock.Object);

            // Act
            var result = controller.DeleteTask(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
