using System.Collections.Generic;
using TaskMangementApp.Models;

namespace TaskMangementApp.Services.Responses
{
    public class TaskServiceResponse : BaseServiceResponse
    {
        public Models.Task? Task { get; set; }

        public TaskServiceResponse(bool success, Models.Task? task = null, string? message = null) : base(success, message)
        {
            Task = task;
        }
    }

    public class TaskServiceResponseList : BaseServiceResponse
    {
        public List<Models.Task?> Tasks { get; set; }

        public TaskServiceResponseList(bool success, List<Models.Task?> tasks, string? message = null) : base(success, message)
        {
            Tasks = tasks;
        }
    }
}
