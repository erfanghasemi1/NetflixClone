using Netflix.Data;
using Netflix.Entities;
using Netflix.Services.Interfaces;

namespace Netflix.Services.Implementations
{
    public sealed class ImportOrchestrator : IImportOrchestrator
    {
        private readonly ApplicationDbContext _db;
        private readonly IGenreImportService _genreImport;
        private readonly IMediaImportService _mediaImport;

        public ImportOrchestrator(
            ApplicationDbContext db,
            IGenreImportService genreImport,
            IMediaImportService mediaImport)
        {
            _db = db;
            _genreImport = genreImport;
            _mediaImport = mediaImport;
        }

        public async Task ImportAllAsync()
        {
            var history = new ImportHistory
            {
                StartedAt = DateTime.UtcNow,
                Status = "Running"
            };

            _db.ImportHistories.Add(history);
            await _db.SaveChangesAsync();

            try
            {
                // 1. Genres first
                await _genreImport.ImportMovieGenresAsync();
                await _genreImport.ImportTvGenresAsync();

                // 2. Media
                await _mediaImport.ImportMoviesByGenreAsync();
                await _mediaImport.ImportTvSeriesByGenreAsync();
                await _mediaImport.ImportDiscoverMoviesAsync();
                await _mediaImport.ImportDiscoverTvSeriesAsync();

                history.Status = "Completed";
                history.FinishedAt = DateTime.UtcNow;
            }
            catch (Exception)
            {
                history.Status = "Failed";
                history.FinishedAt = DateTime.UtcNow;
                throw;
            }
            finally
            {
                await _db.SaveChangesAsync();
            }
        }
    }
}