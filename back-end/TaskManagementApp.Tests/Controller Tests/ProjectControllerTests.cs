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

public class ProjectControllerTests
{
    private readonly Mock<IProjectService> projectServiceMock = new();
    private ProjectController CreateController() => new ProjectController(projectServiceMock.Object);

    [Fact]
    public async Task CreateProject_ReturnsOk_WhenSuccess()
    {
        projectServiceMock.Setup(s => s.CreateProjectAsync(It.IsAny<JsonElement>())).ReturnsAsync(new ProjectServiceResponse(true));
        var controller = CreateController();
        var result = await controller.CreateProject(JsonDocument.Parse("{}".ToString()).RootElement);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task CreateProject_ReturnsBadRequest_WhenFail()
    {
        projectServiceMock.Setup(s => s.CreateProjectAsync(It.IsAny<JsonElement>())).ReturnsAsync(new ProjectServiceResponse(false, null, "fail"));
        var controller = CreateController();
        var result = await controller.CreateProject(JsonDocument.Parse("{}".ToString()).RootElement);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetProject_ReturnsOk_WhenSuccess()
    {
        var id = Guid.NewGuid();
        projectServiceMock.Setup(s => s.GetProjectAsync(id)).ReturnsAsync(new ProjectServiceResponse(true, new TaskManagementApp.Models.Project { Name = "Test Project" }));
        var controller = CreateController();
        var result = await controller.GetProject(id);
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetProject_ReturnsBadRequest_WhenFail()
    {
        var id = Guid.NewGuid();
        projectServiceMock.Setup(s => s.GetProjectAsync(id)).ReturnsAsync(new ProjectServiceResponse(false, null, "fail"));
        var controller = CreateController();
        var result = await controller.GetProject(id);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetProjectsAsync_ReturnsOk_WhenSuccess()
    {
        projectServiceMock.Setup(s => s.GetProjectsAsync(false, null)).ReturnsAsync(new ProjectServiceResponseList(true, new List<TaskManagementApp.Models.Project>()));
        var controller = CreateController();
        var result = await controller.GetProjectsAsync();
        Assert.IsType<OkObjectResult>(result.Result);
    }
}
