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

            if (request == null || !reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult))
            {
                return BadRequest();
            }

            if (request.Name == null)
            {
                return new OkObjectResult(Models.Common.Response.Failed("Mandatory field missing"));
            }

            var creationResult = _projectService.AddNewProject(request);

            if (creationResult == null || !creationResult.IsSuccess || creationResult.Data <= 0)
            {
                return new OkObjectResult(Models.Common.Response.Failed("Adding project internal error"));
            }
            else
            {
                AddUserToProject(creationResult.Data, JwtMiddleware.GetCurrentUserId(reverseTokenResult));
            }

            return new OkObjectResult(true);
        }

        [HttpPost("{projectId}/tasks")]
        public IActionResult AddTask(int projectId, [FromBody] AddTaskRequest request)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (request == null) return BadRequest();
            if (!reverseTokenResult.IsValid) return Unauthorized();


            if (request.Name == null)
            {
                return new OkObjectResult(Models.Common.Response.Failed("Mandatory field missing"));
            }

            request.ProjectId = projectId;
            var creationResult = _taskService.AddNewTask(request);

            if (creationResult == null)
            {
                return new OkObjectResult(Models.Common.Response.Failed("Adding task internal error"));
            }

            return new OkObjectResult(true);
        }

        [HttpGet("")]
        public IActionResult GetAllProjects()
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (!reverseTokenResult.IsValid) return Unauthorized();


            string hash = reverseTokenResult.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Hash)?.Value;

            if (string.IsNullOrWhiteSpace(hash))
            {
                return new BadRequestResult();
            }

            var projects = _projectService.GetAllProjects();
            if (projects == null)
            {
                return new OkObjectResult(Response<List<ProjectResponse>>.Succeeded(new List<ProjectResponse>()));
            }

            return new OkObjectResult(Response<List<ProjectResponse>>.Succeeded(projects));
        }

        [HttpGet("{projectId}")]
        public IActionResult GetProjectById(int projectId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (projectId < 1) return BadRequest();
            if (!reverseTokenResult.IsValid) return Unauthorized();


            return new OkObjectResult(_projectService.GetProjectById(projectId));
        }

        [HttpPatch("{projectId}")]
        public IActionResult UpdateProject(int projectId, [FromBody] UpdateProjectRequest request)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (request == null) return BadRequest();
            if (!reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult)) return Unauthorized();


            var projectUpdate = new UpdateProjectRequest();
            try
            {
                projectUpdate.Id = projectId;
                projectUpdate.Name = request.Name ?? string.Empty;
                projectUpdate.Description = request.Description ?? string.Empty;
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return new OkObjectResult(_projectService.UpdateProject(projectUpdate));
        }

        [HttpDelete("{projectId}")]
        public IActionResult DeleteProject(int projectId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (projectId < 1) return BadRequest();
            if (!reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult)) return Unauthorized();


            return new OkObjectResult(_projectService.DeleteProject(projectId));
        }

        [HttpPut("{projectId}/users/{userId}")]
        public IActionResult AddUserToProject(int projectId, int userId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (projectId < 1 || userId < 0 || !reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult))
            {
                return BadRequest();
            }

            return new OkObjectResult(_projectService.AddUserToProject(new ProjectUserRequest { ProjectId = projectId, UserId = userId }));
        }

        [HttpDelete("{projectId}/users/{userId}")]
        public IActionResult RemoveUserFromProject(int projectId, int userId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (projectId < 1 || userId < 0 || !reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult))
            {
                return BadRequest();
            }

            return new OkObjectResult(_projectService.RemoveUserFromProject(new ProjectUserRequest { ProjectId = projectId, UserId = userId }));
        }
    }
}
