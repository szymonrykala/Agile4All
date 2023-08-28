using AgileApp.Models.Projects;
using AgileApp.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace AgileApp.Repository.Projects
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AgileDbContext _dbContext;
        private IQueryable<ProjectDb> ProjectEntities => _dbContext.Projects.AsNoTracking();

        public ProjectRepository(AgileDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ProjectDb> GetAllProjects(Func<ProjectDb, bool> predicate) => ProjectEntities.Where(predicate).ToList();

        public IEnumerable<Proj_UserDb> GetProjUserTable(Func<Proj_UserDb, bool> predicate) => _dbContext.Proj_Users.AsNoTracking().Where(predicate).ToList();

        public ProjectDb GetProjectById(int id) => ProjectEntities.FirstOrDefault(p => p.Id == id);

        public ProjectDb GetProjectByName(string name) => ProjectEntities.FirstOrDefault(
            p => p.Name.Contains(name) || p.Description.Contains(name));

        public int AddNewProject(ProjectDb project)
        {
            _dbContext.Projects.Add(project);
            _dbContext.SaveChanges();

            return project.Id;
        }

        public int DeleteProject(int id)
        {
            var projectOld = _dbContext.Projects.FirstOrDefault(p => p.Id == id);
            if (projectOld != null)
            {
                var toRm = _dbContext.Proj_Users.Where(p => p.Project_Id == projectOld.Id).FirstOrDefault();

                if (projectOld != null && toRm != null)
                {
                    _dbContext.Proj_Users.Remove(toRm);
                    _dbContext.Projects.Remove(projectOld);
                    return _dbContext.SaveChanges();
                }
            }

            return 0;
        }

        public int UpdateProject(ProjectDb project)
        {
            var projectToUpdate = _dbContext.Projects.FirstOrDefault(p => p.Id == project.Id);
            if (projectToUpdate != null)
            {
                projectToUpdate.Name = project.Name;
                projectToUpdate.Description = project.Description;

                return _dbContext.SaveChanges();
            }

            return 0;
        }

        public bool AddUserToProject(ProjectUserRequest request)
        {
            var projectUserTable = _dbContext.Proj_Users.FirstOrDefault(p => p.Project_Id == request.ProjectId && p.User_Id == request.UserId);
            if (projectUserTable != null)
            {
                return false;
            }

            _dbContext.Proj_Users.Add(new Proj_UserDb { Project_Id = request.ProjectId, User_Id = request.UserId });
            return _dbContext.SaveChanges() == 1;
        }

        public bool RemoveUserFromProject(ProjectUserRequest request)
        {
            var projectUserTable = _dbContext.Proj_Users.FirstOrDefault(p => p.Project_Id == request.ProjectId && p.User_Id == request.UserId);
            if (projectUserTable != null)
            {
                _dbContext.Proj_Users.Remove(projectUserTable);
                return _dbContext.SaveChanges() == 1;
            }

            return false;
        }
    }
}
