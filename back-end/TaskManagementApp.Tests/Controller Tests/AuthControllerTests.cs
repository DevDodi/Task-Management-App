using Xunit;
using Moq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Controllers;
using TaskManagementApp.Services.Interfaces;
using TaskManagementApp.Services.Responses;

public class AuthControllerTests
{
    private readonly Mock<IJwtService> _jwtServiceMock = new();
    private AuthController CreateController() => new AuthController(_jwtServiceMock.Object);

    [Fact]
    public async Task Login_ReturnsOk_WhenSuccess()
    {
        _jwtServiceMock.Setup(s => s.Authenticate(It.IsAny<JsonElement>())).ReturnsAsync(new JwtServiceResponse(true, System.Guid.NewGuid(), "token", System.DateTime.UtcNow.AddMinutes(10)));
        var controller = CreateController();
        var result = await controller.Login(JsonDocument.Parse("{}".ToString()).RootElement);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenFail()
    {
        _jwtServiceMock.Setup(s => s.Authenticate(It.IsAny<JsonElement>())).ReturnsAsync(new JwtServiceResponse(false, Guid.Empty, "fail", System.DateTime.UtcNow));
        var controller = CreateController();
        var result = await controller.Login(JsonDocument.Parse("{}".ToString()).RootElement);
        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}
