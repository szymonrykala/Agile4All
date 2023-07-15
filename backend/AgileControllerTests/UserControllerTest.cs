using AgileApp.Controllers;
using AgileApp.Enums;
using AgileApp.Models;
using AgileApp.Models.Common;
using AgileApp.Models.Jwt;
using AgileApp.Models.Requests;
using AgileApp.Models.Users;
using AgileApp.Services.Users;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
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
                            .Returns(new Response { IsSuccess = true });

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
                           .ReturnsAsync(new AuthorizeUserResult { Exists = true, Id = 1, Role = 1 });

            var cookieHelperMock = new Mock<ICookieHelper>();
            cookieHelperMock.Setup(x => x.ReturnJwtTokenString(It.IsAny<Microsoft.AspNetCore.Http.HttpContext>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                            .Returns(It.IsAny<string>());

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
                           .Returns(It.IsAny<string>);

            var cookieHelperMock = new Mock<ICookieHelper>();

            var controller = new UserController(userServiceMock.Object, cookieHelperMock.Object);

            // Create a valid AuthorizationDataRequest
            var request = new AuthorizationDataRequest()
            {
                // Set necessary properties here. e.g.,
                Email = "test@test.com",
                Password = "password",
                FirstName = "FirstName",
                LastName = "LastName"
                // etc.
            };

            // Act
            var result = controller.AddUser(request);

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
                            .Returns(new JwtReverseResult
                            {
                                IsValid = true,
                                Claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Hash, "hash")
                                }
                            });

            var controller = new UserController(userServiceMock.Object, cookieHelperMock.Object);

            // Setup HttpContext
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

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
                            .Returns(new JwtReverseResult { IsValid = true });

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
                           .Returns(new GetAllUsersResponse { Role = ((int)UserRoleEnum.ADMIN).ToString() });

            userServiceMock.Setup(x => x.UpdateUser(It.IsAny<UpdateUserRequest>()))
                           .Returns(true);

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

            var controller = new UserController(userServiceMock.Object, cookieHelperMock.Object);

            // Setup HttpContext
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

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
                            .Returns(new JwtReverseResult
                            {
                                IsValid = true,
                                Claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Role, ((int)UserRoleEnum.ADMIN).ToString())
                                }
                            });

            var controller = new UserController(userServiceMock.Object, cookieHelperMock.Object);

            // Setup HttpContext
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = controller.DeleteUser(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
