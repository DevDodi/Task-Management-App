using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using TaskManagementApp.DB;
using TaskManagementApp.Models;
using TaskManagementApp.Services;
using Xunit;

public class UserServiceTests
{
    private AppDBContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDBContext(options);
    }

    [Fact]
    public async void CreateUserAsync_ReturnsFalse_WhenDeserializationFails()
    {
        var db = GetDbContext();
        var service = new UserService(db);
        var result = await service.CreateUserAsync(JsonDocument.Parse("null").RootElement);
        Assert.False(result.Success);
    }

    [Fact]
    public async void CreateUserAsync_ReturnsFalse_WhenEmailExists()
    {
        var db = GetDbContext();
        db.Users.Add(new User { Email = "test@example.com", PasswordHash = "hash" });
        db.SaveChanges();

        var service = new UserService(db);
        var user = new { Email = "test@example.com", Password = "password" };
        var json = JsonSerializer.Serialize(user);
        var result = await service.CreateUserAsync(JsonDocument.Parse(json).RootElement);
        Assert.False(result.Success);
        Assert.Equal("This email is currently being used by another User.", result.Message);
    }

    [Fact]
    public async void CreateUserAsync_ReturnsTrue_WhenValid()
    {
        var db = GetDbContext();
        var service = new UserService(db);
        var user = new { Email = "unique@example.com", Password = "password" };
        var json = JsonSerializer.Serialize(user);
        var result = await service.CreateUserAsync(JsonDocument.Parse(json).RootElement);
        Assert.True(result.Success);
    }

    [Fact]
    public async void DeleteUserAsync_ReturnsFalse_WhenNotFound()
    {
        var db = GetDbContext();
        var service = new UserService(db);
        var result = await service.DeleteUserAsync(Guid.NewGuid());
        Assert.False(result.Success);
        Assert.Equal("User does not exist", result.Message);
    }

    [Fact]
    public async void DeleteUserAsync_ReturnsTrue_WhenFound()
    {
        var db = GetDbContext();
        var user = new User { Email = "delete@example.com", PasswordHash = "hash" };
        db.Users.Add(user);
        db.SaveChanges();

        var service = new UserService(db);
        var result = await service.DeleteUserAsync(user.Id);
        Assert.True(result.Success);
    }

    [Fact]
    public async void GetUsersAsync_ReturnsAllUsers()
    {
        var db = GetDbContext();
        db.Users.Add(new User { Email = "a@example.com", PasswordHash = "hash" });
        db.Users.Add(new User { Email = "b@example.com", PasswordHash = "hash" });
        db.SaveChanges();

        var service = new UserService(db);
        var result = await service.GetUsersAsync();
        Assert.True(result.Success);
        Assert.Equal(2, result.Users.Count);
    }

    [Fact]
    public async void GetUserAsync_ReturnsFalse_WhenNotFound()
    {
        var db = GetDbContext();
        var service = new UserService(db);
        var result = await service.GetUserAsync(Guid.NewGuid());
        Assert.False(result.Success);
    }

    [Fact]
    public async void GetUserAsync_ReturnsTrue_WhenFound()
    {
        var db = GetDbContext();
        var user = new User { Email = "findme@example.com", PasswordHash = "hash" };
        db.Users.Add(user);
        db.SaveChanges();

        var service = new UserService(db);
        var result = await service.GetUserAsync(user.Id);
        Assert.True(result.Success);
        Assert.NotNull(result.User);
        Assert.Equal(user.Email, result.User.Email);
    }

    [Fact]
    public async void UpdateUserAsync_ReturnsFalse_WhenDeserializationFails()
    {
        var db = GetDbContext();
        var service = new UserService(db);
        var result = await service.UpdateUserAsync(Guid.NewGuid(), JsonDocument.Parse("null").RootElement);
        Assert.False(result.Success);
    }

    [Fact]
    public async void UpdateUserAsync_ReturnsFalse_WhenEmailExistsForAnotherUser()
    {
        var db = GetDbContext();
        var user1 = new User { Email = "user1@example.com", PasswordHash = "hash" };
        var user2 = new User { Email = "user2@example.com", PasswordHash = "hash" };
        db.Users.Add(user1);
        db.Users.Add(user2);
        db.SaveChanges();

        var service = new UserService(db);
        var update = new { Id = user2.Id, Email = "user1@example.com", Password = "newpass" };
        var json = JsonSerializer.Serialize(update);
        var result = await service.UpdateUserAsync(user2.Id, JsonDocument.Parse(json).RootElement);
        Assert.False(result.Success);
        Assert.Equal("This email is currently being used by another User.", result.Message);
    }

    [Fact]
    public async void UpdateUserAsync_ReturnsFalse_WhenUserNotFound()
    {
        var db = GetDbContext();
        var service = new UserService(db);
        var update = new { Id = Guid.NewGuid(), Email = "notfound@example.com", Password = "pass" };
        var json = JsonSerializer.Serialize(update);
        var result = await service.UpdateUserAsync(Guid.NewGuid(), JsonDocument.Parse(json).RootElement);
        Assert.False(result.Success);
    }

    [Fact]
    public async void UpdateUserAsync_ReturnsTrue_WhenValid()
    {
        var db = GetDbContext();
        var user = new User { Email = "update@example.com", PasswordHash = "hash" };
        db.Users.Add(user);
        db.SaveChanges();

        var service = new UserService(db);
        var update = new { Id = user.Id, Email = "updated@example.com", Password = "newpass" };
        var json = JsonSerializer.Serialize(update);
        var result = await service.UpdateUserAsync(user.Id, JsonDocument.Parse(json).RootElement);
        Assert.True(result.Success);
    }
}
