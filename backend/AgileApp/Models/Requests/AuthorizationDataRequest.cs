using AgileApp.Enums;

namespace AgileApp.Models.Requests
{
    public class AuthorizationDataRequest : BaseDataRequest
    {
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public UserRoleEnum Role { get; set; }

        public override bool IsValid => base.IsValid && !string.IsNullOrWhiteSpace(Password);
    }
}
