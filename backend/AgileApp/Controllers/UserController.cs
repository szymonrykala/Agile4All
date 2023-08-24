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
        public IActionResult Logout() => new OkObjectResult(_cookieHelper.InvalidateJwtCookie(HttpContext));

        [HttpPost("login/")]
        public async Task<IActionResult> Login([FromBody] AuthorizationDataRequest request)
        {
            string token = string.Empty;

            if (request == null)
            {
                return BadRequest();
            }

            var authorizationResult = await _userService.AuthorizeUser(request);

            if (authorizationResult == null || !authorizationResult.Exists)
            {
                return new OkObjectResult(new Response { IsSuccess = false, Error = "Bad credentials" });
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
                return BadRequest();
            }

            var isEmailTaken = _userService.IsEmailTaken(request.Email);

            if (request.Email == null || request.FirstName == null || request.LastName == null)
            {
                return new OkObjectResult(Models.Common.Response.Failed("Mandatory field missing"));
            }

            if (string.IsNullOrWhiteSpace(request.Email) || !Regex.IsMatch(request.Email, AppSettings.EmailExpression))
            {
                return new OkObjectResult(Models.Common.Response.Failed("Email format is uncorrect"));
            }

            if (isEmailTaken)
            {
                return new OkObjectResult(Models.Common.Response.Failed("Email taken"));
            }

            var registerResult = _userService.AddUser(request);

            if (registerResult == null)
            {
                return new OkObjectResult(Models.Common.Response.Failed("Registration internal error"));
            }

            return new OkObjectResult(Models.Common.Response.Succeeded());
        }

        [HttpGet("")]
        public IActionResult GetAllUsers()
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (!reverseTokenResult.IsValid) return Unauthorized();

            string hash = reverseTokenResult.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Hash)?.Value;

            return string.IsNullOrWhiteSpace(hash)
                ? (IActionResult)new BadRequestResult()
                : new OkObjectResult(Response<List<Models.Users.GetAllUsersResponse>>.Succeeded(_userService.GetAllUsers()));
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserById(int userId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (userId < 1) return BadRequest();
            if (!reverseTokenResult.IsValid) return Unauthorized();

            var responseData = _userService.GetUserById(userId);
            return new OkObjectResult(responseData.Id == 0
                ? Response<Models.Users.GetAllUsersResponse>.Failed("User has not been found")
                : Response<Models.Users.GetAllUsersResponse>.Succeeded(responseData));
        }

        [HttpPatch("{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] UpdateUserRequest request)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (request == null) return BadRequest();
            if (!reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult)) return Unauthorized();


            var userUpdate = new UpdateUserDTO();
            try
            {
                userUpdate.Id = userId;
                userUpdate.FirstName = request.FirstName ?? string.Empty;
                userUpdate.LastName = request.LastName ?? string.Empty;
                userUpdate.Email = request.Email ?? string.Empty;
                userUpdate.Password = request.Password ?? string.Empty;

                userUpdate.Role = request.Role ?? Enum.Parse<UserRoleEnum>(_userService.GetUserById(userId).Role);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return new OkObjectResult(Response<bool>.Succeeded(_userService.UpdateUser(userUpdate)));
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (userId < 1) return BadRequest();
            if (!reverseTokenResult.IsValid) return Unauthorized(); // || !JwtMiddleware.IsAdmin(reverseTokenResult)

            var result = _userService.DeleteUser(userId);

            return new OkObjectResult(result ? Response<bool>.Succeeded(result) : Response<bool>.Failed("Error during deletion. Ensure that user with given Id exists"));
        }
    }
}
