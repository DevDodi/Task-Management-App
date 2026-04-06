using Xunit;
using Moq;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Controllers;
using TaskManagementApp.Services.Interfaces;
using TaskManagementApp.Services.Responses;
using System.Collections.Generic;

public class UserControllerTests
{
    private readonly Mock<IUserService> userServiceMock = new();
    private UserController CreateController() => new UserController(userServiceMock.Object);

    [Fact]
    public async Task CreateUser_ReturnsOk_WhenSuccess()
    {
        userServiceMock.Setup(s => s.CreateUserAsync(It.IsAny<JsonElement>())).ReturnsAsync(new UserServiceResponse(true));
        var controller = CreateController();
        var result = await controller.CreateUser(JsonDocument.Parse("{}".ToString()).RootElement);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CreateUser_ReturnsBadRequest_WhenFail()
    {
        userServiceMock.Setup(s => s.CreateUserAsync(It.IsAny<JsonElement>())).ReturnsAsync(new UserServiceResponse(false, null, "fail"));
        var controller = CreateController();
        var result = await controller.CreateUser(JsonDocument.Parse("{}".ToString()).RootElement);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetUsers_ReturnsOk_WhenSuccess()
    {
        userServiceMock.Setup(s => s.GetUsersAsync()).ReturnsAsync(new UserServiceResponseList(true, new List<TaskManagementApp.Models.DTOs.UserDTO>()));
        var controller = CreateController();
        var result = await controller.GetUsers();
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUsers_ReturnsBadRequest_WhenFail()
    {
        userServiceMock.Setup(s => s.GetUsersAsync()).ReturnsAsync(new UserServiceResponseList(false, null, "fail"));
        var controller = CreateController();
        var result = await controller.GetUsers();
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUser_ReturnsOk_WhenSuccess()
    {
        var id = Guid.NewGuid();
        var userDto = new TaskManagementApp.Models.DTOs.UserDTO { Id = id, Email = "test@example.com" };
        userServiceMock.Setup(s => s.GetUserAsync(id)).ReturnsAsync(new UserServiceResponse(true, userDto));
        var controller = CreateController();
        var result = await controller.GetUser(id);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetUser_ReturnsBadRequest_WhenFail()
    {
        var id = Guid.NewGuid();
        userServiceMock.Setup(s => s.GetUserAsync(id)).ReturnsAsync(new UserServiceResponse(false, null, "fail"));
        var controller = CreateController();
        var result = await controller.GetUser(id);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
