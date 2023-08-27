using AgileApp.Controllers.Responses;
using AgileApp.Models.Common;
using AgileApp.Models.Projects;
using AgileApp.Models.Tasks;
using AgileApp.Services.Projects;
using AgileApp.Services.Tasks;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AgileApp.Controllers
{
    [Route("projects/")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ICookieHelper _cookieHelper;
        private readonly ITaskService _taskService;

        public ProjectController(
            IProjectService projectService,
            ICookieHelper cookieHelper,
            ITaskService taskService)
        {
            _projectService = projectService;
            _cookieHelper = cookieHelper;
            _taskService = taskService;
        }

        [HttpPost("")]
        public IActionResult AddProject([FromBody] AddProjectRequest request)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (request == null || !reverseTokenResult.IsValid)
            {
                return new BadRequestObjectResult("Request must be valid");
            }

            if (!JwtMiddleware.IsAdmin(reverseTokenResult))
                return new UnauthorizedObjectResult("User performing adding action must be an Admin");

            if (request.Name == null)
            {
                return new NotAcceptableObjectResult(Models.Common.Response.Failed("Mandatory field missing"));
            }

            var creationResult = _projectService.AddNewProject(request);

            if (creationResult == null || !creationResult.IsSuccess || creationResult.Data <= 0)
            {
                return new InternalServerErrorObjectResult(Models.Common.Response.Failed("Adding project internal error"));
            }

            return AddUserToProject(creationResult.Data, JwtMiddleware.GetCurrentUserId(reverseTokenResult));
        }

        [HttpPost("{projectId}/tasks")]
        public IActionResult AddTask(int projectId, [FromBody] AddTaskRequest request)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (request == null) return new BadRequestObjectResult(Response<bool>.Failed("Request must be valid"));
            if (!reverseTokenResult.IsValid) return new UnauthorizedObjectResult(Response<bool>.Failed("User performing task adding action must be logged in"));

            if (request.Name == null)
            {
                return new OkObjectResult(Models.Common.Response.Failed("Mandatory field is missing"));
            }

            request.ProjectId = projectId;
            var creationResult = _taskService.AddNewTask(request);

            if (creationResult == null)
            {
                return new InternalServerErrorObjectResult(Models.Common.Response.Failed("Adding task internal error"));
            }

            return new OkObjectResult(true);
        }

        [HttpGet("")]
        public IActionResult GetAllProjects()
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (!reverseTokenResult.IsValid) return new UnauthorizedObjectResult("User performing adding action must be logged in");

            string hash = reverseTokenResult.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Hash)?.Value;

            if (string.IsNullOrWhiteSpace(hash))
            {
                return new BadRequestObjectResult("User performing adding action must be logged in");
            }

            var projects = _projectService.GetAllProjects();
            if (projects == null)
            {
                return new NotFoundObjectResult(Response<List<ProjectResponse>>.Succeeded(new List<ProjectResponse>()));
            }

            return new OkObjectResult(Response<List<ProjectResponse>>.Succeeded(projects));
        }

        [HttpGet("{projectId}")]
        public IActionResult GetProjectById(int projectId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (projectId < 1) return new BadRequestObjectResult("Request must be valid");
            if (reverseTokenResult == null || !reverseTokenResult.IsValid) return new UnauthorizedObjectResult("User performing adding action must be logged in");

            var result = _projectService.GetProjectById(projectId);
            return result != null
                ? new OkObjectResult(result)
                : new NotFoundObjectResult(Response<ProjectResponse>.Failed("Project not found"));
        }

        [HttpPatch("{projectId}")]
        public IActionResult UpdateProject(int projectId, [FromBody] UpdateProjectRequest request)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (request == null) return new BadRequestObjectResult("Request must be valid");
            if (!reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult)) return new UnauthorizedObjectResult("User performing adding action must be an Admin");

            var projectUpdate = new UpdateProjectRequest();
            try
            {
                projectUpdate.Id = projectId;
                projectUpdate.Name = request.Name ?? string.Empty;
                projectUpdate.Description = request.Description ?? string.Empty;

                return new OkObjectResult(_projectService.UpdateProject(projectUpdate));
            }
            catch (Exception)
            {
                return new InternalServerErrorObjectResult("An exception occured during updating the project");
            }
        }

        [HttpDelete("{projectId}")]
        public IActionResult DeleteProject(int projectId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (projectId < 1) return new BadRequestObjectResult("Request must be valid, check if the project id is correct");
            if (!reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult)) return new UnauthorizedObjectResult("User performing adding action must be an Admin");

            var result = _projectService.DeleteProject(projectId);
            return result
                ? new OkObjectResult(result)
                : new InternalServerErrorObjectResult("An internal error occured during deletion process");
        }

        [HttpPut("{projectId}/users/{userId}")]
        public IActionResult AddUserToProject(int projectId, int userId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (projectId < 1 || userId < 0)
            {
                return new BadRequestObjectResult(Response<bool>.Failed("Wrong ProjectID or userID during adding to the project"));
            }

            if (!reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult))
                return new UnauthorizedObjectResult(Response<bool>.Failed("User performing adding action must be an Admin"));

            var result = _projectService.AddUserToProject(new ProjectUserRequest { ProjectId = projectId, UserId = userId });
            return result.IsSuccess
                ? new OkObjectResult(result)
                : new InternalServerErrorObjectResult(result);
        }

        [HttpDelete("{projectId}/users/{userId}")]
        public IActionResult RemoveUserFromProject(int projectId, int userId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (projectId < 1 || userId < 0 || !reverseTokenResult.IsValid)
            {
                return new BadRequestObjectResult(Response<bool>.Failed("Passed data must be valid"));
            }

            if (!JwtMiddleware.IsAdmin(reverseTokenResult))
                return new UnauthorizedObjectResult(Response<bool>.Failed("User performing deletion process must be an Admin"));

            var response = _projectService.RemoveUserFromProject(new ProjectUserRequest { ProjectId = projectId, UserId = userId });
            return response.IsSuccess
                ? new OkObjectResult(response)
                : new InternalServerErrorObjectResult(response);
        }
    }
}
