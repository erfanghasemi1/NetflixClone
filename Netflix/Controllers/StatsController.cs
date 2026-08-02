using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netflix.Data;
using Netflix.DTOs.Stats;
using Netflix.Services.Implementations;
using Netflix.Services.Interfaces;

namespace Netflix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public IGetMediaSample getMoviesSample;

        public StatsController(ApplicationDbContext db , IGetMediaSample GMS)
        {
            _db = db;
            getMoviesSample = GMS;

        }

        /// <summary>
        /// Returns Count Of All Tables 
        /// </summary>

        [HttpGet("tables")]
        public async Task<ActionResult<List<TableStatsDto>>> GetTableStats()
        {
            List<TableStatsDto> Stats = new List<TableStatsDto>
            {
                new () {TableName = "Genres" , Count = await _db.Genres.CountAsync()} , 
                new () {TableName = "Media" , Count = await _db.Media.CountAsync()} ,
                new () {TableName = "MediaGenres" , Count = await _db.Media.CountAsync()} , 
                new () {TableName = "MediaTypes" , Count = await _db.MediaTypes.CountAsync()} ,
                new () {TableName = "ImportHistories" , Count= await _db.MediaTypes.CountAsync()}
            };
            return Ok(Stats);
        }
        /// <summary>
        /// Returns Top N Media Samples
        /// </summary>
        [HttpGet("top")]
        public async Task<ActionResult<List<MediaSampleDto>>> GetSamples([FromQuery] int count = 5)
        {
            List<MediaSampleDto> TopSamples = await getMoviesSample.GetMediaSampleAsync(count);
            return Ok(TopSamples);
        }
    }
}
