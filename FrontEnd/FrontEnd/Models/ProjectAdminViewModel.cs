namespace FrontEnd.Models
{
    public class ProjectAdminViewModel
    {
        public ProjectModel Project { get; set; }
        public List<ProjectModel> AllProjects { get; set; }
        public List<MediaDbModel> MediaList { get; set; }
    }
}
