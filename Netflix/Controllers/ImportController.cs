using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Netflix.Services.Interfaces;

namespace Netflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IImportOrchestrator _orchestrator;
        public ImportController(IImportOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        /// <summary>
        /// One-Time endpoint to import all data from TMDB
        /// Call it when you wanna populate database
        /// </summary>

        [HttpPost("all")]
        public async Task<IActionResult> ImportAll()
        {
            await _orchestrator.ImportAllAsync();
            return Ok(new {message = "Import completed successfully JOJO."});
        }
    }
}
