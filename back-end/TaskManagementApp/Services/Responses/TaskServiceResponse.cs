using System.Collections.Generic;

namespace TaskManagementApp.Services.Responses
{
    public class TaskServiceResponse : BaseServiceResponse
    {
        public TaskManagementApp.Models.Task? Task { get; set; }

        public TaskServiceResponse(bool success, TaskManagementApp.Models.Task? task = null, string? message = null) : base(success, message)
        {
            Task = task;
        }
    }

    public class TaskServiceResponseList : BaseServiceResponse
    {
        public List<TaskManagementApp.Models.Task?> Tasks { get; set; }

        public TaskServiceResponseList(bool success, List<TaskManagementApp.Models.Task?> tasks, string? message = null) : base(success, message)
        {
            Tasks = tasks;
        }
    }
}
