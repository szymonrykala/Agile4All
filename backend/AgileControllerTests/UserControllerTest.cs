using AgileApp.Controllers;
using AgileApp.Models;
using AgileApp.Models.Common;
using AgileApp.Models.Requests;
using AgileApp.Services.Users;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AgileControllerTests
{
    public class UserControllerTests
    {
        [Fact]
        public void Logout_ReturnsOkResult()
        {
            // Arrange
            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.InvalidateJwtCookie(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(true);

            var controller = new UserController(null, cookieHelperMock.Object);

            // Act
            var result = controller.Logout();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.AuthorizeUser(It.IsAny<AuthorizationDataRequest>()))
                           .ReturnsAsync(new AuthorizeResult { Exists = true, Id = 1, Role = "Admin" });

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReturnJwtTokenString(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                            .Returns("test_token");

            var controller = new UserController(userServiceMock.Object, cookieHelperMock.Object);

            // Act
            var result = await controller.Login(new AuthorizationDataRequest());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void AddUser_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.IsEmailTaken(It.IsAny<string>()))
                           .Returns(false);

            userServiceMock.Setup(x => x.AddUser(It.IsAny<AuthorizationDataRequest>()))
                           .Returns(new Response());

            var cookieHelperMock = new Mock<ICookieHelper>();

            var controller = new UserController(userServiceMock.Object, cookieHelperMock.Object);

            // Act
            var result = controller.AddUser(new AuthorizationDataRequest());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetAllUsers_WithAuthorizedUser_ReturnsOkResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetAllUsers())
                           .Returns(new List<GetAllUsersResponse>());

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new ReverseJwtResult { IsValid = true });

            var controller = new UserController(userServiceMock.Object, cookieHelperMock.Object);

            // Act
            var result = controller.GetAllUsers();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetUserById_WithValidUserIdAndAuthorizedUser_ReturnsOkResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>()))
                           .Returns(new GetAllUsersResponse());

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new ReverseJwtResult { IsValid = true });

            var controller = new UserController(userServiceMock.Object, cookieHelperMock.Object);

            // Act
            var result = controller.GetUserById(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void UpdateUser_WithValidUserIdAndAdminUser_ReturnsOkResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserById(It.IsAny<int>()))
                           .Returns(new GetAllUsersResponse { Role = "Admin" });

            userServiceMock.Setup(x => x.UpdateUser(It.IsAny<UpdateUserRequest>()))
                           .Returns(true);

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new ReverseJwtResult { IsValid = true });

            var controller = new UserController(userServiceMock.Object, cookieHelperMock.Object);

            // Act
            var result = controller.UpdateUser(1, new UpdateUserRequest());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void DeleteUser_WithValidUserIdAndAdminUser_ReturnsOkResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.DeleteUser(It.IsAny<int>()))
                           .Returns(true);

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReverseJwtFromRequest(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>()))
                            .Returns(new ReverseJwtResult { IsValid = true });

            var controller = new UserController(userServiceMock.Object, cookieHelperMock.Object);

            // Act
            var result = controller.DeleteUser(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
