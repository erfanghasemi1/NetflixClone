namespace Netflix.Services.Interfaces
{
    public interface IGenreImportService
    {
        Task ImportMovieGenresAsync();

        Task ImportTvGenresAsync();
    }
}
