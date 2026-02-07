using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TaskMangementApp.DB;
using TaskMangementApp.Models;
using TaskMangementApp.Services;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController(ProjectService projectService)
    {
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] string projectJson)
        {
            if (string.IsNullOrWhiteSpace(projectJson))
                return new BadRequestResult();

            var result = await projectService.CreateProjectAsync(projectJson);

            if (!result.Success)
                return new BadRequestObjectResult(result.Message);
            
            return new OkResult();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(Guid id)
        {

            if (id == Guid.Empty)
                return new BadRequestResult();

            var result = await projectService.GetProjectAsync(id);

            if (!result.Success)
                return new BadRequestObjectResult(result.Message);

            return new OkObjectResult(result.Project);   
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] string projectJson)
        {
            if (string.IsNullOrWhiteSpace(projectJson))
                return new BadRequestResult();

            var result = await projectService.UpdateProjectAsync(id, projectJson);

            if (!result.Success)
                return new BadRequestObjectResult(result.Message);

            return new OkResult();            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            if (id == Guid.Empty)
                return new BadRequestResult();

            var result = await projectService.DeleteProjectAsync(id);

            return new OkResult();       
        }
    }
}
