namespace Netflix.Entities
{
    public class Genre
    {
        public int Id {  get; set; }
        public string Name { get; set; } = null!;
        public int TmdbGenreId { get; set; }
        public ICollection<MediaGenre> MediaGenres { get; set; } = new List<MediaGenre>();
    }
}
