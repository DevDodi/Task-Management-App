using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TaskMangementApp.Services;
using TaskMangementApp.Services.Interfaces;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController (IJwtSevice jwtService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] JsonElement userJson)
        {
            var result = await jwtService.Authenticate(userJson);

            if (!result.Success)
                return new UnauthorizedObjectResult(result.Message);

            return new OkObjectResult(result.AccessToken);
        }
    }
}
