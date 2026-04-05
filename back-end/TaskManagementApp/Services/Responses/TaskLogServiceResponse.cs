using TaskManagementApp.Models;

namespace TaskManagementApp.Services.Responses
{
    public class TaskLogServiceResponse : BaseServiceResponse
    {
        public TaskLog? TaskLog { get; set; }
        public TaskLogServiceResponse(bool success, TaskLog? taskLog = null, string? message = null) : base(success, message)
        {
            TaskLog = taskLog;
        }
    }

    public class TaskLogServiceResponseList : BaseServiceResponse
    {
        public List<TaskLog?> TaskLogs { get; set; }
        public TaskLogServiceResponseList(bool success, List<TaskLog?> taskLogs, string? message = null) : base(success, message)
        {
            TaskLogs = taskLogs;
        }
    }
}
