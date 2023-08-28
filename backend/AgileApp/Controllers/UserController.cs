using AgileApp.Controllers.Responses;
using AgileApp.Enums;
using AgileApp.Models;
using AgileApp.Models.Common;
using AgileApp.Models.Requests;
using AgileApp.Models.Users;
using AgileApp.Services.Users;
using AgileApp.Utils;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace AgileApp.Controllers
{
    [Route("users/")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICookieHelper _cookieHelper;

        public UserController(
            IUserService userService,
            ICookieHelper cookieHelper)
        {
            _userService = userService;
            _cookieHelper = cookieHelper;
        }

        [HttpDelete("logout/")]
        public IActionResult Logout()
        {
            var response = _cookieHelper.InvalidateJwtCookie(HttpContext);

            return response.IsSuccess
                ? new OkObjectResult(_cookieHelper.InvalidateJwtCookie(HttpContext))
                : new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid" });
        }

        [HttpPost("login/")]
        public async Task<IActionResult> Login([FromBody] AuthorizationDataRequest request)
        {
            string token = string.Empty;

            if (request == null
                || string.IsNullOrWhiteSpace(request.Password)
                || string.IsNullOrWhiteSpace(request.Email))
            {
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid" });
            }

            var authorizationResult = await _userService.AuthorizeUser(request);

            if (authorizationResult == null || !authorizationResult.Exists)
            {
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "Bad credentials" });
            }

            if (authorizationResult.Exists)
            {
                token = _cookieHelper.ReturnJwtTokenString(HttpContext, request.Email, authorizationResult.Id, authorizationResult.Role);
            }

            return new OkObjectResult(Response<Models.Users.AuthorizeResult>.Succeeded(
                new Models.Users.AuthorizeResult { Token = token, UserId = authorizationResult.Id }));
        }

        [HttpPost("")]
        public IActionResult AddUser([FromBody] RegistrationDataRequest request)
        {
            if (request == null || !request.IsValid)
            {
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid" });
            }

            var isEmailTaken = _userService.IsEmailTaken(request.Email);

            if (request.Email == null || request.FirstName == null || request.LastName == null)
            {
                return new NotAcceptableObjectResult(Models.Common.Response.Failed("Mandatory field missing"));
            }

            if (string.IsNullOrWhiteSpace(request.Email) || !Regex.IsMatch(request.Email, AppSettings.EmailExpression))
            {
                return new NotAcceptableObjectResult(Models.Common.Response.Failed("Email format is not valid"));
            }

            if (isEmailTaken)
            {
                return new ConflictObjectResult(Models.Common.Response.Failed("Email taken"));
            }

            var registerResult = _userService.AddUser(request);

            if (registerResult == null)
            {
                return new InternalServerErrorObjectResult(Models.Common.Response.Failed("Registration internal error"));
            }

            return new OkObjectResult(Models.Common.Response.Succeeded());
        }

        [HttpGet("")]
        public IActionResult GetAllUsers()
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (!reverseTokenResult.IsValid)
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing adding action must be logged in" });

            string hash = reverseTokenResult.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Hash)?.Value;

            if (string.IsNullOrWhiteSpace(hash))
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing adding action must be logged in" });

            var result = _userService.GetAllUsers();
            return result?.Count() > 0
                ? new OkObjectResult(Response<List<Models.Users.GetAllUsersResponse>>.Succeeded(result))
                : new NotFoundObjectResult(new Response { IsSuccess = false, Error = "The user list is empty (for some reason)" });
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserById(int userId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (userId < 1)
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid" });
            if (!reverseTokenResult.IsValid)
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing adding action must be logged in" });

            var responseData = _userService.GetUserById(userId);
            return responseData.Id == 0
                ? new NotFoundObjectResult(Response<Models.Users.GetAllUsersResponse>.Failed("User has not been found"))
                : new OkObjectResult(Response<Models.Users.GetAllUsersResponse>.Succeeded(responseData));
        }

        [HttpPatch("{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] UpdateUserRequest request)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (request == null)
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid" });
            if (!reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult))
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing adding action must be an Admin" });


            var userUpdate = new UpdateUserDTO();
            try
            {
                userUpdate.Id = userId;
                userUpdate.FirstName = request.FirstName ?? string.Empty;
                userUpdate.LastName = request.LastName ?? string.Empty;
                userUpdate.Email = request.Email ?? string.Empty;
                userUpdate.Password = request.Password ?? string.Empty;

                userUpdate.Role = request?.Role ?? Enum.Parse<UserRoleEnum>(_userService.GetUserById(userId).Role);
            }
            catch (Exception)
            {
                return new InternalServerErrorObjectResult(Response<bool>.Failed("Error during creation"));
            }

            return new OkObjectResult(Response<bool>.Succeeded(_userService.UpdateUser(userUpdate)));
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (userId < 1) 
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid" });
            if (!reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult)) 
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing adding action must be an Admin" });

            var result = _userService.DeleteUser(userId);

            if (result)
                return new OkObjectResult(Response<bool>.Succeeded(result));
            return new InternalServerErrorObjectResult(Response<bool>.Failed("Error during deletion. Ensure that user with given Id exists"));
        }
    }
}
