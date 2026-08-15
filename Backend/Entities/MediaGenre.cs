namespace Netflix.Entities
{
    public class MediaGenre
    {
        public long MediaId { get; set; }

        public Media Media { get; set; } = null!;

        public int GenreId { get; set; }

        public Genre Genre { get; set; } = null!;
    }
}
