using AgileApp.Models;
using AgileApp.Models.Requests;
using AgileApp.Models.Users;

namespace AgileApp.Services.Users
{
    public interface IUserService
    {
        Task<AuthorizeUserResult> AuthorizeUser(AuthorizationDataRequest request);

        //response may be changed after the design!!!
        public List<Models.Users.GetAllUsersResponse> GetAllUsers();

        public Models.Users.GetAllUsersResponse GetUserById(int id);

        public Models.Users.GetAllUsersResponse GetUserByName(string userName);

        public Models.Users.GetAllUsersResponse GetUserByEmail(string email);

        public string AddUser(RegistrationDataRequest request);

        public bool UpdateUser(UpdateUserDTO user);

        public bool DeleteUser(int id);

        public bool IsEmailTaken(string email);
    }
}
