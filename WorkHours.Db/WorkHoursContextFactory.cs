using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WorkHours.Db;

public class WorkHoursContextFactory : IDesignTimeDbContextFactory<WorkHoursContext>
{
    public WorkHoursContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WorkHoursContext>();
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "workhours.db");
        optionsBuilder.UseSqlite($"Filename={dbPath}");

        return new WorkHoursContext(optionsBuilder.Options);
    }
}