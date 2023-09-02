using AgileApp.Controllers.Responses;
using AgileApp.Models.Common;
using AgileApp.Models.Files;
using AgileApp.Services.Files;
using AgileApp.Services.Projects;
using AgileApp.Services.Tasks;
using AgileApp.Utils.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AgileApp.Controllers
{
    [Route("files/")]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        private readonly ICookieHelper _cookieHelper;
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;

        public FileController(IFileService fileService,
            ICookieHelper cookieHelper,
            ITaskService taskService,
            IProjectService projectService)
        {
            _fileService = fileService;
            _cookieHelper = cookieHelper;
            _taskService = taskService;
            _projectService = projectService;
        }

        [HttpGet()]
        public IActionResult GetFiles([FromQuery] int taskId = -1, [FromQuery] int projectId = -1)
        {

            if (projectId == -1 && taskId == -1)
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid" });

            var res = _fileService.GetFiles(taskId, projectId);

            return new OkObjectResult(Models.Common.Response<List<GetFileResponse>>.Succeeded(res));
        }

        [HttpPost("")]
        public IActionResult UploadFile([FromForm] UploadFileRequest request)
        {
            var reverseTokenResult = _cookieHelper.ReverseJwtFromRequest(HttpContext);

            if (!reverseTokenResult.IsValid)
                return new UnauthorizedObjectResult(new Response { IsSuccess = false, Error = "User performing Upload action must be logged in" });

            int userId = JwtMiddleware.GetCurrentUserId(reverseTokenResult);
            if (userId < 1)
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "User performing Upload action must be logged in" });

            if (request.TaskId == null || request.TaskId < 1 || (request.TaskId > 0 && _taskService.GetTaskById((int)request.TaskId).Id < 1))
                return new NotAcceptableObjectResult(Response<bool>.Failed("TaskID must point to a valid task"));

            if (request.ProjectId == null || request.ProjectId < 1 || (request.ProjectId > 0 && _projectService.GetProjectById((int)request.ProjectId).Id < 1))
                return new NotAcceptableObjectResult(Response<bool>.Failed("ProjectID must point to a valid project"));

            request.UserId = userId;
            var result = _fileService.UploadFile(request);
            return result.IsSuccess
                ? new OkObjectResult(result)
                : new InternalServerErrorObjectResult(result);
        }

        [HttpGet("{fileId}")]
        public IActionResult GetFileById(int fileId)
        {
            string filepath = _fileService.GetFileById(fileId);
            Response.ContentType = "application/octet-stream";
            return string.IsNullOrWhiteSpace(filepath) || !System.IO.File.Exists(filepath)
                ? new NotFoundObjectResult(new Response { IsSuccess = false, Error = "File with given ID does not exist" })
                : File(System.IO.File.ReadAllBytes(filepath), "*/*", System.IO.Path.GetFileName(filepath));
        }

        [HttpDelete("{fileId}")]
        public IActionResult DeleteFile(int fileId)
        {
            var result = _fileService.DeleteFile(fileId);
            return result.IsSuccess
                ? new OkObjectResult(result)
                : new InternalServerErrorObjectResult(result);
        }
    }
}
