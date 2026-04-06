using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagementApp.DB;
using TaskManagementApp.Events;
using TaskManagementApp.Models;
using TaskManagementApp.Models.Events;
using TaskManagementApp.Services;

namespace TaskManagementApp.Tests
{
    public class TaskLogServiceTests
    {
        private AppDBContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDBContext(options);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskLogsAsync_ReturnsLog_AfterTaskCreatedEvent()
        {
            var db = GetDbContext();
            var service = new TaskLogService(db);

            // Simulate event handler writing a TaskLog after a task is created
            var taskId = Guid.NewGuid();
            var log = new TaskLog
            {
                Id = Guid.NewGuid(),
                TaskId = taskId,
                Message = "Task created",
                LastUpdatedUtc = DateTime.UtcNow
            };
            db.TaskLogs.Add(log);
            db.SaveChanges();

            // Act
            var result = await service.GetTaskLogsAsync(taskId, null, null);

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.TaskLogs);
            Assert.Equal(log.Id, result.TaskLogs[0].Id);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskLogsAsync_ReturnsLog_AfterTaskUpdatedEvent()
        {
            var db = GetDbContext();
            var service = new TaskLogService(db);

            // Simulate event handler writing a TaskLog after a task is updated
            var taskId = Guid.NewGuid();
            var log = new TaskLog
            {
                Id = Guid.NewGuid(),
                TaskId = taskId,
                Message = "Task updated",
                LastUpdatedUtc = DateTime.UtcNow
            };
            db.TaskLogs.Add(log);
            db.SaveChanges();

            // Act
            var result = await service.GetTaskLogsAsync(taskId, null, null);

            // Assert
            Assert.True(result.Success);
            Assert.Single(result.TaskLogs);
            Assert.Equal("Task updated", result.TaskLogs[0].Message);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTaskLogsAsync_ReturnsEmpty_WhenNoLogs()
        {
            var db = GetDbContext();
            var service = new TaskLogService(db);

            var taskId = Guid.NewGuid();

            var result = await service.GetTaskLogsAsync(taskId, null, null);

            Assert.True(result.Success);
            Assert.Empty(result.TaskLogs);
        }

        [Fact]
        public async System.Threading.Tasks.Task Publish_TaskCreatedEvent_CreatesTaskLog()
        {
            var db = GetDbContext();
            var logService = new TaskLogService(db);
            var handler = new TaskLogEventHandler(logService);
            var publisher = new EventPublisher(new[] { handler });

            var taskId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var evt = new TaskCreatedEvent
            {
                TaskId = taskId,
                Title = "Test Task",
                UpdatedById = userId,
                UpdatedByEmail = "user@example.com",
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            await publisher.PublishAsync(new[] { evt });

            // Assert
            var log = db.TaskLogs.FirstOrDefault(l => l.TaskId == taskId);
            Assert.NotNull(log);
            Assert.Equal(LogAction.Created, log.Action);
            Assert.Contains("created task", log.Message);
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTaskAsync_PublishesEvent_AndTaskLogIsSaved()
        {
            var db = GetDbContext();
            var logService = new TaskLogService(db);
            var handler = new TaskLogEventHandler(logService);
            var publisher = new EventPublisher(new[] { handler });
            var taskService = new TaskService(db, publisher);

            // Add a user to satisfy TaskService's user lookup
            var user = new User { Email = "user@example.com", PasswordHash = "hash" };
            db.Users.Add(user);
            db.SaveChanges();

            var taskObj = new { Title = "Integration Test Task" };
            var json = JsonSerializer.Serialize(taskObj);

            // Act: create the task, which should publish the event
            var result = await taskService.CreateTaskAsync(user.Id, JsonDocument.Parse(json).RootElement);
            Assert.True(result.Success);

            // Assert: a TaskLog should exist for the created task
            var createdTask = db.Tasks.FirstOrDefault(t => t.Title == "Integration Test Task");
            Assert.NotNull(createdTask);
            var log = db.TaskLogs.FirstOrDefault(l => l.TaskId == createdTask.Id);
            Assert.NotNull(log);
            Assert.Equal(LogAction.Created, log.Action);
            Assert.Contains("created task", log.Message);
        }
    }
}
