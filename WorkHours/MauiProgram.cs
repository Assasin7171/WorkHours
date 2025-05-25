using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkHours.Db;
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

        builder.Services.AddScoped<MainViewModel>();
        builder.Services.AddScoped<MainView>(s => new MainView()
        {
            BindingContext = s.GetRequiredService<MainViewModel>(),
        });
        builder.Services.AddDbContext<WorkHoursContext>(options =>
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "workhours.db");
            options.UseSqlite($"Filename={dbPath}");
        });
        
        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<WorkHoursContext>();
            // db.Database.EnsureDeleted(); 
            db.Database.Migrate();
        }
        
        return builder.Build();
    }
}