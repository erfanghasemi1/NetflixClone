using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Netflix.Configurations;
using Netflix.Data;
using Netflix.DTOs.Tmdb;
using Netflix.Entities;
using Netflix.Enums;
using Netflix.Services.Interfaces;

namespace Netflix.Services.Implementations
{
    public sealed class MediaImportService : IMediaImportService
    {
        private readonly ApplicationDbContext _db;
        private readonly ITmdbService _tmdb;
        private readonly ImportOptions _options;
        private readonly ILogger<MediaImportService> _logger;

        public MediaImportService(
            ApplicationDbContext db,
            ITmdbService tmdb,
            IOptions<ImportOptions> options,
            ILogger<MediaImportService> logger)
        {
            _db = db;
            _tmdb = tmdb;
            _options = options.Value;
            _logger = logger;
        }

        public async Task ImportMoviesByGenreAsync()
        {
            if (!_options.ImportMovies) return;

            _logger.LogInformation("[IMPORT] Starting Movies By Genre import...");
            var genres = await _db.Genres.ToListAsync();

            await ImportByGenreAsync(
                genres,
                mediaTypeId: 1,
                getPage: (genreId, sort, page) =>
                    _tmdb.GetMoviesByGenreAsync(genreId, sort, page));

            _logger.LogInformation("[IMPORT] Movies Imported By Genre Successfully!");
        }

        public async Task ImportTvSeriesByGenreAsync()
        {
            if (!_options.ImportTvSeries) return;

            _logger.LogInformation("[IMPORT] Starting TV Series By Genre import...");
            var genres = await _db.Genres.ToListAsync();

            await ImportByGenreAsync(
                genres,
                mediaTypeId: 2,
                getPage: (genreId, sort, page) =>
                    _tmdb.GetTvSeriesByGenreAsync(genreId, sort, page));

            _logger.LogInformation("[IMPORT] TV Series Imported By Genre Successfully!");
        }

        public async Task ImportDiscoverMoviesAsync()
        {
            if (!_options.ImportMovies) return;

            _logger.LogInformation("[IMPORT] Starting Discover Movies import...");

            await ImportDiscoverAsync(
                mediaTypeId: 1,
                getPage: (sort, page) => _tmdb.DiscoverMoviesAsync(sort, page));

            _logger.LogInformation("[IMPORT] Movies Imported By Discover Successfully!");
        }

        public async Task ImportDiscoverTvSeriesAsync()
        {
            if (!_options.ImportTvSeries) return;

            _logger.LogInformation("[IMPORT] Starting Discover TV Series import...");

            await ImportDiscoverAsync(
                mediaTypeId: 2,
                getPage: (sort, page) => _tmdb.DiscoverTvSeriesAsync(sort, page));

            _logger.LogInformation("[IMPORT] TV Series Imported By Discover Successfully!");
        }

        private async Task ImportByGenreAsync(
            List<Genre> genres,
            int mediaTypeId,
            Func<int, TmdbSortType, int, Task<MediaResponseDto>> getPage)
        {
            var sortTypes = new[]
            {
                TmdbSortType.Popularity,
                TmdbSortType.TopRated,
                TmdbSortType.MostVoted,
                TmdbSortType.Newest
            };

            // Pre-fetch all known genres into memory once to avoid querying DB per item
            var genreMap = await _db.Genres.ToDictionaryAsync(g => g.TmdbGenreId, g => g.Id);

            foreach (var genre in genres)
            {
                _logger.LogInformation("[IMPORT] Processing Genre: {GenreName} (ID: {GenreId})", genre.Name, genre.TmdbGenreId);

                foreach (var sort in sortTypes)
                {
                    for (int page = 1; page <= _options.PagesPerGenre; page++)
                    {
                        _logger.LogInformation("[IMPORT] Fetching {MediaType} | Genre: {Genre} | Sort: {Sort} | Page {Page}/{MaxPage}",
                            mediaTypeId == 1 ? "Movies" : "TV", genre.Name, sort, page, _options.PagesPerGenre);

                        var response = await getPage(genre.TmdbGenreId, sort, page);

                        if (response?.Results != null && response.Results.Count > 0)
                        {
                            await ProcessPageBatchAsync(response.Results, mediaTypeId, genreMap);
                        }
                    }
                }
            }
        }

        private async Task ImportDiscoverAsync(
            int mediaTypeId,
            Func<TmdbSortType, int, Task<MediaResponseDto>> getPage)
        {
            var sortTypes = new[]
            {
                TmdbSortType.Popularity,
                TmdbSortType.TopRated,
                TmdbSortType.MostVoted,
                TmdbSortType.Newest
            };

            var genreMap = await _db.Genres.ToDictionaryAsync(g => g.TmdbGenreId, g => g.Id);

            foreach (var sort in sortTypes)
            {
                for (int page = 1; page <= _options.DiscoverPages; page++)
                {
                    _logger.LogInformation("[IMPORT] Discover {MediaType} | Sort: {Sort} | Page {Page}/{MaxPage}",
                        mediaTypeId == 1 ? "Movies" : "TV", sort, page, _options.DiscoverPages);

                    var response = await getPage(sort, page);

                    if (response?.Results != null && response.Results.Count > 0)
                    {
                        await ProcessPageBatchAsync(response.Results, mediaTypeId, genreMap);
                    }
                }
            }
        }

        /// <summary>
        /// Processes an entire page of results in a SINGLE batch operation.
        /// Reduces SQL queries from ~100 per page to just 2-3 queries total.
        /// </summary>
        private async Task ProcessPageBatchAsync(
    List<MediaDto> dtos,
    int mediaTypeId,
    Dictionary<int, int> genreMap)
        {
            // 1. Deduplicate TMDb items coming in from the API page itself
            var distinctDtos = dtos
                .Where(d => d != null)
                .DistinctBy(d => d.Id)
                .ToList();

            var pageTmdbIds = distinctDtos.Select(d => d.Id).ToList();

            // 2. Fetch all existing media items by TmdbId (Globally, to respect IX_Media_TmdbId)
            var existingMedia = await _db.Media
                .Where(m => pageTmdbIds.Contains(m.TmdbId))
                .ToDictionaryAsync(m => m.TmdbId);

            var newMediaList = new List<Media>();
            var processedTmdbIds = new HashSet<int>(existingMedia.Keys);

            foreach (var dto in distinctDtos)
            {
                // 3. Ensure we don't insert duplicate TmdbIds in the same SaveChanges batch
                if (!processedTmdbIds.Contains(dto.Id))
                {
                    var media = new Media
                    {
                        TmdbId = dto.Id,
                        MediaTypeId = mediaTypeId,
                        Title = dto.Title ?? dto.Name ?? "Unknown",
                        OriginalTitle = dto.OriginalTitle ?? dto.OriginalName,
                        Overview = dto.Overview,
                        PosterPath = dto.PosterPath,
                        BackdropPath = dto.BackdropPath,
                        ReleaseDate = dto.ReleaseDate ?? dto.FirstAirDate,
                        VoteAverage = dto.VoteAverage,
                        VoteCount = dto.VoteCount,
                        Popularity = dto.Popularity,
                        OriginalLanguage = dto.OriginalLanguage,
                        Adult = dto.Adult,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    newMediaList.Add(media);
                    processedTmdbIds.Add(dto.Id); // Lock this ID for this execution batch
                }
            }

            // 4. Batch Insert missing media items
            if (newMediaList.Count > 0)
            {
                await _db.Media.AddRangeAsync(newMediaList);
                await _db.SaveChangesAsync();
                _logger.LogInformation("[IMPORT] Inserted {Count} new media records.", newMediaList.Count);
            }

            // Combine newly inserted and existing media items to map their genres
            var allPageMedia = existingMedia.Values.Concat(newMediaList).ToList();
            var allMediaIds = allPageMedia.Select(m => m.Id).ToList();

            // 5. Fetch existing genre links for these media items in 1 query
            var existingMediaGenres = await _db.MediaGenres
                .Where(mg => allMediaIds.Contains(mg.MediaId))
                .Select(mg => new { mg.MediaId, mg.GenreId })
                .ToListAsync();

            var existingGenreSet = existingMediaGenres
                .Select(mg => (mg.MediaId, mg.GenreId))
                .ToHashSet();

            var newMediaGenres = new List<MediaGenre>();

            foreach (var media in allPageMedia)
            {
                var dto = distinctDtos.FirstOrDefault(d => d.Id == media.TmdbId);
                if (dto?.GenreIds == null) continue;

                foreach (var tmdbGenreId in dto.GenreIds)
                {
                    if (genreMap.TryGetValue(tmdbGenreId, out int localGenreId))
                    {
                        if (!existingGenreSet.Contains((media.Id, localGenreId)))
                        {
                            newMediaGenres.Add(new MediaGenre
                            {
                                MediaId = media.Id,
                                GenreId = localGenreId
                            });

                            existingGenreSet.Add((media.Id, localGenreId));
                        }
                    }
                }
            }

            // 6. Batch Insert new Genre relationships
            if (newMediaGenres.Count > 0)
            {
                await _db.MediaGenres.AddRangeAsync(newMediaGenres);
                await _db.SaveChangesAsync();
            }
        }
    }
}