using AgileApp.Enums;

namespace AgileApp.Repository.Models
{
    public class TaskDb
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public UserTaskStatus Status { get; set; }

        public int? StoryPoints { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? LastChangedBy { get; set; }

        public int ProjectId { get; set; }

        public int UserId { get; set; }
    }
}
