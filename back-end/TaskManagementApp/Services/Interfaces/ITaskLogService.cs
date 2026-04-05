using TaskManagementApp.Models;
using TaskManagementApp.Services.Responses;

namespace TaskManagementApp.Services.Interfaces
{
    public interface ITaskLogService
    {
        Task<TaskLogServiceResponse> CreateTaskLogAsync(TaskLog taskLog);
        Task<TaskLogServiceResponseList> GetTaskLogsAsync(Guid taskId, DateTimeOffset? startDateRange, DateTimeOffset? endDateRange);
    }
}
