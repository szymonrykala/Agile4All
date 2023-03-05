using AgileApp.Models.Projects;
using AgileApp.Repository.Models;

namespace AgileApp.Repository.Projects
{
    public interface IProjectRepository
    {
        public IEnumerable<ProjectDb> GetAllProjects(Func<ProjectDb, bool> predicate);

        public IEnumerable<Proj_UserDb> GetProjUserTable(Func<Proj_UserDb, bool> predicate);

        public int AddNewProject(ProjectDb project);

        public bool AddUserToProject(ProjectUserRequest request);

        public bool RemoveUserFromProject(ProjectUserRequest request);

        public ProjectDb GetProjectById(int id);

        public ProjectDb GetProjectByName(string name);

        public int UpdateProject(ProjectDb project);

        public int DeleteProject(int id);
    }
}
