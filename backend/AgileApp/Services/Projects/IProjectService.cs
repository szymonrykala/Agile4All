using AgileApp.Models.Common;
using AgileApp.Models.Projects;

namespace AgileApp.Services.Projects
{
    public interface IProjectService
    {
        public List<ProjectResponse> GetAllProjects();

        public Response<int> AddNewProject(AddProjectRequest project);

        public Response AddUserToProject(ProjectUserRequest request);

        public Response RemoveUserFromProject(ProjectUserRequest request);

        public ProjectResponse GetProjectById(int id);

        public ProjectResponse GetProjectByName(string name);

        public Response<string> UpdateProject(UpdateProjectRequest project);

        public bool DeleteProject(int id);
    }
}
