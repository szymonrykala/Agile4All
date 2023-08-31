using AgileApp.Models.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AgileApp.Services.Tasks
{
    public interface ITaskService
    {
        public List<TaskResponse> GetAllTasks();

        IActionResult AddNewTask(AddTaskRequest task);

        TaskResponse GetTaskById(int id);

        TaskResponse GetTaskByName(string name);

        bool UpdateTask(UpdateTaskRequest task);

        bool DeleteTask(int id);
    }
}
