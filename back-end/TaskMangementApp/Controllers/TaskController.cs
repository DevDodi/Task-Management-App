using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskMangementApp.DB;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController(AppDBContext dbContext)
    {
        [HttpPost]
        public HttpResponseMessage CreateTask(string taskJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(taskJson))
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                Models.Task? task = JsonSerializer.Deserialize<Models.Task>(taskJson);

                if (task is null)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                task.Id = task.Id == Guid.Empty || dbContext.Tasks.Any(t => t.Id == task.Id) ? Guid.NewGuid() : task.Id;

                if (!dbContext.Users.Any(u => u.Id == task.AssignedUser))
                    throw new Exception("Assigned User does not exist");

                if (!dbContext.Projects.Any(p => p.Id == task.AssignedProject))
                    throw new Exception("Assigned Project does not exist");

                dbContext.Tasks.Add(task);
                dbContext.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex) { return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message) }; }
        }

        [HttpGet("{id}")]
        public HttpResponseMessage GetTask(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                var task = dbContext.Tasks.Find(id);

                if (task is null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                var taskJson = JsonSerializer.Serialize(task);

                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(taskJson) };
            }
            catch (Exception ex) { return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message) }; }
        }

        [HttpGet]
        public HttpResponseMessage GetAllTasks()
        {
            try
            {
                var tasks = dbContext.Tasks.ToList();
                string taskListJson = JsonSerializer.Serialize(tasks);

                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(taskListJson) };
            }
            catch (Exception ex) { return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message) }; }
        }

        [HttpPut("{id}")]
        public HttpResponseMessage UpdateTask(Guid id, string taskJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(taskJson))
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                Models.Task? task = JsonSerializer.Deserialize<Models.Task>(taskJson);

                if (task is null)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                var userExists = dbContext.Users.Any(u => u.Id == task.AssignedUser);
                if (!userExists)
                    throw new Exception("Assigned User does not exist");

                var projectExists = dbContext.Projects.Any(p => p.Id == task.AssignedProject);
                if (!projectExists)
                    throw new Exception("Assigned Project does not exist");

                var existingTask = dbContext.Tasks.FirstOrDefault(t => t.Id == id);

                if (existingTask is null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                existingTask = task;
                dbContext.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex) { return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message) }; }
        }

        [HttpDelete("{id}")]
        public HttpResponseMessage DeleteTask(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                var task = new Models.Task { Id = id };
                dbContext.Tasks.Remove(task);
                dbContext.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex) { return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message) }; }
        }
    }
}
