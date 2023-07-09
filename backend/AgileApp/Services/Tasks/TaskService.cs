﻿using AgileApp.Enums;
using AgileApp.Models.Tasks;
using AgileApp.Repository.Tasks;
using AgileApp.Repository.Users;
using AgileApp.Utils;
using Microsoft.VisualBasic;
using System.Text;
using System.Threading.Tasks;

namespace AgileApp.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(
            ITaskRepository taskRepository,
            IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }
        public bool DeleteTask(int id) => _taskRepository.DeleteTask(id) == 1;

        public List<TaskResponse> GetAllTasks()
        {
            var response = new List<TaskResponse>();
            var tasksDb = _taskRepository.GetAllTasks(p => !string.IsNullOrWhiteSpace(p.Name)).ToList();

            foreach (var task in tasksDb)
            {
                var user = _userRepository?.GetUserById(task?.LastChangedBy ?? -1);
                var username = new StringBuilder(user?.FirstName ?? "");
                username.Append(" ");
                username.Append(user?.LastName ?? "");

                response.Add(new TaskResponse
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    Status = Enum.GetName(task.Status),
                    CreationDate = task.CreationDate,
                    DueDate = task.DueDate,
                    LastChangedBy = username.ToString(),
                    StoryPoints = task.StoryPoints,
                    ProjectId = task.ProjectId,
                    UserId = task.UserId
                });
            }

            return response;
        }

        public string AddNewTask(AddTaskRequest task)
        {
            try
            {
                int affectedRows = _taskRepository.AddNewTask(new Repository.Models.TaskDb
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

                return affectedRows == 1
                    ? "true"
                    : "false";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public TaskResponse GetTaskById(int id)
        {
            var response = new TaskResponse();
            var taskDb = _taskRepository.GetTaskById(id);

            var user = _userRepository?.GetUserById(taskDb?.LastChangedBy ?? -1);
            var username = new StringBuilder(user?.FirstName ?? "");
            username.Append(" ");
            username.Append(user?.LastName ?? "");

            if (taskDb != null)
            {
                response.Id = taskDb.Id;
                response.Name = taskDb.Name;
                response.Status = Enum.GetName(taskDb.Status);
                response.CreationDate = taskDb.CreationDate;
                response.DueDate = taskDb.DueDate;
                response.LastChangedBy = username.ToString();
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

            var user = _userRepository?.GetUserById(taskDb?.LastChangedBy ?? -1);
            var username = new StringBuilder(user?.FirstName ?? "");
            username.Append(" ");
            username.Append(user?.LastName ?? "");

            if (taskDb != null)
            {
                response.Id = taskDb.Id;
                response.Name = taskDb.Name;
                response.Status = Enum.GetName(taskDb.Status);
                response.CreationDate = taskDb.CreationDate;
                response.DueDate = taskDb.DueDate;
                response.LastChangedBy = username.ToString();
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
