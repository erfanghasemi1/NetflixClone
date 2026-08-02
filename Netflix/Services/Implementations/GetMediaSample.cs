using Microsoft.EntityFrameworkCore;
using Netflix.Data;
using Netflix.DTOs.Stats;
using Netflix.Services.Interfaces;

namespace Netflix.Services.Implementations
{
    public class GetMediaSample : IGetMediaSample
    {
        private readonly ApplicationDbContext _db;

        public GetMediaSample(ApplicationDbContext db)
        {
            _db = db;   
        }

        public async Task<List<MediaSampleDto>> GetMediaSampleAsync(int count)
        {
            return await _db.Media
                          .OrderByDescending(m => m.Popularity)
                          .Take(count)
                          .Select(m => new MediaSampleDto()
                          {
                                TmdbId = m.TmdbId,
                                Title = m.Title,
                                MediaType = m.MediaTypeId == 1 ? "Movie" : "Tv-Series",
                                VoteAvg = m.VoteAverage,
                                VoteCount = m.VoteCount,
                                Popularity = m.Popularity
                          })
                          .ToListAsync();
        }
    }
}
