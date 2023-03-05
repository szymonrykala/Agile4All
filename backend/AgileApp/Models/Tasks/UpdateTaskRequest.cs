using AgileApp.Enums;

namespace AgileApp.Models.Tasks
{
    public class UpdateTaskRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public UserTaskStatus? Status { get; set; }

        public int? ProjectId { get; set; }

        public int? UserId { get; set; }
    }
}
