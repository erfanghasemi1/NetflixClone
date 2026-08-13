namespace Netflix.Configurations
{
    /// <summary>
    /// Represents the configuration options for importing data from the TMDB API.
    /// This class is used to control which data to retrieve and how much of it to fetch.
    /// </summary>
    /// <remarks>
    /// This class is typically bound to the <c>Import</c> section in appsettings.json.
    /// It is marked as <see langword="sealed"/> to prevent inheritance.
    /// </remarks>
    public sealed class ImportOptions
    {
        public const string SectionName = "Import";

        public int PagesPerGenre { get; set; } = 3;

        public int DiscoverPages { get; set; } = 10;

        public int MinimumVoteCount { get; set; } = 100;

        public bool ImportMovies { get; set; } = true;

        public bool ImportTvSeries { get; set; } = true;
    }
}
