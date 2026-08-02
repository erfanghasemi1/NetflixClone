namespace Netflix.DTOs.Stats
{
    public sealed class GenreStatsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MediaCount { get; set; }
    }
}
