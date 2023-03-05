using AgileApp.Enums;

namespace AgileApp.Repository.Models
{
    public class TaskDb
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public UserTaskStatus Status { get; set; }

        public int ProjectId { get; set; }

        public int UserId { get; set; }
    }
}
