using AgileApp.Enums;

namespace AgileApp.Models.Tasks
{
    public class AddTaskRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public UserTaskStatus Status { get; set; }

        public int ProjectId { get; set; }

        public int UserId { get; set; }
    }
}
