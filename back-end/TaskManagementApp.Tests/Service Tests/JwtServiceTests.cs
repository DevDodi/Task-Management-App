using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using TaskManagementApp.DB;
using TaskManagementApp.Models;
using TaskManagementApp.Services;
using Xunit;

public class JwtServiceTests
{
    private AppDBContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDBContext(options);
    }

    private IConfiguration GetConfig()
    {
        var dict = new System.Collections.Generic.Dictionary<string, string?>
        {
            {"JwtSettings:Issuer", "issuer"},
            {"JwtSettings:Audience", "audience"},
            {"JwtSettings:Key", "supersecretkey12345678901234567890"},
            {"JwtSettings:ExpiryMinutes", "60"}
        };
        return new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
    }

    [Fact]
    public async void Authenticate_ReturnsFalse_WhenDeserializationFails()
    {
        var db = GetDbContext();
        var config = GetConfig();
        var service = new JwtService(db, config);
        var result = await service.Authenticate(JsonDocument.Parse("null").RootElement);
        Assert.False(result.Success);
    }

    [Fact]
    public async void Authenticate_ReturnsTrue_WithValidCredentials()
    {
        var db = GetDbContext();
        var config = GetConfig();
        var email = "user@example.com";
        var password = "Password123!";
        var user = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };
        db.Users.Add(user);
        db.SaveChanges();

        var loginDto = new { Email = email, Password = password };
        var json = JsonSerializer.Serialize(loginDto);
        var service = new JwtService(db, config);

        var result = await service.Authenticate(JsonDocument.Parse(json).RootElement);

        Assert.True(result.Success);
        Assert.NotNull(result.AccessToken);
        Assert.Equal(user.Id, result.UserId);
    }

    [Fact]
    public async void Authenticate_ReturnsFalse_WithInvalidPassword()
    {
        var db = GetDbContext();
        var config = GetConfig();
        var email = "user@example.com";
        var password = "Password123!";
        var user = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };
        db.Users.Add(user);
        db.SaveChanges();

        var loginDto = new { Email = email, Password = "WrongPassword" };
        var json = JsonSerializer.Serialize(loginDto);
        var service = new JwtService(db, config);

        var result = await service.Authenticate(JsonDocument.Parse(json).RootElement);

        Assert.False(result.Success);
        Assert.Null(result.AccessToken);
    }

    [Fact]
    public async void Authenticate_ReturnsFalse_WithNonexistentUser()
    {
        var db = GetDbContext();
        var config = GetConfig();
        var loginDto = new { Email = "nouser@example.com", Password = "Password123!" };
        var json = JsonSerializer.Serialize(loginDto);
        var service = new JwtService(db, config);

        var result = await service.Authenticate(JsonDocument.Parse(json).RootElement);

        Assert.False(result.Success);
        Assert.Null(result.AccessToken);
    }
}
