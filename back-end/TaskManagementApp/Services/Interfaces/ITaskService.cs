using System.Text.Json;
using TaskManagementApp.Services.Responses;

namespace TaskManagementApp.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskServiceResponse> CreateTaskAsync(Guid userId, JsonElement taskJson);
        Task<TaskServiceResponse> GetTaskAsync(Guid id);
        Task<TaskServiceResponseList> GetTasksAsync(Guid? projectId);
        Task<TaskServiceResponse> UpdateTaskAsync(Guid userId, Guid id, JsonElement taskJson);
        Task<TaskServiceResponse> DeleteTaskAsync(Guid userId, Guid id);    
    }
}
