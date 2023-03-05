using AgileApp.Enums;
using AgileApp.Models;
using AgileApp.Models.Requests;
using AgileApp.Repository.Users;
using AgileApp.Utils;

namespace AgileApp.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AuthorizeUserResult> AuthorizeUser(AuthorizationDataRequest request)
        {
            try
            {
                var requestedUser = _userRepository
                    .GetAllUsers(u => request.Email == u.Email && request.Password == u.Password)
                    .FirstOrDefault();

                return requestedUser != null
                    ? AuthorizeUserResult.Exist(requestedUser.Id, (int)requestedUser.Role)
                    : AuthorizeUserResult.NotExist();
            }
            catch (Exception ex)
            {
                return new AuthorizeUserResult();
            }
        }

        public List<Models.Users.GetAllUsersResponse> GetAllUsers()
        {
            var response = new List<Models.Users.GetAllUsersResponse>();
            var usersDb = _userRepository.GetAllUsers();

            foreach (var user in usersDb)
                response.Add(new Models.Users.GetAllUsersResponse { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, Role = Enum.GetName(user.Role) });

            return response;
        }

        public string AddUser(AuthorizationDataRequest request)
        {
            try
            {
                string clientHash = Guid.NewGuid().ToString();

                int affectedRows = _userRepository.AddNewUser(new Repository.Models.UserDb
                {
                    Hash = clientHash,
                    Email = request.Email,
                    Password = request.Password,
                    LastName = request.LastName,
                    FirstName = request.FirstName,
                    Role = UserRoleEnum.STUDENT
                });

                return affectedRows == 1
                    ? clientHash
                    : "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public bool DeleteUser(int id) => _userRepository.DeleteUser(id) == 1;

        public Models.Users.GetAllUsersResponse GetUserByEmail(string email)
        {
            var response = new Models.Users.GetAllUsersResponse();
            var userDb = _userRepository.GetUserByEmail(email);

            if (userDb != null)
            {
                response.Id = userDb.Id;
                response.Email = userDb.Email;
                response.LastName = userDb.LastName;
                response.FirstName = userDb.FirstName;
                response.Role = Enum.GetName(userDb.Role);
            }

            return response;
        }

        public Models.Users.GetAllUsersResponse GetUserById(int id)
        {
            var response = new Models.Users.GetAllUsersResponse();
            var userDb = _userRepository.GetUserById(id);

            if (userDb != null)
            {
                response.Id = userDb.Id;
                response.Email = userDb.Email;
                response.LastName = userDb.LastName;
                response.FirstName = userDb.FirstName;
                response.Role = Enum.GetName(userDb.Role);
            }

            return response;
        }

        public Models.Users.GetAllUsersResponse GetUserByName(string userName)
        {
            var response = new Models.Users.GetAllUsersResponse();
            var userDb = _userRepository.GetUserByName(userName);

            if (userDb != null)
            {
                response.Id = userDb.Id;
                response.Email = userDb.Email;
                response.LastName = userDb.LastName;
                response.FirstName = userDb.FirstName;
                response.Role = Enum.GetName(userDb.Role);
            }

            return response;
        }

        public bool UpdateUser(UpdateUserRequest user)
        {
            try
            {
                var dbUser = _userRepository.GetUserById(user.Id);

                dbUser.Email = dbUser.Email.PropertyStringCompare(user.Email);
                dbUser.LastName = dbUser.LastName.PropertyStringCompare(user.LastName);
                dbUser.FirstName = dbUser.FirstName.PropertyStringCompare(user.FirstName);

                dbUser.Role = user.Role != null && user.Role != dbUser.Role ? (UserRoleEnum)user.Role : dbUser.Role;

                return _userRepository.UpdateUser(dbUser) == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsEmailTaken(string email)
        {
            try
            {
                return _userRepository.IsEmailAlreadyUsed(email);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
