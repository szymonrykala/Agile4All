using AgileApp.Enums;

namespace AgileApp.Repository.Models
{
    public class UserDb
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public UserRoleEnum Role { get; set; }

        public string Password { get; set; }

        public string Hash { get; set; }
    }
}
