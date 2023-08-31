using AgileApp.Repository.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AgileApp.Repository.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AgileDbContext _dbContext;
        private IQueryable<TaskDb> TaskEntities => _dbContext.Tasks.AsNoTracking();

        public TaskRepository(AgileDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<TaskDb> GetAllTasks(Func<TaskDb, bool> predicate) => TaskEntities.Where(predicate).ToList();

        public TaskDb GetTaskById(int id) => TaskEntities.FirstOrDefault(p => p.Id == id);

        public TaskDb GetTaskByName(string name) => TaskEntities.FirstOrDefault(
            p => p.Name.Contains(name) || p.Description.Contains(name));

        public int AddNewTask(TaskDb task)
        {
            var proj = _dbContext.Projects.Where(p => p.Id == task.ProjectId).FirstOrDefault();
            if (proj != null) 
            {
                task.ProjectId = proj.Id;
                _dbContext.Tasks.Add(task);

                _dbContext.SaveChanges();
                return task.Id;
            }

            return -1;
        }

        public int DeleteTask(int id)
        {
            var taskOld = _dbContext.Tasks.FirstOrDefault(p => p.Id == id);
            if (taskOld != null)
            {
                var filesToRm = _dbContext.Files?.Where(t => t.Task_Id == id).ToList();
                if (filesToRm != null && filesToRm.Count() > 0)
                    filesToRm.ForEach(f => { _dbContext.Remove(f); _dbContext.SaveChanges(); });

                _dbContext.Tasks.Remove(taskOld);
                return _dbContext.SaveChanges();
            }

            return 0;
        }

        public int UpdateTask(TaskDb task)
        {
            var projectToUpdate = _dbContext.Tasks.FirstOrDefault(p => p.Id == task.Id);
            if (projectToUpdate != null)
            {
                projectToUpdate.Name = task.Name;
                projectToUpdate.UserId = task.UserId;
                projectToUpdate.Status = task.Status;
                projectToUpdate.ProjectId = task.ProjectId;
                projectToUpdate.Description = task.Description;

                return _dbContext.SaveChanges();
            }

            return 0;
        }
    }
}
