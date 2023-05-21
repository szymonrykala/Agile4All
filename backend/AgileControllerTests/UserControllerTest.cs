using AgileApp.Enums;
using AgileApp.Models;
using AgileApp.Models.Common;
using AgileApp.Models.Requests;
using AgileApp.Services.Users;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Mvc;
using Moq;
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

            var controller = new UserController(Mock.Of<IUserService>(), cookieHelperMock.Object);

            cookieHelperMock.Setup(helper => helper.InvalidateJwtCookie(controller.HttpContext))
                            .Returns(true);

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
            var cookieHelperMock = new Mock<ICookieHelper>();

            var controller = new UserController(userServiceMock.Object, cookieHelperMock.Object);

            var request = new AuthorizationDataRequest { Email = "test@example.com", Password = "password" };
            var authorizationResult = new AuthorizationResult { Exists = true, Id = 1, Role = UserRoleEnum.Admin };
            var expectedToken = "fakeToken";

            userServiceMock.Setup(service => service.AuthorizeUser(request))
                           .ReturnsAsync(authorizationResult);

            cookieHelperMock.Setup(helper => helper.ReturnJwtTokenString(controller.HttpContext, request.Email, authorizationResult.Id, authorizationResult.Role))
                            .Returns(expectedToken);

            // Act
            var result = await controller.Login(request);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<Models.Users.AuthorizeResult>>(okObjectResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(expectedToken, response.Data.Token);
            Assert.Equal(authorizationResult.Id, response.Data.UserId);
        }

        [Fact]
        public async Task Login_WithInvalidRequest_ReturnsBadRequestResult()
        {
            // Arrange
            var controller = new UserController(Mock.Of<IUserService>(), Mock.Of<ICookieHelper>());

            // Act
            var result = await controller.Login(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        // More tests for other actions in the UserController
    }
}
