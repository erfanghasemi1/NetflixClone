namespace Netflix.DTOs.Search
{

    /// <summary>
    /// Data Transfer Object representing the media information returned to the client 
    /// after a search operation, containing core details such as identity, titles, 
    /// visual asset paths, release dates, popularity metrics, and associated genre identifiers.
    /// </summary>


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
