using Microsoft.EntityFrameworkCore;

namespace WorkHours.Entities;

public class WorkHoursDbContext : DbContext
{
    public DbSet<WorkSession> WorkSessions { get; set; }
    public DbSet<Place> Places { get; set; }

    //for adding/removing migrations from class library
    public WorkHoursDbContext()
    {
    }

    public WorkHoursDbContext(DbContextOptions<WorkHoursDbContext> options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //var dbPath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), "WorkHours.db"); //$"Data Source={dbPath}"
        optionsBuilder.UseSqlite();
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
            eb.HasData(new List<Place>().Append(new Place()
                { Id = 1, WorkSessions = null, Name = "Wybierz miejsce pracy" }));
        });
    }
}