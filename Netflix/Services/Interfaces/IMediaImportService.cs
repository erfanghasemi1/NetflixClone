namespace Netflix.Services.Interfaces
{
    public interface IMediaImportService
    {
        Task ImportMoviesByGenreAsync();

        Task ImportTvSeriesByGenreAsync();

        Task ImportDiscoverMoviesAsync();

        Task ImportDiscoverTvSeriesAsync();
    }
}
