using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using TaskMangementApp.DB;
using TaskMangementApp.Services.Interfaces;
using TaskMangementApp.Services.Responses;

namespace TaskMangementApp.Services
{
    public class TaskService(AppDBContext dbContext) : ITaskService
    {
        public Task<TaskServiceResponse> CreateTaskAsync(string taskJson)
        {
            Models.Task? task = JsonSerializer.Deserialize<Models.Task>(taskJson);

            if (task is null)
                return Task.FromResult(new TaskServiceResponse(false));

            task.Id = task.Id == Guid.Empty || dbContext.Tasks.Any(t => t.Id == task.Id) ? Guid.NewGuid() : task.Id;

            if (!dbContext.Users.Any(u => u.Id == task.AssignedUser))
                return Task.FromResult(new TaskServiceResponse(false, null, "Assigned User does not exist"));

            if (!dbContext.Projects.Any(p => p.Id == task.AssignedProject))
                return Task.FromResult(new TaskServiceResponse(false, null, "Assigned Project does not exist"));

            dbContext.Tasks.Add(task);
            dbContext.SaveChanges();

            return Task.FromResult(new TaskServiceResponse(true));
        }

        public Task<TaskServiceResponse> DeleteTaskAsync(Guid id)
        {
            var task = new Models.Task { Id = id };
            dbContext.Tasks.Remove(task);
            dbContext.SaveChanges();

            return Task.FromResult(new TaskServiceResponse(true));
        }

        public Task<TaskServiceResponseList> GetAllTasksAsync()
        {
            var tasks = dbContext.Tasks.ToList();
            return Task.FromResult(new TaskServiceResponseList(true, tasks));
        }

        public Task<TaskServiceResponse> GetTaskAsync(Guid id)
        {
            var task = dbContext.Tasks.Find(id);

            if (task is null)
                return Task.FromResult(new TaskServiceResponse(false));

            return Task.FromResult(new TaskServiceResponse(true, task));
        }

        public Task<TaskServiceResponse> UpdateTaskAsync(Guid id, string taskJson)
        {
            Models.Task? task = JsonSerializer.Deserialize<Models.Task>(taskJson);

            if (task is null)
                return Task.FromResult(new TaskServiceResponse(false));

            var userExists = dbContext.Users.Any(u => u.Id == task.AssignedUser);
            if (!userExists)
                return Task.FromResult(new TaskServiceResponse(false, null, "Assigned User does not exist"));

            var projectExists = dbContext.Projects.Any(p => p.Id == task.AssignedProject);
            if (!projectExists)
                return Task.FromResult(new TaskServiceResponse(false, null, "Assigned Project does not exist"));

            var existingTask = dbContext.Tasks.FirstOrDefault(t => t.Id == id);

            if (existingTask is null)
                return Task.FromResult(new TaskServiceResponse(false));

            existingTask = task;
            dbContext.SaveChanges();

            return Task.FromResult(new TaskServiceResponse(true));
        }
    }
}
