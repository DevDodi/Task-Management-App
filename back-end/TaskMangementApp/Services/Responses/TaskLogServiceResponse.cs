using TaskMangementApp.Models;

namespace TaskMangementApp.Services.Responses
{
    public class TaskLogServiceResponse : BaseServiceResponse
    {
        public List<TaskLog?> TaskLogs { get; set; }

        public TaskLogServiceResponse(bool success, List<TaskLog?> taskLogs, string? message = null) : base(success, message)
        {
            TaskLogs = taskLogs;
        }
    }
}
