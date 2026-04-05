using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TaskManagementApp.Services;
using TaskManagementApp.Services.Interfaces;

namespace TaskManagementApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController (IJwtService jwtService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] JsonElement userJson)
        {
            var result = await jwtService.Authenticate(userJson);

            if (!result.Success)
                return new UnauthorizedObjectResult(result.Message);

            return new OkObjectResult(new { result.UserId, result.AccessToken });
        }
    }
}
