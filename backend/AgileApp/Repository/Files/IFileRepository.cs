using AgileApp.Models.Common;
using AgileApp.Repository.Models;

namespace AgileApp.Repository.Files
{
    public interface IFileRepository
    {
        public bool UploadFile(FileDb file);

        public IEnumerable<FileDb> GetAllFiles(Func<FileDb, bool> predicate);

        public FileDb GetFileById(int id);

        public Response DeleteFile(int id);
    }
}
