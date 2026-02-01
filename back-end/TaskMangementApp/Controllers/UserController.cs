using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskMangementApp.DB;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController (AppDBContext dbContext)
    {
        [HttpPost]
        public HttpResponseMessage CreateUser(string userJson)
        {
            return null;

        }

        [HttpGet("{id}")]
        public HttpResponseMessage GetUser(Guid id)
        {
            return null;

        }

        [HttpPatch("{id}")]
        public HttpResponseMessage UpdateUser(Guid id, string userJson)
        {
            return null;

        }

        [HttpDelete("{id}")]
        public HttpResponseMessage DeleteUser(Guid id)
        {
            return null;

        }
    }
}
