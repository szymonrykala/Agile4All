using AgileApp.Models.Common;
using AgileApp.Models.Files;

namespace AgileApp.Services.Files
{
    public interface IFileService
    {
        public Response<bool> UploadFile(Models.Files.UploadFileRequest file);

        public string GetFileById(int id);

        public Response DeleteFile(int id);

        public List<GetFileResponse> GetFiles(int taskId, int projectId);
    }
}
