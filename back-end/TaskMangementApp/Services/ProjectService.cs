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
        public Task<ProjectServiceResponse> CreateProjectAsync(JsonElement projectJson)
        {
            Project? project = null;
            try { project = JsonSerializer.Deserialize<Project>(projectJson); } catch { }

            if (project is null)
                return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(false));

            if (project.OwnerId != Guid.Empty && !dbContext.Users.Any(u => u.Id == project.OwnerId))
                return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(false, null, "Assigned Owner does not exist"));

            dbContext.Projects.Add(project);
            dbContext.SaveChanges();

            return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(true));
        }

        public Task<ProjectServiceResponse> DeleteProjectAsync(Guid id)
        {
            var existingProject = dbContext.Projects.Find(id);

            if (existingProject is null)
                return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(false, null, "Project does not exist"));

            dbContext.Projects.Remove(existingProject);
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

        public Task<ProjectServiceResponse> UpdateProjectAsync(Guid id, JsonElement projectJson)
        {
            Project? project = null;
            try { project = JsonSerializer.Deserialize<Project>(projectJson); } catch { }

            if (project is null)
                return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(false));

            if (project.OwnerId != Guid.Empty && !dbContext.Users.Any(u => u.Id == project.OwnerId))
                return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(false, null, "Assigned Owner does not exist"));

            var existingProject = dbContext.Projects.FirstOrDefault(t => t.Id == id);

            if (existingProject is null)
                return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(false));

            project.Id = id;
            dbContext.Entry(existingProject).CurrentValues.SetValues(project);
            dbContext.SaveChanges();

            return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponse(true));
        }
    }
}
