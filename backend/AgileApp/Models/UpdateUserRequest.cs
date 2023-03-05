using System.ComponentModel.DataAnnotations;

namespace AgileApp.Models
{
    public class UpdateUserRequest
    {
        [Required]
        public int Id { get; set; }

        public string ? Email { get; set; }

        public string ? FirstName { get; set; }

        public string ? LastName { get; set; }

        public string ? Password { get; set; }

        public Enums.UserRoleEnum ? Role { get; set; }
    }
}
