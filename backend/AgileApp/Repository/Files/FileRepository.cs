using AgileApp.Models.Common;
using AgileApp.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace AgileApp.Repository.Files
{
    public class FileRepository : IFileRepository
    {
        private readonly AgileDbContext _dbContext;
        private IQueryable<FileDb> FileEntities => _dbContext.Files.AsNoTracking();

        public FileRepository(AgileDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<FileDb> GetAllFiles(Func<FileDb, bool> predicate) => FileEntities.Where(predicate);

        public FileDb GetFileById(int id) => FileEntities.FirstOrDefault(f => f.Id == id);

        public Response DeleteFile(int id)
        {
            var fileDb = _dbContext.Files.FirstOrDefault(f => f.Id == id);

            if (fileDb == null)
                return Response.Failed("File doesn't exist");

            try
            {
                File.Delete(fileDb.Path);
            }
            catch (Exception)
            {
                return Response.Failed("File cannot be deleted or is currently opened");
            }

            _dbContext.Files.Remove(fileDb);
            return _dbContext.SaveChanges() == 1
                ? Response.Succeeded()
                : Response.Failed("Error during deletion");
        }

        public bool UploadFile(FileDb file)
        {
            _dbContext.Files.Add(file);
            return _dbContext.SaveChanges() == 1;
        }
    }
}
