using AgileApp.Repository.Models;
using Microsoft.EntityFrameworkCore;

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
            _dbContext.Tasks.Add(task);
            return _dbContext.SaveChanges();
        }

        public int DeleteTask(int id)
        {
            var projectOld = _dbContext.Tasks.FirstOrDefault(p => p.Id == id);
            if (projectOld != null)
            {
                _dbContext.Tasks.Remove(projectOld);
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
