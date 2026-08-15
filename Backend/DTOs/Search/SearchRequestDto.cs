using System.ComponentModel.DataAnnotations;

namespace Netflix.DTOs.Search
{

    /// <summary>
    /// Represents the search criteria submitted by the user when querying media,
    /// including an optional search text, content type, list of genre identifiers,
    /// minimum rating (0 to 10), and release year (1900 to 2100), where the rating
    /// and release year values are validated against their allowed ranges.
    /// </summary>
    

    public class SearchRequestDto
    {
        public string? SearchText { get; set; }
        public int? ContentType { get; set; }
        public List<int>? GenresId { get; set; }

        [Range(0,10,ErrorMessage =("Minimum Rate Must Be Between 0 to 10."))]
        public decimal? MinRate { get; set; }

        [Range(1900,2100,ErrorMessage =("Released Year Must be Between 1900 to 2100."))]
        public int? ReleaseYear { get; set; }
    }
}
