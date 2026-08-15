using Microsoft.EntityFrameworkCore;
using Netflix.Entities;

namespace Netflix.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Media> Media => Set<Media>();

        public DbSet<Genre> Genres => Set<Genre>();

        public DbSet<MediaGenre> MediaGenres => Set<MediaGenre>();

        public DbSet<MediaType> MediaTypes => Set<MediaType>();

        public DbSet<ImportHistory> ImportHistories => Set<ImportHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MediaGenre>()
                .HasKey(x => new { x.MediaId, x.GenreId });

            modelBuilder.Entity<Media>()
                .HasIndex(x => x.TmdbId)
                .IsUnique();

            modelBuilder.Entity<Genre>()
                .HasIndex(x => x.TmdbGenreId)
                .IsUnique();

            modelBuilder.Entity<Media>()
                .HasOne(x => x.MediaType)
                .WithMany(x => x.Media)
                .HasForeignKey(x => x.MediaTypeId);

            modelBuilder.Entity<MediaGenre>()
                .HasOne(x => x.Media)
                .WithMany(x => x.MediaGenres)
                .HasForeignKey(x => x.MediaId);

            modelBuilder.Entity<MediaGenre>()
                .HasOne(x => x.Genre)
                .WithMany(x => x.MediaGenres)
                .HasForeignKey(x => x.GenreId);

            modelBuilder.Entity<MediaType>().HasData(
                new MediaType { Id = 1, Name = "Movie" },
                new MediaType { Id = 2, Name = "TV" }
            );
        }
    }
}
