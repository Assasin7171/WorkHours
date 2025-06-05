using Microsoft.EntityFrameworkCore;
using WorkHoursDb;
using WorkHoursDb.Entities;

namespace WorkHours.Services;

public class DbInitializer : IDbInitializer
{
    private readonly WorkHoursContext _context;

    public DbInitializer(WorkHoursContext context)
    {
        _context = context;
    }

    public void Initialize()
    {
        // Tworzy bazę jeśli nie istnieje
        _context.Database.Migrate();
    }
}