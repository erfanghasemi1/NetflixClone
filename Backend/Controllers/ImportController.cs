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
        /// API Controller responsible for handling data import operations from the TMDB API.
        /// </summary>
        /// <remarks>
        /// This controller acts as the entry point for triggering import processes.
        /// It delegates the actual import logic to <see cref="IImportOrchestrator"/>.
        /// <para>
        /// Base route: <c>api/import</c>
        /// </para>
        /// </remarks>

        [HttpPost("all")]
        public async Task<IActionResult> ImportAll()
        {
            await _orchestrator.ImportAllAsync();
            return Ok(new {message = "Import completed successfully JOJO."});
        }
    }
}
