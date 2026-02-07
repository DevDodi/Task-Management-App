using System.Net;
using System.Text.Json;
using TaskMangementApp.DB;
using TaskMangementApp.Models;
using TaskMangementApp.Services.Interfaces;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services
{
    public class ProjectService(AppDBContext dbContext) : IProjectService
    {
        public Task<ProjectServiceResponse> CreateProjectAsync(string projectJson)
        {
            Project? project = JsonSerializer.Deserialize<Project>(projectJson);

            if (project is null)
                return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(false));

            project.Id = project.Id == Guid.Empty || dbContext.Projects.Any(p => p.Id == project.Id) ? Guid.NewGuid() : project.Id;

            var ownerExists = dbContext.Users.Any(u => u.Id == project.OwnerId);
            if (!ownerExists)
                return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(false, null, "Assigned Owner does not exist"));

            dbContext.Projects.Add(project);
            dbContext.SaveChanges();

            return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(true));
        }

        public Task<ProjectServiceResponse> DeleteProjectAsync(Guid id)
        {
            var project = new Project { Id = id };
            dbContext.Projects.Remove(project);
            dbContext.SaveChanges();

            return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(true));
        }

        public Task<ProjectServiceResponse> GetProjectAsync(Guid id)
        {
            var project = dbContext.Projects.Find(id);

            if (project is null)
                return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(false));

            return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(true, project));
        }

        public Task<ProjectServiceResponse> UpdateProjectAsync(Guid id, string projectJson)
        {
            Project? project = JsonSerializer.Deserialize<Project>(projectJson);

            if (project is null)
                return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(false));

            var ownerExists = dbContext.Users.Any(u => u.Id == project.OwnerId);
            if (!ownerExists)
                return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(false, null, "Assigned Owner does not exist"));

            var existingProject = dbContext.Projects.FirstOrDefault(t => t.Id == id);

            if (existingProject is null)
                return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(false));

            existingProject = project;
            dbContext.SaveChanges();

            return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(true));
        }
    }
}
