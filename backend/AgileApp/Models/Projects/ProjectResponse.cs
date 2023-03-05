namespace AgileApp.Models.Projects
{
    public class ProjectResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<UserResponse> Users { get; set; }
    }
}
