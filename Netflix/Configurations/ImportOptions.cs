namespace Netflix.Configurations
{

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
