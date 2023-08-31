using AgileApp.Controllers.Responses;
using AgileApp.Models.Common;
using AgileApp.Models.Tasks;
using AgileApp.Repository.Projects;
using AgileApp.Services.Projects;
using AgileApp.Services.Tasks;
using AgileApp.Services.Users;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AgileApp.Controllers
{
    [Route("tasks/")]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectRepository;
        private readonly IUserService _userService;
        private readonly ICookieHelper _cookieHelper;

        public TaskController(
            ITaskService taskService,
            IProjectService projectRepository,
            IUserService userService,
            ICookieHelper cookieHelper)
        {
            _taskService = taskService;
            _projectRepository = projectRepository;
            _cookieHelper = cookieHelper;
            _userService = userService;
        }

        [HttpGet("")]
        public IActionResult GetAllTasks()
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (!reverseTokenResult.IsValid)
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing Get action must be logged in" });

            var result = _taskService.GetAllTasks();
            return result != null && result.Count() > 0
                ? new OkObjectResult(Response<List<TaskResponse>>.Succeeded(result))
                : new NotFoundObjectResult(Response<List<TaskResponse>>.Succeeded(new List<TaskResponse>()));
        }

        [HttpGet("{taskId}")]
        public IActionResult GetTaskById(int taskId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (taskId < 1)
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Ensure that the given taskID is valid" });
            if (!reverseTokenResult.IsValid)
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing Get action must be logged in" });

            var result = _taskService.GetTaskById(taskId);
            return result.Id < 1
                ? new NotFoundObjectResult(Response<TaskResponse>.Failed("Task with given ID not found"))
                : new OkObjectResult(Response<TaskResponse>.Succeeded(result));
        }

        [HttpPatch("{taskId}")]
        public IActionResult UpdateTask(int taskId, [FromBody] UpdateTaskRequest request)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (request == null)
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid" });
            if (!reverseTokenResult.IsValid)
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performer an update action must be logged in" });

            var taskUpdate = new UpdateTaskRequest();
            try
            {
                taskUpdate.Id = taskId;
                taskUpdate.Name = request.Name ?? string.Empty;
                taskUpdate.Description = request.Description ?? string.Empty;
                taskUpdate.Status = request.Status;
                taskUpdate.UserId = request.UserId;
                taskUpdate.ProjectId = request.ProjectId;
                taskUpdate.LastChangedBy = JwtMiddleware.GetCurrentUserId(reverseTokenResult);

                var validation = ValidateTaskForUpdate(taskUpdate);
                if (validation is not OkObjectResult)
                    return validation;

                var response = _taskService.UpdateTask(taskUpdate);
                return response
                    ? new OkObjectResult(Response<bool>.Succeeded(response))
                    : new InternalServerErrorObjectResult(Response<bool>.Failed("An error occured during altering the task, please check the request data and try again"));
            }
            catch (Exception)
            {
                return new InternalServerErrorObjectResult(new Response { IsSuccess = false, Error = "An error occured during altering the task, please try again" });
            }
        }

        [HttpDelete("{taskId}")]
        public IActionResult DeleteTask(int taskId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (!reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult))
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing deletion action must be admin" });
            if (taskId < 1 || (taskId > 0 && GetTaskById(taskId) is not OkObjectResult))
                return new NotAcceptableObjectResult(Response<bool>.Failed("TaskID must point to a valid task"));

            var result = _taskService.DeleteTask(taskId);
            return result
                ? new OkObjectResult(Response<bool>.Succeeded(result))
                : new InternalServerErrorObjectResult(new Response { IsSuccess = false, Error = "An error has occured during the deletion process" });
        }

        private ObjectResult ValidateTaskForUpdate(UpdateTaskRequest task)
        {
            if (task.Id < 1 || (task.Id > 0 && GetTaskById(task.Id) is not OkObjectResult))
                return new NotAcceptableObjectResult(Response<bool>.Failed("TaskID must point to a valid task"));

            if (task.ProjectId > 0 && _projectRepository.GetProjectById((int)task.ProjectId).Id < 1)
                return new NotAcceptableObjectResult(Response<bool>.Failed("ProjectID must point to a valid project"));

            if (task.UserId < 1 || (task.UserId > 0 && _userService.GetUserById((int)task.UserId).Id < 1))
                return new NotAcceptableObjectResult(Response<bool>.Failed("UserID must point to an existing user"));

            return new OkObjectResult(true);
        }
    }
}
