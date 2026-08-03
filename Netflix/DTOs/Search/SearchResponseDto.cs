namespace Netflix.DTOs.Search
{
    public class SearchResponseDto
    {
        public long Id { get; set; }

        public int MediaTypeId { get; set; }

        public string Title { get; set; } = null!;

        public string? Overview { get; set; }

        public string? PosterPath { get; set; }

        public string? BackdropPath { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public decimal VoteAverage { get; set; }

        public int VoteCount { get; set; }

        public decimal Popularity { get; set; }
        public List<int> GenresId { get; set; } = new List<int>();
    }
}
