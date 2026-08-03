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
        [HttpGet]
        public async Task<ActionResult<List<Media>>> SearchAsync([FromQuery] SearchRequestDto request)
        {
            var result = await _searchMedia.SearchAsync(request);
            return Ok(result);
        }
    }
}
