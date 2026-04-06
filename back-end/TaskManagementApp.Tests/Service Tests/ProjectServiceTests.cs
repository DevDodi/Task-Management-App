using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using TaskManagementApp.DB;
using TaskManagementApp.Models;
using TaskManagementApp.Services;
using Xunit;

public class ProjectServiceTests
{
    private AppDBContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDBContext(options);
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateProjectAsync_ReturnsFalse_WhenDeserializationFails()
    {
        var db = GetDbContext();
        var service = new ProjectService(db);
        var result = await service.CreateProjectAsync(JsonDocument.Parse("null").RootElement);
        Assert.False(result.Success);
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateProjectAsync_ReturnsFalse_WhenOwnerDoesNotExist()
    {
        var db = GetDbContext();
        var service = new ProjectService(db);
        var project = new { Name = "Test Project", OwnerId = Guid.NewGuid() };
        var json = JsonSerializer.Serialize(project);
        var result = await service.CreateProjectAsync(JsonDocument.Parse(json).RootElement);
        Assert.False(result.Success);
        Assert.Equal("Assigned Owner does not exist", result.Message);
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateProjectAsync_ReturnsTrue_WhenOwnerExists()
    {
        var db = GetDbContext();
        var owner = new User { Email = "owner@example.com", PasswordHash = "hash" };
        db.Users.Add(owner);
        db.SaveChanges();

        var service = new ProjectService(db);
        var project = new { Name = "Test Project", OwnerId = owner.Id };
        var json = JsonSerializer.Serialize(project);
        var result = await service.CreateProjectAsync(JsonDocument.Parse(json).RootElement);
        Assert.True(result.Success);
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteProjectAsync_ReturnsFalse_WhenNotFound()
    {
        var db = GetDbContext();
        var service = new ProjectService(db);
        var result = await service.DeleteProjectAsync(Guid.NewGuid());
        Assert.False(result.Success);
        Assert.Equal("Project does not exist", result.Message);
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteProjectAsync_ReturnsTrue_WhenFound()
    {
        var db = GetDbContext();
        var project = new Project { Name = "Test Project" };
        db.Projects.Add(project);
        db.SaveChanges();

        var service = new ProjectService(db);
        var result = await service.DeleteProjectAsync(project.Id);
        Assert.True(result.Success);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetProjectAsync_ReturnsFalse_WhenNotFound()
    {
        var db = GetDbContext();
        var service = new ProjectService(db);
        var result = await service.GetProjectAsync(Guid.NewGuid());
        Assert.False(result.Success);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetProjectAsync_ReturnsTrue_WhenFound()
    {
        var db = GetDbContext();
        var project = new Project { Name = "Test Project" };
        db.Projects.Add(project);
        db.SaveChanges();

        var service = new ProjectService(db);
        var result = await service.GetProjectAsync(project.Id);
        Assert.True(result.Success);
        Assert.NotNull(result.Project);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetProjectsAsync_ReturnsFalse_WhenUnassignedAndOwnerId()
    {
        var db = GetDbContext();
        var service = new ProjectService(db);
        var result = await service.GetProjectsAsync(true, Guid.NewGuid());
        Assert.False(result.Success);
        Assert.Equal("Cannot combine unassigned and ownerId.", result.Message);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateProjectAsync_ReturnsFalse_WhenDeserializationFails()
    {
        var db = GetDbContext();
        var service = new ProjectService(db);
        var result = await service.UpdateProjectAsync(Guid.NewGuid(), JsonDocument.Parse("null").RootElement);
        Assert.False(result.Success);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateProjectAsync_ReturnsFalse_WhenOwnerDoesNotExist()
    {
        var db = GetDbContext();
        var project = new Project { Name = "Test Project" };
        db.Projects.Add(project);
        db.SaveChanges();

        var service = new ProjectService(db);
        var update = new { Name = "Updated", OwnerId = Guid.NewGuid() };
        var json = JsonSerializer.Serialize(update);
        var result = await service.UpdateProjectAsync(project.Id, JsonDocument.Parse(json).RootElement);
        Assert.False(result.Success);
        Assert.Equal("Assigned Owner does not exist", result.Message);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateProjectAsync_ReturnsFalse_WhenProjectNotFound()
    {
        var db = GetDbContext();
        var owner = new User { Email = "owner@example.com", PasswordHash = "hash" };
        db.Users.Add(owner);
        db.SaveChanges();

        var service = new ProjectService(db);
        var update = new { Name = "Updated", OwnerId = owner.Id };
        var json = JsonSerializer.Serialize(update);
        var result = await service.UpdateProjectAsync(Guid.NewGuid(), JsonDocument.Parse(json).RootElement);
        Assert.False(result.Success);
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateProjectAsync_ReturnsTrue_WhenValid()
    {
        var db = GetDbContext();
        var owner = new User { Email = "owner@example.com", PasswordHash = "hash" };
        db.Users.Add(owner);
        var project = new Project { Name = "Test Project" };
        db.Projects.Add(project);
        db.SaveChanges();

        var service = new ProjectService(db);
        var update = new { Name = "Updated", OwnerId = owner.Id };
        var json = JsonSerializer.Serialize(update);
        var result = await service.UpdateProjectAsync(project.Id, JsonDocument.Parse(json).RootElement);
        Assert.True(result.Success);
    }
}
