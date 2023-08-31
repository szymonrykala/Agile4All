using AgileApp.Controllers.Responses;
using AgileApp.Enums;
using AgileApp.Models.Common;
using AgileApp.Models.Tasks;
using AgileApp.Repository.Tasks;
using AgileApp.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AgileApp.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(
            ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public bool DeleteTask(int id) => _taskRepository.DeleteTask(id) == 1;

        public List<TaskResponse> GetAllTasks()
        {
            var response = new List<TaskResponse>();
            var tasksDb = _taskRepository.GetAllTasks(p => !string.IsNullOrWhiteSpace(p.Name)).ToList();

            foreach (var task in tasksDb)
            {
                response.Add(new TaskResponse
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    Status = Enum.GetName(task.Status),
                    CreationDate = task.CreationDate,
                    DueDate = task.DueDate,
                    LastChangedBy = task.LastChangedBy,
                    StoryPoints = task.StoryPoints,
                    ProjectId = task.ProjectId,
                    UserId = task.UserId
                });
            }

            return response;
        }

        public IActionResult AddNewTask(AddTaskRequest task)
        {
            try
            {
                int taskId = _taskRepository.AddNewTask(new Repository.Models.TaskDb
                {
                    Name = task.Name,
                    UserId = task.UserId,
                    Status = task.Status,
                    ProjectId = task.ProjectId,
                    Description = task.Description,
                    CreationDate = DateTime.UtcNow,
                    DueDate = task?.DueDate ?? DateTime.UtcNow.AddYears(1),
                    LastChangedBy = task.UserId,
                    StoryPoints = task.StoryPoints
                });

                return taskId > 0
                    ? new OkObjectResult(Response<string>.Succeeded("The new task of ID equal to: " + taskId + " has been created"))
                    : taskId == -1 ? new NotFoundObjectResult(Response<bool>.Failed("Couldn't find a project specified")) 
                    : new InternalServerErrorObjectResult(Response<bool>.Failed("Encountered an unknown error while trying to add a new task"));
            }
            catch (Exception ex)
            {
                return new InternalServerErrorObjectResult(Response<bool>.Failed(ex.Message));
            }
        }

        public TaskResponse GetTaskById(int id)
        {
            var response = new TaskResponse();
            var taskDb = _taskRepository.GetTaskById(id);

            if (taskDb != null)
            {
                response.Id = taskDb.Id;
                response.Name = taskDb.Name;
                response.Status = Enum.GetName(taskDb.Status);
                response.CreationDate = taskDb.CreationDate;
                response.DueDate = taskDb.DueDate;
                response.LastChangedBy = taskDb.LastChangedBy;
                response.StoryPoints = taskDb.StoryPoints;
                response.Description = taskDb.Description;
                response.ProjectId = taskDb.ProjectId;
                response.UserId = taskDb.UserId;
            }

            return response;
        }

        public TaskResponse GetTaskByName(string name)
        {
            var response = new TaskResponse();
            var taskDb = _taskRepository.GetTaskByName(name);

            if (taskDb != null)
            {
                response.Id = taskDb.Id;
                response.Name = taskDb.Name;
                response.Status = Enum.GetName(taskDb.Status);
                response.CreationDate = taskDb.CreationDate;
                response.DueDate = taskDb.DueDate;
                response.LastChangedBy = taskDb.LastChangedBy;
                response.StoryPoints = taskDb.StoryPoints;
                response.Description = taskDb.Description;
                response.ProjectId = taskDb.ProjectId;
                response.UserId = taskDb.UserId;
            }

            return response;
        }

        public bool UpdateTask(UpdateTaskRequest task)
        {
            try
            {
                var dbTask = _taskRepository.GetTaskById(task.Id);

                dbTask.Name = dbTask.Name.PropertyStringCompare(task.Name);
                dbTask.Description = dbTask.Description.PropertyStringCompare(task.Description);

                // dbTask.CreationDate -> do not
                dbTask.LastChangedBy = task.LastChangedBy;

                dbTask.DueDate = task.DueDate != null && task.DueDate > DateTime.UtcNow ? task.DueDate : dbTask.DueDate;
                dbTask.StoryPoints = task.StoryPoints > 0 && task.StoryPoints != dbTask.StoryPoints ? task.StoryPoints : dbTask.StoryPoints;
                dbTask.UserId = task.UserId != null && task.UserId > 0 ? (int)task.UserId : dbTask.UserId;
                dbTask.ProjectId = task.ProjectId != null && task.ProjectId > 0 ? (int)task.ProjectId : dbTask.ProjectId;
                dbTask.Status = task.Status != null && task.Status != dbTask.Status ? (UserTaskStatus)task.Status : dbTask.Status;

                return _taskRepository.UpdateTask(dbTask) == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
