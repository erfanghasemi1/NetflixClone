namespace Netflix.Entities
{
    public class MediaType
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<Media> Media { get; set; } = new List<Media>();
    }
}
