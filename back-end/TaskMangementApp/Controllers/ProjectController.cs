using Microsoft.AspNetCore.Mvc;
using TaskMangementApp.DB;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    public class ProjectController(AppDBContext dbContext)
    {
        [HttpPost(Name = "CreateProject")]
        public HttpResponseMessage CreateProject(string projectJson)
        {

        }

        [HttpGet(Name = "GetProject")]
        public HttpResponseMessage GetProject(Guid id)
        {

        }

        [HttpPatch(Name = "UpdateProject")]
        public HttpResponseMessage UpdateProject()
        {

        }

        [HttpDelete(Name = "DeleteProject")]
        public HttpResponseMessage DeleteProject()
        {

        }
    }
}
