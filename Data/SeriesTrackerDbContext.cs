using Microsoft.EntityFrameworkCore;
using Series_Tracker.Models;

namespace Series_Tracker.Data;

public class SeriesTrackerDbContext : DbContext
{
    public SeriesTrackerDbContext(DbContextOptions<SeriesTrackerDbContext> options)
        : base(options)
    {
    }

    public DbSet<SeriesItem> Series => Set<SeriesItem>();

    public DbSet<SeriesTitle> SeriesTitles => Set<SeriesTitle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SeriesItem>(entity =>
        {
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Author)
                .IsRequired()
                .HasMaxLength(200);

            entity.HasMany(e => e.Titles)
                .WithOne(e => e.SeriesItem)
                .HasForeignKey(e => e.SeriesItemId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SeriesTitle>(entity =>
        {
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.State)
                .HasConversion<string>();

            entity.HasIndex(e => new { e.SeriesItemId, e.Position })
                .IsUnique();
        });
    }
}
