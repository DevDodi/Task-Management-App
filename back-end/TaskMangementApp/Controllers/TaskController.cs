using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using TaskMangementApp.DB;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    public class TaskController(AppDBContext dbContext)
    {
        [HttpPost(Name = "CreateTask")]
        public HttpResponseMessage CreateTask(string taskJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(taskJson))
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                Models.Task task = JsonConvert.DeserializeObject<Models.Task>(taskJson);

                if (task is null)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                var userExists = dbContext.Users.Any(u => u.Id == task.AssignedUser);
                if (!userExists)
                    throw new Exception("Assigned User does not exist");

                var projectExists = dbContext.Projects.Any(p => p.Id == task.AssignedProject);
                if (!projectExists)
                    throw new Exception("Assigned Project does not exist");

                dbContext.Tasks.Add(task);
                dbContext.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex) { return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message)}; }
        }

        [HttpGet(Name = "GetTask")]
        public HttpResponseMessage GetTask(Guid id)
        {

        }

        [HttpGet(Name = "GetAllTask")]
        public HttpResponseMessage GetAllTask()
        {

        }

        [HttpPatch(Name = "UpdateTask")]
        public HttpResponseMessage UpdateTask()
        {

        }

        [HttpDelete(Name = "DeleteTask")]
        public HttpResponseMessage DeleteTask()
        {

        }
    }
}
