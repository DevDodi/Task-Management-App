using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TaskMangementApp.Models;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectServiceResponse> CreateProjectAsync(JsonElement projectJson);
        Task<ProjectServiceResponse> GetProjectAsync(Guid id);
        Task<ProjectServiceResponseList> GetProjectsAsync();
        Task<ProjectServiceResponse> UpdateProjectAsync(Guid id, JsonElement projectJson);
        Task<ProjectServiceResponse> DeleteProjectAsync(Guid id);
    }
}
