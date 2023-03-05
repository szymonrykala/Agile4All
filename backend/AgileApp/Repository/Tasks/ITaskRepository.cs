using AgileApp.Repository.Models;

namespace AgileApp.Repository.Tasks
{
    public interface ITaskRepository
    {
        public IEnumerable<TaskDb> GetAllTasks(Func<TaskDb, bool> predicate);

        int AddNewTask(TaskDb task);

        TaskDb GetTaskById(int id);

        TaskDb GetTaskByName(string name);

        int UpdateTask(TaskDb task);

        int DeleteTask(int id);
    }
}
