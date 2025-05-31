using Microsoft.EntityFrameworkCore;
using WorkHours.Db.Entities;

namespace WorkHours.Db;

public class WorkHoursContext : DbContext
{
    public DbSet<Worksession> Worksessions { get; set; }
    public DbSet<Place> Places { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Worksession>(eb =>
        {
            eb.Property(x=>x.CreatedTime).ValueGeneratedOnAdd();
            eb.HasOne(x => x.Place).WithMany();
        });
    }
}