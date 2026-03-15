using System.Text.Json;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskServiceResponse> CreateTaskAsync(JsonElement taskJson);
        Task<TaskServiceResponse> GetTaskAsync(Guid id);
        Task<TaskServiceResponseList> GetTasksAsync(Guid? projectId);
        Task<TaskServiceResponse> UpdateTaskAsync(Guid id, JsonElement taskJson);
        Task<TaskServiceResponse> DeleteTaskAsync(Guid id);     
    }
}
