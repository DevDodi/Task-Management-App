using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TaskMangementApp.DB;
using TaskMangementApp.Models;
using TaskMangementApp.Services;
using TaskMangementApp.Services.Interfaces;

namespace TaskMangementApp.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/projects")]
    public class ProjectController(IProjectService projectService)
    {
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] JsonElement projectJson)
        {
            var result = await projectService.CreateProjectAsync(projectJson);

            if (!result.Success)
                return new BadRequestObjectResult(new { result.Message });
            
            return new OkResult();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(Guid id)
        {

            if (id == Guid.Empty)
                return new BadRequestResult();

            var result = await projectService.GetProjectAsync(id);

            if (!result.Success)
                return new BadRequestObjectResult(new { result.Message });

            return new OkObjectResult(new { result.Project });   
        }

        [HttpGet()]
        public async Task<ActionResult<Project>> GetProjectsAsync()
        {
            var result = await projectService.GetProjectsAsync();
            return new OkObjectResult(new { result.Projects });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] JsonElement projectJson)
        {
            var result = await projectService.UpdateProjectAsync(id, projectJson);

            if (!result.Success)
                return new BadRequestObjectResult(new { result.Message });

            return new OkResult();            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            if (id == Guid.Empty)
                return new BadRequestResult();

            var result = await projectService.DeleteProjectAsync(id);

            if (!result.Success)
                return new NotFoundObjectResult(new { result.Message });

            return new OkResult();       
        }
    }
}
