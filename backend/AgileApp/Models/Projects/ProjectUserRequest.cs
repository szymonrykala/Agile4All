using System.ComponentModel.DataAnnotations;

namespace AgileApp.Models.Projects
{
    public class ProjectUserRequest
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int UserId{ get; set; }
    }
}
