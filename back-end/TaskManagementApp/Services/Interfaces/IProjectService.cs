using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TaskManagementApp.Services.Responses;
using static TaskManagementApp.Models.Project;

namespace TaskManagementApp.Services.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectServiceResponse> CreateProjectAsync(JsonElement projectJson);
        Task<ProjectServiceResponse> GetProjectAsync(Guid id);
        Task<ProjectServiceResponseList> GetProjectsAsync(bool unassigned, Guid? ownerId);
        Task<ProjectServiceResponse> UpdateProjectAsync(Guid id, JsonElement projectJson);
        Task<ProjectServiceResponse> DeleteProjectAsync(Guid id);
    }
}
