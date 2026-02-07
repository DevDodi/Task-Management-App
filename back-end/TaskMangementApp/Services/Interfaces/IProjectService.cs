using Microsoft.AspNetCore.Mvc;
using TaskMangementApp.Models;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectServiceResponse> CreateProjectAsync(string projectJson);
        Task<ProjectServiceResponse> GetProjectAsync(Guid id);
        Task<ProjectServiceResponse> UpdateProjectAsync(Guid id, string projectJson);
        Task<ProjectServiceResponse> DeleteProjectAsync(Guid id);
    }
}
