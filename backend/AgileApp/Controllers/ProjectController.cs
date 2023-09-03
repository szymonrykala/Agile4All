using AgileApp.Controllers.Responses;
using AgileApp.Models.Common;
using AgileApp.Models.Projects;
using AgileApp.Models.Tasks;
using AgileApp.Services.Projects;
using AgileApp.Services.Tasks;
using AgileApp.Services.Users;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AgileApp.Controllers
{
    [Route("projects/")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly ICookieHelper _cookieHelper;
        private readonly ITaskService _taskService;

        public ProjectController(
            IProjectService projectService,
            IUserService userService,
            ICookieHelper cookieHelper,
            ITaskService taskService)
        {
            _projectService = projectService;
            _cookieHelper = cookieHelper;
            _taskService = taskService;
            _userService = userService;
        }

        [HttpPost("")]
        public IActionResult AddProject([FromBody] AddProjectRequest request)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (request == null)
            {
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid" });
            }

            if (!reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult))
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing adding action must be an Admin" });

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new NotAcceptableObjectResult(Models.Common.Response.Failed("Mandatory field missing"));
            }

            var creationResult = _projectService.AddNewProject(request);

            if (creationResult == null || !creationResult.IsSuccess || creationResult.Data <= 0)
            {
                return new InternalServerErrorObjectResult(Models.Common.Response.Failed("Adding project internal error"));
            }

            var result = AddUserToProject(creationResult.Data, JwtMiddleware.GetCurrentUserId(reverseTokenResult));

            if (result == null || result is not OkObjectResult)
                return new InternalServerErrorObjectResult(Response<bool>.Failed("A new project of ID=" 
                    + creationResult.Data + " has been added, but there the server encountered a problem when trying to assign you to the project"));
            return new OkObjectResult(Response<string>.Succeeded("The project has been created. The given ID is equal to: " + creationResult.Data));
        }

        [HttpPost("{projectId}/tasks")]
        public IActionResult AddTask(int projectId, [FromBody] AddTaskRequest request)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (request == null) return new BadRequestObjectResult(Response<bool>.Failed("Request must be valid"));
            if (!reverseTokenResult.IsValid) return new UnauthorizedObjectResult(Response<bool>.Failed("User performing task adding action must be logged in"));

            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Description))
            {
                return new NotAcceptableObjectResult(Models.Common.Response.Failed("Mandatory field is missing"));
            }

            // może nie wygląda jak by to miało sens, ale serwis wrzuci userowi ID 0 jak nie znajdzie
            if (request.UserId < 1 || _userService.GetUserById(request.UserId).Id < 1)
                return new NotAcceptableObjectResult(Models.Common.Response.Failed("UserID must be valid"));

            request.ProjectId = projectId;
            var creationResult = _taskService.AddNewTask(request);

            if (creationResult == null)
                return new InternalServerErrorObjectResult(Models.Common.Response.Failed("Adding task internal error"));
            else
                return creationResult;
        }

        [HttpGet("")]
        public IActionResult GetAllProjects(int userId = 0)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (!reverseTokenResult.IsValid) return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing adding action must be logged in" });

            string hash = reverseTokenResult.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Hash)?.Value;

            if (string.IsNullOrWhiteSpace(hash))
            {
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "User performing adding action must be logged in" });
            }

            var projects = userId < 1 ? _projectService.GetAllProjects() : _projectService.GetAllProjects().Where(p => p.Users.Any(u => u.Id == userId)).ToList();
            if (projects == null || projects.Count() < 1)
            {
                return new OkObjectResult(Response<List<ProjectResponse>>.Succeeded(new List<ProjectResponse>()));
            }

            return new OkObjectResult(Response<List<ProjectResponse>>.Succeeded(projects));
        }

        [HttpGet("{projectId}")]
        public IActionResult GetProjectById(int projectId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (projectId < 1)
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid" });
            if (reverseTokenResult == null || !reverseTokenResult.IsValid)
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing adding action must be logged in" });

            var result = _projectService.GetProjectById(projectId);
            return result != null && result.Id > 0
                ? new OkObjectResult(result)
                : new NotFoundObjectResult(Response<ProjectResponse>.Failed("Project not found"));
        }

        [HttpPatch("{projectId}")]
        public IActionResult UpdateProject(int projectId, [FromBody] UpdateProjectRequest request)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (request == null)
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid" });

            if (!reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult))
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing adding action must be an Admin" });

            if (GetProjectById(projectId) is not OkObjectResult)
                return new NotAcceptableObjectResult(Response<bool>.Failed("Project with given ID does not exist"));

            var projectUpdate = new UpdateProjectRequest();
            try
            {
                projectUpdate.Id = projectId;
                projectUpdate.Name = request.Name ?? string.Empty;
                projectUpdate.Description = request.Description ?? string.Empty;

                var result = _projectService.UpdateProject(projectUpdate);
                return result.IsSuccess
                    ? new OkObjectResult(result)
                    : new InternalServerErrorObjectResult(result);
            }
            catch (Exception)
            {
                return new InternalServerErrorObjectResult(new Response { IsSuccess = false, Error = "An exception occured during updating the project" });
            }
        }

        [HttpDelete("{projectId}")]
        public IActionResult DeleteProject(int projectId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (projectId < 1)
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid, check if the project id is correct" });
            if (!reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult))
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing adding action must be an Admin" });

            var checkProject = GetProjectById(projectId);
            if (checkProject is not OkObjectResult)
                return checkProject;

            var result = _projectService.DeleteProject(projectId);
            return result
                ? new OkObjectResult(Response<bool>.Succeeded(result))
                : new InternalServerErrorObjectResult(Response<bool>.Failed("An internal error occured during deletion process"));
        }

        [HttpPut("{projectId}/users/{userId}")]
        public IActionResult AddUserToProject(int projectId, int userId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (projectId < 1 || userId < 1)
            {
                return new BadRequestObjectResult(Response<bool>.Failed("Wrong ProjectID or userID during adding to the project"));
            }

            if (!reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult))
                return new UnauthorizedObjectResult(Response<bool>.Failed("User performing adding action must be an Admin"));

            if (_projectService.GetProjectById(projectId).Id < 1)
                return new NotAcceptableObjectResult(Response<bool>.Failed("Given ProjectID is not valid"));

            if (_userService.GetUserById(userId).Id < 1)
                return new NotAcceptableObjectResult(Response<bool>.Failed("Given UserID is not valid"));

            var result = _projectService.AddUserToProject(new ProjectUserRequest { ProjectId = projectId, UserId = userId });
            return result.IsSuccess
                ? new OkObjectResult(result)
                : new InternalServerErrorObjectResult(result);
        }

        [HttpDelete("{projectId}/users/{userId}")]
        public IActionResult RemoveUserFromProject(int projectId, int userId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (!reverseTokenResult.IsValid)
            {
                return new UnauthorizedObjectResult(Response<bool>.Failed("User must be logged in"));
            }

            if (projectId < 1 || userId < 1)
            {
                return new BadRequestObjectResult(Response<bool>.Failed("Passed data must be valid"));
            }

            if (!JwtMiddleware.IsAdmin(reverseTokenResult))
                return new UnauthorizedObjectResult(Response<bool>.Failed("User performing deletion process must be an Admin"));

            if (_projectService.GetProjectById(projectId).Id < 1)
                return new NotAcceptableObjectResult(Response<bool>.Failed("Given ProjectID is not valid"));

            if (_userService.GetUserById(userId).Id < 1)
                return new NotAcceptableObjectResult(Response<bool>.Failed("Given UserID is not valid"));

            var response = _projectService.RemoveUserFromProject(new ProjectUserRequest { ProjectId = projectId, UserId = userId });
            return response.IsSuccess
                ? new OkObjectResult(response)
                : new InternalServerErrorObjectResult(response);
        }
    }
}
