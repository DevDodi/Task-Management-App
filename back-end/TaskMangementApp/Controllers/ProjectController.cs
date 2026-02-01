using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TaskMangementApp.DB;
using TaskMangementApp.Models;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController(AppDBContext dbContext)
    {
        [HttpPost]
        public HttpResponseMessage CreateProject(string projectJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(projectJson))
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                Project? project = JsonSerializer.Deserialize<Project>(projectJson);

                if (project is null)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                var ownerExists = dbContext.Users.Any(u => u.Id == project.OwnerId);
                if (!ownerExists)
                    throw new Exception("Assigned Owner does not exist");

                dbContext.Projects.Add(project);
                dbContext.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex) { return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message) }; }
        }

        [HttpGet("{id}")]
        public HttpResponseMessage GetProject(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                var project = dbContext.Projects.Find(id);

                if (project is null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                var projectJson = JsonSerializer.Serialize(project);

                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(projectJson) };
            }
            catch (Exception ex) { return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message) }; }
        }

        [HttpPatch("{id}")]
        public HttpResponseMessage UpdateProject(Guid id, string projectJson)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(projectJson))
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                Project? project = JsonSerializer.Deserialize<Project>(projectJson);

                if (project is null)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                var ownerExists = dbContext.Users.Any(u => u.Id == project.OwnerId);
                if (!ownerExists)
                    throw new Exception("Assigned Owner does not exist");

                var existingProject = dbContext.Projects.FirstOrDefault(t => t.Id == id);

                if (existingProject is null)
                    return new HttpResponseMessage(HttpStatusCode.NotFound);

                existingProject = project;
                dbContext.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex) { return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message) }; }

        }

        [HttpDelete("{id}")]
        public HttpResponseMessage DeleteProject(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                var project = new Project { Id = id };
                dbContext.Projects.Remove(project);
                dbContext.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex) { return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message) }; }
        }
    }
}
