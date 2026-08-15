namespace Netflix.Entities
{
    public class Media
    {
        public long Id { get; set; }

        public int TmdbId { get; set; }

        public int MediaTypeId { get; set; }

        public MediaType MediaType { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? OriginalTitle { get; set; }

        public string? Overview { get; set; }

        public string? PosterPath { get; set; }

        public string? BackdropPath { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public decimal VoteAverage { get; set; }

        public int VoteCount { get; set; }

        public decimal Popularity { get; set; }

        public string? OriginalLanguage { get; set; }

        public bool Adult { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<MediaGenre> MediaGenres { get; set; } = new List<MediaGenre>();
    }
}
