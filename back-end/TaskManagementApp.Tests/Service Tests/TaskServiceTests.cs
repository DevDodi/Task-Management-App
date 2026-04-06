using Xunit;
using Moq;
using System;
using System.Text.Json;
using System.Collections.Generic;
using TaskManagementApp.Services;
using TaskManagementApp.Services.Responses;
using TaskManagementApp.Models;
using TaskManagementApp.DB;
using TaskManagementApp.Events.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TaskServiceTests
{
    private AppDBContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDBContext(options);
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateTaskAsync_ReturnsFalse_WhenDeserializationFails()
    {
        var db = GetDbContext();
        var publisher = new Mock<IEventPublisher>();
        var service = new TaskService(db, publisher.Object);
        var result = await service.CreateTaskAsync(Guid.NewGuid(), JsonDocument.Parse("null").RootElement);
        Assert.False(result.Success);
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateTaskAsync_ReturnsFalse_WhenAssignedUserDoesNotExist()
    {
        var db = GetDbContext();
        var publisher = new Mock<IEventPublisher>();
        var service = new TaskService(db, publisher.Object);
        var task = new { Title = "Test", AssignedUser = Guid.NewGuid() };
        var json = JsonSerializer.Serialize(task);
        var result = await service.CreateTaskAsync(Guid.NewGuid(), JsonDocument.Parse(json).RootElement);
        Assert.False(result.Success);
        Assert.Equal("Assigned User does not exist", result.Message);
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateTaskAsync_ReturnsFalse_WhenAssignedProjectDoesNotExist()
    {
        var db = GetDbContext();
        var publisher = new Mock<IEventPublisher>();
        var user = new User { Email = "user@example.com", PasswordHash = "hash" };
        db.Users.Add(user);
        db.SaveChanges();

        var service = new TaskService(db, publisher.Object);
        var task = new { Title = "Test", AssignedUser = user.Id, AssignedProject = Guid.NewGuid() };
        var json = JsonSerializer.Serialize(task);
        var result = await service.CreateTaskAsync(user.Id, JsonDocument.Parse(json).RootElement);
        Assert.False(result.Success);
        Assert.Equal("Assigned Project does not exist", result.Message);
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateTaskAsync_ReturnsTrue_WhenValid()
    {
        var db = GetDbContext();
        var publisher = new Mock<IEventPublisher>();
        var user = new User { Email = "user@example.com", PasswordHash = "hash" };
        var project = new Project { Name = "Project" };
        db.Users.Add(user);
        db.Projects.Add(project);
        db.SaveChanges();

        var service = new TaskService(db, publisher.Object);
        var task = new { Title = "Test", AssignedUser = user.Id, AssignedProject = project.Id };
        var json = JsonSerializer.Serialize(task);
        var result = await service.CreateTaskAsync(user.Id, JsonDocument.Parse(json).RootElement);
        Assert.True(result.Success);
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteTaskAsync_ReturnsFalse_WhenTaskNotFound()
    {
        var db = GetDbContext();
        var publisher = new Mock<IEventPublisher>();
        var service = new TaskService(db, publisher.Object);
        var result = await service.DeleteTaskAsync(Guid.NewGuid(), Guid.NewGuid());
        Assert.False(result.Success);
        Assert.Equal("Task does not exist", result.Message);
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteTaskAsync_ReturnsTrue_WhenTaskFound()
    {
        var db = GetDbContext();
        var publisher = new Mock<IEventPublisher>();
        var task = new TaskManagementApp.Models.Task { Title = "Test" };
        db.Tasks.Add(task);
        db.SaveChanges();

        var service = new TaskService(db, publisher.Object);
        var result = await service.DeleteTaskAsync(Guid.NewGuid(), task.Id);
        Assert.True(result.Success);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTaskAsync_ReturnsFalse_WhenDeserializationFails()
    {
        var db = GetDbContext();
        var publisher = new Mock<IEventPublisher>();
        var service = new TaskService(db, publisher.Object);
        var result = await service.UpdateTaskAsync(Guid.NewGuid(), Guid.NewGuid(), JsonDocument.Parse("null").RootElement);
        Assert.False(result.Success);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTaskAsync_ReturnsFalse_WhenAssignedUserDoesNotExist()
    {
        var db = GetDbContext();
        var publisher = new Mock<IEventPublisher>();
        var task = new TaskManagementApp.Models.Task { Title = "Test" };
        db.Tasks.Add(task);
        db.SaveChanges();

        var service = new TaskService(db, publisher.Object);
        var update = new { Title = "Test", AssignedUser = Guid.NewGuid() };
        var json = JsonSerializer.Serialize(update);
        var result = await service.UpdateTaskAsync(Guid.NewGuid(), task.Id, JsonDocument.Parse(json).RootElement);
        Assert.False(result.Success);
        Assert.Equal("Assigned User does not exist", result.Message);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTaskAsync_ReturnsFalse_WhenAssignedProjectDoesNotExist()
    {
        var db = GetDbContext();
        var publisher = new Mock<IEventPublisher>();
        var user = new User { Email = "user@example.com", PasswordHash = "hash" };
        db.Users.Add(user);
        var task = new TaskManagementApp.Models.Task { Title = "Test", AssignedUser = user.Id };
        db.Tasks.Add(task);
        db.SaveChanges();

        var service = new TaskService(db, publisher.Object);
        var update = new { Title = "Test", AssignedUser = user.Id, AssignedProject = Guid.NewGuid() };
        var json = JsonSerializer.Serialize(update);
        var result = await service.UpdateTaskAsync(user.Id, task.Id, JsonDocument.Parse(json).RootElement);
        Assert.False(result.Success);
        Assert.Equal("Assigned Project does not exist", result.Message);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTaskAsync_ReturnsFalse_WhenTaskNotFound()
    {
        var db = GetDbContext();
        var publisher = new Mock<IEventPublisher>();
        var user = new User { Email = "user@example.com", PasswordHash = "hash" };
        db.Users.Add(user);
        db.SaveChanges();

        var service = new TaskService(db, publisher.Object);
        var update = new { Title = "Test", AssignedUser = user.Id };
        var json = JsonSerializer.Serialize(update);
        var result = await service.UpdateTaskAsync(user.Id, Guid.NewGuid(), JsonDocument.Parse(json).RootElement);
        Assert.False(result.Success);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTaskAsync_ReturnsTrue_WhenValid()
    {
        var db = GetDbContext();
        var publisher = new Mock<IEventPublisher>();
        var user = new User { Email = "user@example.com", PasswordHash = "hash" };
        db.Users.Add(user);
        var task = new TaskManagementApp.Models.Task { Title = "Test", AssignedUser = user.Id };
        db.Tasks.Add(task);
        db.SaveChanges();

        var service = new TaskService(db, publisher.Object);
        var update = new { Title = "Updated", AssignedUser = user.Id };
        var json = JsonSerializer.Serialize(update);
        var result = await service.UpdateTaskAsync(user.Id, task.Id, JsonDocument.Parse(json).RootElement);
        Assert.True(result.Success);
    }
}
