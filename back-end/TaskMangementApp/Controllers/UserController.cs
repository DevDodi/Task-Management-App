using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using TaskMangementApp.DB;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    public class UserController (AppDBContext dbContext)
    {
        [HttpPost(Name = "CreateUser")]
        public HttpResponseMessage CreateUser(string userJson)
        {

        }

        [HttpGet(Name = "GetUser")]
        public HttpResponseMessage GetUser(Guid id)
        {

        }

        [HttpPatch(Name = "UpdateUser")]
        public HttpResponseMessage UpdateUser()
        {

        }

        [HttpDelete(Name = "DeleteUser")]
        public HttpResponseMessage DeleteUser()
        {

        }
    }
}
