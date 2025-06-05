using Microsoft.EntityFrameworkCore;
using WorkHoursDb.Entities;

namespace WorkHoursDb;

public class WorkHoursContext : DbContext
{
    public DbSet<Worksession> Worksessions { get; set; }
    public DbSet<Place> Places { get; set; }

    public WorkHoursContext(DbContextOptions options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Worksession>(eb =>
        {
            eb.HasOne(x => x.Place).WithMany();
            eb.Property(x => x.CreatedTime).IsRequired();
        });

        modelBuilder.Entity<Place>(eb =>
        {
            eb.Property(x => x.Description).IsRequired().HasMaxLength(50);
        });
    }
}