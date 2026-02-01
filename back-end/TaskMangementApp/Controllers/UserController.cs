using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TaskMangementApp.DB;
using TaskMangementApp.Models;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController (AppDBContext dbContext)
    {
        [HttpPost]
        public HttpResponseMessage CreateUser(string userJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userJson))
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                User? user = JsonSerializer.Deserialize<User>(userJson);

                if (user is null)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                if (dbContext.Users.Any(u => u.Email == user.Email))
                    throw new Exception("This email is currently being used by another User.");

                user.Id = user.Id == Guid.Empty ? Guid.NewGuid() : user.Id;

                dbContext.Users.Add(user);
                dbContext.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex) { return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message) }; }
        }

        [HttpGet("{id}")]
        public HttpResponseMessage GetUser(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                var user = dbContext.Users.Find(id);

                if (user is null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                var userJson = JsonSerializer.Serialize(user);

                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(userJson) };
            }
            catch (Exception ex) { return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message) }; }
        }

        [HttpPatch("{id}")]
        public HttpResponseMessage UpdateUser(Guid id, string userJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userJson))
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                User? user = JsonSerializer.Deserialize<User>(userJson);

                if (user is null)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                if (dbContext.Users.Any(u => u.Email == user.Email && u.Id != user.Id))
                    throw new Exception("This email is currently being used by another User.");

                var existingUser = dbContext.Users.FirstOrDefault(u => u.Id == id);

                if (existingUser is null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                existingUser = user;
                dbContext.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex) { return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message) }; }
        }

        [HttpDelete("{id}")]
        public HttpResponseMessage DeleteUser(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                var user = new User { Id = id };
                dbContext.Users.Remove(user);
                dbContext.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex) { return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message) }; }
        }
    }
}
