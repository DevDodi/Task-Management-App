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
        public Task<TaskServiceResponse> CreateTaskAsync(JsonElement taskJson)
        {
            Models.Task? task = null;
            try { task = JsonSerializer.Deserialize<Models.Task>(taskJson); } catch { }

            if (task is null)
                return Task.FromResult(new TaskServiceResponse(false));

            if (task.AssignedUser != Guid.Empty && !dbContext.Users.Any(u => u.Id == task.AssignedUser))
                return Task.FromResult(new TaskServiceResponse(false, null, "Assigned User does not exist"));

            if (task.AssignedProject != Guid.Empty && !dbContext.Projects.Any(p => p.Id == task.AssignedProject))
                return Task.FromResult(new TaskServiceResponse(false, null, "Assigned Project does not exist"));

            dbContext.Tasks.Add(task);
            dbContext.SaveChanges();

            return Task.FromResult(new TaskServiceResponse(true));
        }

        public Task<TaskServiceResponse> DeleteTaskAsync(Guid id)
        {
            var existingTask = dbContext.Tasks.Find(id);

            if (existingTask is null)
                return System.Threading.Tasks.Task.FromResult(new TaskServiceResponse(false, null, "Task does not exist"));

            dbContext.Tasks.Remove(existingTask);
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

        public Task<TaskServiceResponse> UpdateTaskAsync(Guid id, JsonElement taskJson)
        {
            Models.Task? task = null;
            try { task = JsonSerializer.Deserialize<Models.Task>(taskJson); } catch { }

            if (task is null)
                return Task.FromResult(new TaskServiceResponse(false));

            if (task.AssignedUser != Guid.Empty && !dbContext.Users.Any(u => u.Id == task.AssignedUser))
                return Task.FromResult(new TaskServiceResponse(false, null, "Assigned User does not exist"));

            if (task.AssignedProject != Guid.Empty && !dbContext.Projects.Any(p => p.Id == task.AssignedProject))
                return Task.FromResult(new TaskServiceResponse(false, null, "Assigned Project does not exist"));

            var existingTask = dbContext.Tasks.FirstOrDefault(t => t.Id == id);

            if (existingTask is null)
                return Task.FromResult(new TaskServiceResponse(false));

            task.Id = id;
            dbContext.Entry(existingTask).CurrentValues.SetValues(task);
            dbContext.SaveChanges();

            return Task.FromResult(new TaskServiceResponse(true));
        }
    }
}
