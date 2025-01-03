using Microsoft.EntityFrameworkCore;

namespace ClassLib.Entities;

public class WorkHoursDbContext : DbContext
{
    public DbSet<WorkSession> WorkSessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var path = Path.Combine(Environment.CurrentDirectory, "WorkHours.db");
        optionsBuilder.UseSqlite($"Data Source={path}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WorkSession>(eb =>
        {
            eb.HasKey(x => x.Id);
        });
    }
}