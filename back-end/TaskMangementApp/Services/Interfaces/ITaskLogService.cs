using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services.Interfaces
{
    public interface ITaskLogService
    {
        Task<TaskLogServiceResponse> GetTaskLogsAsync(Guid taskId, DateTimeOffset? startDateRange, DateTimeOffset? endDateRange);
    }
}
