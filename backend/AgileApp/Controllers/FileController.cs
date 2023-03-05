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

            if (projectId == -1 && taskId == -1) return BadRequest();

            var res = _fileService.GetFiles(taskId, projectId);

            return new OkObjectResult(Models.Common.Response<List<GetFileResponse>>.Succeeded(res));
        }

        [HttpPost("")]
        public IActionResult UploadFile([FromForm] UploadFileRequest request)
        {
            return new OkObjectResult(_fileService.UploadFile(request));
        }

        [HttpGet("{fileId}")]
        public IActionResult GetFileById(int fileId)
        {
            string filepath = _fileService.GetFileById(fileId);

            return string.IsNullOrWhiteSpace(filepath)
                ? NotFound()
                : File(System.IO.File.ReadAllBytes(filepath), "*/*", System.IO.Path.GetFileName(filepath));
        }

        [HttpDelete("{fileId}")]
        public IActionResult DeleteFile(int fileId)
        {
            return new OkObjectResult(_fileService.DeleteFile(fileId));
        }
    }
}
