using TaskMangementApp.Models;

namespace TaskMangementApp.Services.Responses
{
    public class ProjectServiceResponse : BaseServiceResponse
    {
        public Project? Project { get; set; }

        public ProjectServiceResponse(bool success, Project? project = null, string? message = null) : base(success, message)
        {
            Project = project;
        }
    }

    public class ProjectServiceResponseList : BaseServiceResponse
    {
        public List<Project?> Projects { get; set; }

        public ProjectServiceResponseList(bool success, List<Project?> projects, string? message = null) : base(success, message)
        {
            Projects = projects;
        }
    }
}
