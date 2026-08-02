namespace Netflix.DTOs.Stats
{
    public class MediaSampleDto
    {
        public int TmdbId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string MediaType {  get; set; } = string.Empty;
        public decimal VoteAvg { get; set; }
        public int VoteCount { get; set; }
        public decimal Popularity { get; set; }
    }
}
