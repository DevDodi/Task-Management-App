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
}
