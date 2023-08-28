using AgileApp.Controllers.Responses;
using AgileApp.Models.Common;
using AgileApp.Models.Files;
using AgileApp.Services.Files;
using Microsoft.AspNetCore.Mvc;

namespace AgileApp.Controllers
{
    [Route("files/")]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet()]
        public IActionResult GetFiles([FromQuery] int taskId = -1, [FromQuery] int projectId = -1)
        {

            if (projectId == -1 && taskId == -1) 
                return new BadRequestObjectResult(new Response { IsSuccess = false, Error = "Request must be valid" });

            var res = _fileService.GetFiles(taskId, projectId);

            return res != null && res.Count() > 0
                ? new OkObjectResult(Models.Common.Response<List<GetFileResponse>>.Succeeded(res))
                : new NotFoundObjectResult(new Response { IsSuccess = false, Error = "Files with given request not found" });
        }

        [HttpPost("")]
        public IActionResult UploadFile([FromForm] UploadFileRequest request)
        {
            var result = _fileService.UploadFile(request);
            return result.IsSuccess
                ? new OkObjectResult(_fileService.UploadFile(request))
                : new InternalServerErrorObjectResult(result);
        }

        [HttpGet("{fileId}")]
        public IActionResult GetFileById(int fileId)
        {
            string filepath = _fileService.GetFileById(fileId);

            return string.IsNullOrWhiteSpace(filepath)
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
