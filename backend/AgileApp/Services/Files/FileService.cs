using AgileApp.Models.Common;
using AgileApp.Models.Files;
using AgileApp.Repository.Files;
using AgileApp.Repository.Models;

namespace AgileApp.Services.Files
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;

        public FileService(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        public Response<bool> UploadFile(Models.Files.UploadFileRequest file)
        {
            if (file == null || file.FileData == null)
                return Response<bool>.Failed("The file is too large or missing");

            try
            {
                int projectId = file?.ProjectId ?? 0;
                int taskId = file?.TaskId ?? 0;
                var uploadPath = Path.Combine("Upload", projectId.ToString(), taskId.ToString());
                var fileFullPath = Path.Combine(uploadPath, file.FileData.FileName);

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                if (File.Exists(fileFullPath))
                    return Response<bool>.Failed("Error: file already exists");

                using (FileStream fs = new FileStream(fileFullPath, FileMode.CreateNew, FileAccess.Write))
                {
                    var stream = new MemoryStream();
                    file.FileData.CopyTo(stream);
                    fs.Write(stream.ToArray(), 0, (int)file.FileData.Length);
                    fs.Close();
                }

                if (File.Exists(fileFullPath))
                {
                    FileDb fdb = new FileDb()
                    {
                        Modification_Date = DateTime.UtcNow,
                        Name = file.FileData.FileName,
                        Path = fileFullPath,
                        Project_Id = projectId,
                        Task_Id = taskId,
                        User_Id = file.UserId
                    };

                    _fileRepository.UploadFile(fdb);

                    return Response<bool>.Succeeded(true);
                }
                else
                {
                    return Response<bool>.Failed("Error: file disappeared");
                }
            }
            catch (Exception)
            {
                return Response<bool>.Failed("Internal error");
            }
        }

        public string GetFileById(int id)
        {
            var fileDb = _fileRepository.GetFileById(id);

            if (fileDb == null)
                return string.Empty;

            return fileDb.Path;
        }

        public Response DeleteFile(int id)
        {
            return _fileRepository.DeleteFile(id);
        }

        public List<GetFileResponse> GetFiles(int taskId, int projectId)
        {
            var repositoryRes = new List<FileDb>();
            var response = new List<GetFileResponse>();

            if ((projectId != -1 && taskId != -1) || taskId != -1)
                repositoryRes = _fileRepository.GetAllFiles(f => f.Task_Id == taskId).ToList();

            else if (projectId != -1)
                repositoryRes = _fileRepository.GetAllFiles(f => f.Project_Id == projectId).ToList();
            else
            {
                return response;
            }

            foreach (var item in repositoryRes)
            {
                response.Add(new GetFileResponse { Id = item.Id, Link = item.Path, ModificationDate = item.Modification_Date, Name = item.Name, UserId = item.User_Id });
            }

            return response;
        }
    }
}
