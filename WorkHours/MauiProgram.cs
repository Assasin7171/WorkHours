using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkHours.Entities;
using WorkHours.ViewModels;
using WorkHours.Views;

namespace WorkHours;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        
        builder.Services.AddDbContext<WorkHoursContext>(op =>
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "workhours.db");
            op.UseSqlite($"Filename={dbPath}");
        });
        
        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<WorkHoursContext>();
            if (db.Database.CanConnect())
            {
                var places = db.Places.ToList();
                var worksessions = db.Worksessions.ToList();
            
                if (places.Count <= 0)
                {
                    db.Places.AddRange(
                        new Place()
                        {
                            Id = Guid.NewGuid(), Name = "Home"
                        },
                        new Place()
                        {
                            Id = Guid.NewGuid(), Name = "Office"
                        });
                }

                if (worksessions.Count <= 0)
                {
                    db.Worksessions.AddRange(
                        new Worksession()
                        {
                            Id = Guid.NewGuid(),
                            CreatedTime = new DateTime(2025,4,10),
                            HoursWorked = 4,
                            Place = places.First(),
                        },
                        new Worksession()
                        {
                            Id = Guid.NewGuid(),
                            CreatedTime = new DateTime(2025,3,16),
                            HoursWorked = 10,
                            Place = places.Last(),
                        });
                }
            
                db.SaveChanges();
                db.Database.Migrate();
            }
            
        }
        
        return builder.Build();
    }
}