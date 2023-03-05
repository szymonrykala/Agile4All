using AgileApp.Models.Tasks;

namespace AgileApp.Services.Tasks
{
    public interface ITaskService
    {
        public List<TaskResponse> GetAllTasks();

        string AddNewTask(AddTaskRequest task);

        TaskResponse GetTaskById(int id);

        TaskResponse GetTaskByName(string name);

        bool UpdateTask(UpdateTaskRequest task);

        bool DeleteTask(int id);
    }
}
