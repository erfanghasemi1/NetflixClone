using System.Text.Json.Serialization;

namespace Netflix.DTOs.Tmdb
{
    public sealed class MediaDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Name { get; set; }

        public string? OriginalTitle { get; set; }

        public string? OriginalName { get; set; }

        public string Overview { get; set; } = string.Empty;

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }

        [JsonPropertyName("backdrop_path")]
        public string? BackdropPath { get; set; }

        [JsonPropertyName("release_date")]
        public DateOnly? ReleaseDate { get; set; }

        [JsonPropertyName("first_air_date")]
        public DateOnly? FirstAirDate { get; set; }

        [JsonPropertyName("vote_average")]
        public decimal VoteAverage { get; set; }

        [JsonPropertyName("vote_count")]
        public int VoteCount { get; set; }

        public decimal Popularity { get; set; }

        [JsonPropertyName("original_language")]
        public string OriginalLanguage { get; set; } = string.Empty;

        public bool Adult { get; set; }

        [JsonPropertyName("genre_ids")]
        public List<int> GenreIds { get; set; } = [];
    }
}
