using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskServiceResponse> CreateTaskAsync(string taskJson);
        Task<TaskServiceResponse> GetTaskAsync(Guid id);
        Task<TaskServiceResponseList> GetAllTasksAsync();
        Task<TaskServiceResponse> UpdateTaskAsync(Guid id, string taskJson);
        Task<TaskServiceResponse> DeleteTaskAsync(Guid id);     
    }
}
