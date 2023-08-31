using Microsoft.EntityFrameworkCore;
using AgileApp.Repository.Models;
using AgileApp.Models;

namespace AgileApp.Repository.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AgileDbContext _dbContext;
        private IQueryable<Models.UserDb> UserEntities => _dbContext.Users.AsNoTracking();
        public IEnumerable<Proj_UserDb> GetProjUserTable(Func<Proj_UserDb, bool> predicate) => _dbContext.Proj_Users.AsNoTracking().Where(predicate).ToList();

        public UserRepository(AgileDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<UserDb> GetAllUsers() => UserEntities.ToList();

        public IEnumerable<UserDb> GetAllUsers(Func<UserDb, bool> predicate) => UserEntities.Where(predicate).ToList();

        public UserDb GetUserById(int id) => UserEntities.FirstOrDefault(u => u.Id == id);

        public UserDb GetUserByName(string name) => UserEntities.FirstOrDefault(
            u => u.FirstName.Contains(name) || u.LastName.Contains(name));

        public UserDb GetUserByEmail(string email) => UserEntities.FirstOrDefault(u => u.Email == email);

        public bool IsEmailAlreadyUsed(string email) => UserEntities.Any(u => u.Email == email);

        public int AddNewUser(UserDb user)
        {
            _dbContext.Users.Add(user);
            return _dbContext.SaveChanges();
        }

        public int UpdateUser(UserDb user)
        {
            var userToUpdate = _dbContext.Users.FirstOrDefault(u => u.Id == user.Id);
            if (userToUpdate != null)
            {
                userToUpdate.FirstName = user.FirstName;
                userToUpdate.LastName = user.LastName;
                userToUpdate.Email = user.Email;

                userToUpdate.Password = user.Password;
                userToUpdate.Role = user.Role;

                return _dbContext.SaveChanges();
            }

            return 0;
        }

        public int DeleteUser(int id)
        {
            // relations to be checked
            var userOld = _dbContext.Users.FirstOrDefault(u => u.Id == id);
            var projUsers_userOld = _dbContext.Proj_Users.Where(u => u.User_Id == id).ToList();
            var tasts_userOld = _dbContext.Tasks.Where(u => u.UserId == id).ToList();
            if (userOld != null)
            {
                if (projUsers_userOld.Count() > 0)
                    projUsers_userOld.ForEach(item => _dbContext.Proj_Users.Remove(item));

                if (tasts_userOld.Count() > 0)
                    tasts_userOld.ForEach(item => _dbContext.Tasks.Remove(item));

                _dbContext.Users.Remove(userOld);
                return _dbContext.SaveChanges();
            }

            return 0;
        }

        public IEnumerable<UserResponse> GetAllUsersOnProject(int id)
        {
            var users = GetProjUserTable(u => u.Project_Id == id);
            var response = new List<UserResponse>();

            if (users == null)
                return response;

            foreach (var user in users)
            {
                var temp = GetUserById(user.User_Id);
                response.Add(new UserResponse { Id = temp.Id, Email = temp.Email, FirstName = temp.FirstName, LastName = temp.LastName, Role = temp.Role });
            }

            return response;
        }
    }
}
