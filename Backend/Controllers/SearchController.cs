using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Netflix.Entities;
using Netflix.DTOs.Search;
using Netflix.Services.Interfaces;

namespace Netflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchMedia _searchMedia;

        public SearchController(ISearchMedia searchMedia)
        {
            _searchMedia = searchMedia;
        }

        /// <summary>
        /// Handles media search requests by receiving search criteria from the query string,
        /// delegating the search operation to the corresponding service, and returning the
        /// matching media results in a successful HTTP response.
        /// </summary>
        
        [HttpGet]
        public async Task<ActionResult<List<Media>>> SearchAsync([FromQuery] SearchRequestDto request)
        {
            var result = await _searchMedia.SearchAsync(request);
            return Ok(result);
        }
    }
}
