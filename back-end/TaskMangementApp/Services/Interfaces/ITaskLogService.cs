using TaskMangementApp.Models;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services.Interfaces
{
    public interface ITaskLogService
    {
        Task<TaskLogServiceResponse> CreateTaskLogAsync(TaskLog taskLog);
        Task<TaskLogServiceResponseList> GetTaskLogsAsync(Guid taskId, DateTimeOffset? startDateRange, DateTimeOffset? endDateRange);
    }
}
