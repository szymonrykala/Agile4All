namespace AgileApp.Models
{
    public class AuthorizeUserResult
    {
        public bool Exists { get; set; }

        public int Id { get; set; }

        public int Role { get; set; }

        public static AuthorizeUserResult NotExist() => new AuthorizeUserResult { Exists = false };

        public static AuthorizeUserResult Exist(int id, int role) => new AuthorizeUserResult { Exists = true, Id = id, Role = role};
    }
}
