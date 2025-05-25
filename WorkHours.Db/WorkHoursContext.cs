using Microsoft.EntityFrameworkCore;
using WorkHours.Db.Tables;

namespace WorkHours.Db;

public class WorkHoursContext : DbContext
{
    public DbSet<Worksession> Worksessions { get; set; }

    public WorkHoursContext(DbContextOptions<WorkHoursContext> options)
        : base(options)
    {
    }

    public WorkHoursContext()
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Worksession>()
            .HasKey(s => s.Id);
        modelBuilder.Entity<Place>()
            .HasKey(p=>p.Id);
    }
}