using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using TaskManagementApp.Models;
using TaskManagementApp.DB;
using TaskManagementApp.Services.Interfaces;
using TaskManagementApp.Services.Responses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static TaskManagementApp.Models.Project;

namespace TaskManagementApp.Services
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

        public Task<ProjectServiceResponseList> GetProjectsAsync(bool unassigned, Guid? ownerId = null)
        {
            if (unassigned && ownerId.HasValue)
                return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponseList(false, null, "Cannot combine unassigned and ownerId."));

            var projectsQuery = dbContext.Projects.AsQueryable();

            if (unassigned)
                projectsQuery = projectsQuery.Where(p => p.OwnerId == Guid.Empty);
            else if (ownerId.HasValue)
                projectsQuery = projectsQuery.Where(p => p.OwnerId == ownerId.Value);

            var projects = projectsQuery.ToList();

            return System.Threading.Tasks.Task.FromResult(new ProjectServiceResponseList(true, projects));
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
