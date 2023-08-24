using AgileApp.Enums;
using AgileApp.Utils;
using System.Text.RegularExpressions;

namespace AgileApp.Models.Users
{
    public class RegistrationDataRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public UserRoleEnum Role { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(Password);
    }
}
