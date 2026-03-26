using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TaskMangementApp.DB;
using TaskMangementApp.Models;
using TaskMangementApp.Services;
using TaskMangementApp.Services.Interfaces;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController (IUserService userService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] JsonElement userJson)
        {
            var result = await userService.CreateUserAsync(userJson);

            if (!result.Success)
                return new BadRequestObjectResult(new { result.Message });

            return new OkResult();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            if (id == Guid.Empty)
                return new BadRequestResult();

            var result = await userService.GetUserAsync(id);

            if (!result.Success)
                return new BadRequestObjectResult(new { result.Message });

            return new OkObjectResult(new { result.User });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] JsonElement userJson)
        {
            var result = await userService.UpdateUserAsync(id, userJson);

            if (!result.Success)
                return new BadRequestObjectResult(new { result.Message });

            return new OkResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (id == Guid.Empty)
                return new BadRequestResult();

            var result = await userService.DeleteUserAsync(id);

            if (!result.Success)
                return new NotFoundObjectResult(new { result.Message });

            return new OkResult();
        }
    }
}
