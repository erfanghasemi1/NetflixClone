using Microsoft.EntityFrameworkCore;
using Netflix.Data;
using Netflix.Entities;
using Netflix.Services.Interfaces;

namespace Netflix.Services.Implementations
{

    /// <summary>
    /// A sealed service responsible for synchronizing genre data between the TMDB API and the local database.
    /// It fetches movie and TV genres, identifies new genres by comparing TMDB identifiers, 
    /// and performs an upsert operation to ensure the local genre repository is up to date without duplicates.
    /// </summary>


    public sealed class GenreImportService : IGenreImportService
    {
        private readonly ApplicationDbContext _db;
        private readonly ITmdbService _tmdb;

        public GenreImportService(ApplicationDbContext db, ITmdbService tmdb)
        {
            _db = db;
            _tmdb = tmdb;
        }

        public async Task ImportMovieGenresAsync()
        {
            var response = await _tmdb.GetMovieGenresAsync();
            await UpsertGenresAsync(response.Genres);
        }

        public async Task ImportTvGenresAsync()
        {
            var response = await _tmdb.GetTvGenresAsync();
            await UpsertGenresAsync(response.Genres);

        }


        /// <summary>
        /// Compares a list of TMDB genre data against existing database records and adds missing genres 
        /// based on their unique TMDB identifier before saving changes.
        /// </summary>

        private async Task UpsertGenresAsync(List<DTOs.Tmdb.GenreDto> genres)
        {
            var existingTmdbIds = (await _db.Genres
                .Select(g => g.TmdbGenreId)
                .ToListAsync());


            foreach (var dto in genres)
            {
                if(!existingTmdbIds.Contains(dto.Id))
                {
                    _db.Genres.Add(new Genre
                    {
                        Name = dto.Name,
                        TmdbGenreId = dto.Id
                    });

                }
            }

            await _db.SaveChangesAsync();
        }
    }
}