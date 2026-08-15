using System.Text.Json.Serialization;

namespace Netflix.DTOs.Tmdb
{
    public sealed class MediaResponseDto
    {
        public int Page { get; set; }

        public List<MediaDto> Results { get; set; } = [];

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
    }
}
