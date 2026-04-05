using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using TaskManagementApp.Events.Interfaces;
using TaskManagementApp.Models.DTOs;
using TaskManagementApp.DB;
using TaskManagementApp.Models.Events;
using TaskManagementApp.Services.Interfaces;
using TaskManagementApp.Services.Responses;

namespace TaskManagementApp.Services
{
    public class TaskService(AppDBContext dbContext, IEventPublisher eventPublisher) : ITaskService
    {
        public Task<TaskServiceResponse> CreateTaskAsync(Guid userId, JsonElement taskJson)
        {
            TaskManagementApp.Models.Task? task = null;
            List<ITaskEvent> events = new List<ITaskEvent>();

            try { task = JsonSerializer.Deserialize<TaskManagementApp.Models.Task>(taskJson); } catch { }

            if (task is null)
                return Task.FromResult(new TaskServiceResponse(false));

            if (task.AssignedUser != Guid.Empty && !dbContext.Users.Any(u => u.Id == task.AssignedUser))
                return Task.FromResult(new TaskServiceResponse(false, null, "Assigned User does not exist"));

            if (task.AssignedProject != Guid.Empty && !dbContext.Projects.Any(p => p.Id == task.AssignedProject))
                return Task.FromResult(new TaskServiceResponse(false, null, "Assigned Project does not exist"));

            dbContext.Tasks.Add(task);
            events.Add(new TaskCreatedEvent
            {
                TaskId = task.Id,
                Title = task.Title,
                UpdatedById = userId,
                UpdatedByEmail = dbContext.Users.FirstOrDefault(u => u.Id == userId)?.Email ?? "Unknown",
                UpdatedAt = DateTime.UtcNow
            });

            dbContext.SaveChanges();
            eventPublisher.PublishAsync(events);

            return Task.FromResult(new TaskServiceResponse(true));
        }

        public Task<TaskServiceResponse> DeleteTaskAsync(Guid userId, Guid id)
        {
            var existingTask = dbContext.Tasks.Find(id);

            if (existingTask is null)
                return System.Threading.Tasks.Task.FromResult(new TaskServiceResponse(false, null, "Task does not exist"));

            dbContext.Tasks.Remove(existingTask);
            dbContext.SaveChanges();

            return Task.FromResult(new TaskServiceResponse(true));
        }

        public Task<TaskServiceResponseList> GetTasksAsync(Guid? projectId)
        {
            var assignedTasks = new List<TaskManagementApp.Models.Task>();

            if (projectId.HasValue && projectId != Guid.Empty)
                assignedTasks = dbContext.Tasks.Where(t => t.AssignedProject == projectId).ToList();
            else
                assignedTasks = dbContext.Tasks.ToList();

            return Task.FromResult(new TaskServiceResponseList(true, assignedTasks));
        }

        public Task<TaskServiceResponse> GetTaskAsync(Guid taskId)
        {
            var task = dbContext.Tasks.Find(taskId);

            if (task is null)
                return Task.FromResult(new TaskServiceResponse(false));

            return Task.FromResult(new TaskServiceResponse(true, task));
        }

        public Task<TaskServiceResponse> UpdateTaskAsync(Guid userId, Guid id, JsonElement taskJson)
        {
            TaskManagementApp.Models.Task? task = null;
            List<ITaskEvent> events = new List<ITaskEvent>();

            try { task = JsonSerializer.Deserialize<TaskManagementApp.Models.Task>(taskJson); } catch { }

            if (task is null)
                return Task.FromResult(new TaskServiceResponse(false));

            if (task.AssignedUser != Guid.Empty && !dbContext.Users.Any(u => u.Id == task.AssignedUser))
                return Task.FromResult(new TaskServiceResponse(false, null, "Assigned User does not exist"));

            if (task.AssignedProject != Guid.Empty && !dbContext.Projects.Any(p => p.Id == task.AssignedProject))
                return Task.FromResult(new TaskServiceResponse(false, null, "Assigned Project does not exist"));

            var existingTask = dbContext.Tasks.FirstOrDefault(t => t.Id == id);

            if (existingTask is null)
                return Task.FromResult(new TaskServiceResponse(false));

            var userEmail = dbContext.Users.FirstOrDefault(u => u.Id == userId)?.Email ?? "Unknown";

            if (existingTask.Status != task.Status)
            {
                events.Add(new TaskStatusChangedEvent
                {
                    TaskId = task.Id,
                    UpdatedById = userId,
                    UpdatedByEmail = userEmail,
                    TaskState = task.Status,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            if (existingTask.AssignedUser != task.AssignedUser)
            {
                events.Add(new TaskAssignedEvent
                {
                    TaskId = task.Id,
                    UpdatedById = userId,
                    UpdatedByEmail = userEmail,
                    AssignedUser = task.AssignedUser,
                    AssignedUserEmail = userEmail, // TODO: This needs to be a passed in field from the request
                    UpdatedAt = DateTime.UtcNow
                });
            }

            if (existingTask.Title != task.Title || existingTask.Description != task.Description)
            {
                events.Add(new TaskUpdatedEvent
                {
                    TaskId = task.Id,
                    UpdatedById = userId,
                    UpdatedByEmail = userEmail,
                    Title = task.Title,
                    Description = task.Description,
                    UpdatedAt = DateTime.UtcNow
                });
            }


            task.Id = id;
            dbContext.Entry(existingTask).CurrentValues.SetValues(task);

            dbContext.SaveChanges();
            eventPublisher.PublishAsync(events);

            return Task.FromResult(new TaskServiceResponse(true));
        }
    }
}
