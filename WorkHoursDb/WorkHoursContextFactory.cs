using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WorkHoursDb;

public class WorkHoursContextFactory : IDesignTimeDbContextFactory<WorkHoursContext>
{
    public WorkHoursContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WorkHoursContext>();

        // Tu nie możesz używać FileSystem.AppDataDirectory
        var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "workhours-dev.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new WorkHoursContext(optionsBuilder.Options);
    }
}