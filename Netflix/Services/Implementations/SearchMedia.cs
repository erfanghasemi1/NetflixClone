using Microsoft.EntityFrameworkCore;
using Netflix.Data;
using Netflix.DTOs.Search;
using Netflix.Services.Interfaces;
using Netflix.Entities;

namespace Netflix.Services.Implementations
{

    /// <summary>
    /// A service responsible for querying and filtering media records from the database
    /// based on user-provided search criteria, including content type, genre, minimum rating,
    /// and release year, while delegating result ordering to a dedicated sorting method.
    /// </summary>
    
    public class SearchMedia : ISearchMedia
    {
        private readonly ApplicationDbContext _db;

        public SearchMedia(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Builds and executes a dynamic database query based on the provided search criteria,
        /// applying optional filters for rating, content type, release year, and genres,
        /// then returns the sorted results mapped to a list of search response DTOs.
        /// </summary>

        public async Task<List<SearchResponseDto>> SearchAsync(SearchRequestDto request)
        {
            var result = _db.Media.AsNoTracking().AsQueryable();

            if (request.MinRate.HasValue)
                result = result.Where(m => m.VoteAverage >= request.MinRate);

            if (request.ContentType.HasValue)
            {
                if (request.ContentType == 1)
                    result = result.Where(m => m.MediaTypeId == 1);
                else if (request.ContentType == 2)
                    result = result.Where(m => m.MediaTypeId == 2);
            }

            if (request.ReleaseYear.HasValue)
            {
                DateOnly minDate = new DateOnly(request.ReleaseYear.Value, 1, 1);
                result = result.Where(m => m.ReleaseDate >= minDate);
            }

            if (request.GenresId != null && request.GenresId.Count > 0)
            {
                result = result.Where(m =>
                                      m.MediaGenres.Any(
                                          mg => request.GenresId.Contains(mg.GenreId)));
            }

            var orderedResult = SortMediaAsync(result, request);

            return await orderedResult.Select(m => new SearchResponseDto
            {
                Id = m.Id,
                MediaTypeId = m.MediaTypeId,
                Title = m.Title,
                Overview = m.Overview,
                PosterPath = m.PosterPath,
                BackdropPath = m.BackdropPath,
                ReleaseDate = m.ReleaseDate,
                VoteAverage = m.VoteAverage,
                VoteCount = m.VoteCount,
                Popularity = m.Popularity,
                GenresId = m.MediaGenres.Select(mg => mg.GenreId).ToList()
            }).ToListAsync();
        }

        /// <summary>
        /// Sorts the media query based on the search text relevance when provided, applying
        /// a weighted scoring system that prioritizes exact title matches (5), partial title 
        /// matches (4), and overview matches (3), followed by popularity, vote average, and 
        /// vote count as tiebreakers. Falls back to sorting purely by popularity, vote average,
        /// and vote count when no search text is present.
        /// </summary>

        public IOrderedQueryable<Media> SortMediaAsync(
            IQueryable<Media> result, SearchRequestDto request)
        {
            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                var term = request.SearchText.Trim().ToLower();

                return result.OrderByDescending(m =>
                    m.Title.ToLower() == term ? 5:
                    m.Title.ToLower().Contains(term) ? 4 :
                    m.Overview != null && m.Overview.ToLower().Contains(term) ? 3 : 0)
                    .ThenByDescending(m => m.Popularity)
                    .ThenByDescending(m => m.VoteAverage)
                    .ThenByDescending(m => m.VoteCount);
            }

            return result.OrderByDescending(m => m.Popularity)
                         .ThenByDescending(m => m.VoteAverage)
                         .ThenByDescending(m => m.VoteCount);
        }
    }
}
