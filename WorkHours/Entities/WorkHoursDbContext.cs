using Microsoft.EntityFrameworkCore;

namespace WorkHours.Entities;

public class WorkHoursDbContext : DbContext
{
    public DbSet<WorkSession> WorkSessions { get; set; }
    public DbSet<Place> Places { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "WorkHours.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WorkSession>(eb =>
        {
            eb.HasKey(x => x.Id);
            eb.HasOne(x => x.Place).WithMany(x => x.WorkSessions).HasForeignKey(x => x.PlaceId);
            eb.Property(x => x.Descriptions).HasMaxLength(200);
        });

        modelBuilder.Entity<Place>(eb =>
        {
            eb.HasKey(x => x.Id);
            eb.Property(x => x.Name).HasMaxLength(200).IsRequired();
            eb.HasMany(x => x.WorkSessions).WithOne(x => x.Place).HasForeignKey(x => x.PlaceId);
        });
    }
}