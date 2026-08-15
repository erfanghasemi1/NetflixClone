using Netflix.DTOs.Tmdb;
using Netflix.Enums;

namespace Netflix.Services.Interfaces
{
    public interface ITmdbService
    {
        Task<GenreResponseDto> GetMovieGenresAsync();
        Task<GenreResponseDto> GetTvGenresAsync();

        Task<MediaResponseDto> GetMoviesByGenreAsync(
            int genreId, TmdbSortType sortType, int page);

        Task<MediaResponseDto> GetTvSeriesByGenreAsync(
            int genreId, TmdbSortType sortType, int page);

        Task<MediaResponseDto> DiscoverMoviesAsync(
            TmdbSortType sortType, int page);

        Task<MediaResponseDto> DiscoverTvSeriesAsync(
            TmdbSortType sortType, int page);
    }
}
