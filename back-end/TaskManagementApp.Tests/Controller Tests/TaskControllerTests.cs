using Xunit;
using Moq;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;
using TaskManagementApp.Controllers;
using TaskManagementApp.Services.Interfaces;
using TaskManagementApp.Services.Responses;
using Microsoft.AspNetCore.Http;

public class TaskControllerTests
{
    private readonly Mock<ITaskService> taskServiceMock = new();
    private TaskController CreateController(Guid userId)
    {
        var controller = new TaskController(taskServiceMock.Object);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }));
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        return controller;
    }

    [Fact]
    public async Task CreateTask_ReturnsOk_WhenSuccess()
    {
        var userId = Guid.NewGuid();
        taskServiceMock.Setup(s => s.CreateTaskAsync(userId, It.IsAny<JsonElement>())).ReturnsAsync(new TaskServiceResponse(true));
        var controller = CreateController(userId);
        var result = await controller.CreateTask(JsonDocument.Parse("{}".ToString()).RootElement);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CreateTask_ReturnsBadRequest_WhenFail()
    {
        var userId = Guid.NewGuid();
        taskServiceMock.Setup(s => s.CreateTaskAsync(userId, It.IsAny<JsonElement>())).ReturnsAsync(new TaskServiceResponse(false, null, "fail"));
        var controller = CreateController(userId);
        var result = await controller.CreateTask(JsonDocument.Parse("{}".ToString()).RootElement);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetTask_ReturnsOk_WhenSuccess()
    {
        var id = Guid.NewGuid();
        taskServiceMock.Setup(s => s.GetTaskAsync(id)).ReturnsAsync(new TaskServiceResponse(true, new TaskManagementApp.Models.Task { Title = "Test Task" }));
        var controller = CreateController(Guid.NewGuid());
        var result = await controller.GetTask(id);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetTask_ReturnsBadRequest_WhenFail()
    {
        var id = Guid.NewGuid();
        taskServiceMock.Setup(s => s.GetTaskAsync(id)).ReturnsAsync(new TaskServiceResponse(false, null, "fail"));
        var controller = CreateController(Guid.NewGuid());
        var result = await controller.GetTask(id);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetTasks_ReturnsOk()
    {
        taskServiceMock.Setup(s => s.GetTasksAsync(null)).ReturnsAsync(new TaskServiceResponseList(true, new List<TaskManagementApp.Models.Task>()));
        var controller = CreateController(Guid.NewGuid());
        var result = await controller.GetTasks();
        Assert.IsType<OkObjectResult>(result.Result);
    }
}
