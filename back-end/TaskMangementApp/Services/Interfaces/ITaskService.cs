using System.Text.Json;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskServiceResponse> CreateTaskAsync(Guid userId, JsonElement taskJson);
        Task<TaskServiceResponse> GetTaskAsync(Guid id);
        Task<TaskServiceResponseList> GetAllTasksAsync();
        Task<TaskServiceResponse> UpdateTaskAsync(Guid userId, Guid id, JsonElement taskJson);
        Task<TaskServiceResponse> DeleteTaskAsync(Guid userId,Guid id);     
    }
}
