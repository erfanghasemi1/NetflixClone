namespace Netflix.Entities
{
    public class ImportHistory
    {
        public int Id { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime? FinishedAt { get; set; }

        public string Status { get; set; } = null!;

        public int ImportedCount { get; set; }

        public int UpdatedCount { get; set; }

        public int FailedCount { get; set; }
    }
}
