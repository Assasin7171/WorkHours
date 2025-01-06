using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkHours.Entities;
using WorkHours.Pages;

namespace WorkHours;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton<WorkHoursDbContext>();
        
        
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<SettingsPageViewModel>();
        builder.Services.AddSingleton<StatisticsPage>();
        builder.Services.AddSingleton<StatisticsPageViewModel>();

        
        var scope = builder.Services.BuildServiceProvider();
        var dbContext= scope.GetRequiredService<WorkHoursDbContext>();
        
        var pendingMigrations = dbContext.Database.GetPendingMigrations();
        if (!pendingMigrations.Any())
        {
            dbContext.Database.Migrate();
            if (dbContext.Database.EnsureCreated())
            {
                Shell.Current.DisplayAlert("Info", "Database is created now", "OK");
            }
        }
        
        return builder.Build();
    }
}