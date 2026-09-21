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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SeriesItem>(entity =>
        {
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Notes)
                .HasMaxLength(1000);

            entity.Property(e => e.Status)
                .HasConversion<string>();
        });
    }
}
