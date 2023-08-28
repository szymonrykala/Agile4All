using AgileApp.Controllers.Responses;
using AgileApp.Models.Common;
using AgileApp.Models.Tasks;
using AgileApp.Services.Tasks;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AgileApp.Controllers
{
    [Route("tasks/")]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ICookieHelper _cookieHelper;

        public TaskController(
            ITaskService taskService,
            ICookieHelper cookieHelper)
        {
            _taskService = taskService;
            _cookieHelper = cookieHelper;
        }

        [HttpGet("")]
        public IActionResult GetAllTasks()
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (!reverseTokenResult.IsValid)
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing Get action must be logged in" });

            var result = _taskService.GetAllTasks();
            return result != null && result.Count() > 0
                ? new OkObjectResult(result)
                : new NotFoundObjectResult(new Response { IsSuccess = false, Error = "Tasks not found" });
        }

        [HttpGet("{taskId}")]
        public IActionResult GetTaskById(int taskId)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (taskId < 1)
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Ensure that the given taskID is valid" });
            if (!reverseTokenResult.IsValid)
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing Get action must be logged in" });

            return new OkObjectResult(_taskService.GetTaskById(taskId));
        }

        [HttpPatch("{taskId}")]
        public IActionResult UpdateTask(int taskId, [FromBody] UpdateTaskRequest request)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (request == null)
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid" });
            if (!reverseTokenResult.IsValid)
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performin an update action must be logged in" });


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

                return new OkObjectResult(_taskService.UpdateTask(taskUpdate));
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

            if (taskId < 1)
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid, ensure that given taskID is valid" });
            if (!reverseTokenResult.IsValid || !JwtMiddleware.IsAdmin(reverseTokenResult))
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing deletion action must be admin" });

            var result = _taskService.DeleteTask(taskId);
            return result
                ? new OkObjectResult(_taskService.DeleteTask(taskId))
                : new InternalServerErrorObjectResult(new Response { IsSuccess = false, Error = "An error has occured during the deletion process" });
        }
    }
}
