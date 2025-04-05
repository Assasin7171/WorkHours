using CommunityToolkit.Maui;
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
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        //builder.Services.AddSingleton<WorkHoursDbContext>();
        builder.Services.AddDbContext<WorkHoursDbContext>(
            options => options.UseSqlite($"Filename={GetDatabasePath()}", x=>x.MigrationsAssembly(nameof(WorkHoursDb))));

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddSingleton<SettingsPageViewModel>();
        builder.Services.AddSingleton<StatisticsPage>();
        builder.Services.AddSingleton<StatisticsPageViewModel>();
        builder.Services.AddSingleton<WorkSessionDetailsPage>();
        builder.Services.AddSingleton<WorkSessionDetailsViewModel>();


        //var scope = builder.Services.BuildServiceProvider();
        //var dbContext= scope.GetRequiredService<WorkHoursDbContext>();

        //var pendingMigrations = dbContext.Database.GetPendingMigrations();
        //if (!pendingMigrations.Any())
        //{
        //    dbContext.Database.EnsureCreated();
        //    dbContext.Database.Migrate();
        //}

        return builder.Build();
    }

    private static string GetDatabasePath()
    {
        var dataBasePath = "";
        var databaseName = "WorkHours.db";

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            dataBasePath = Path.Combine(FileSystem.AppDataDirectory, databaseName);
        }
        else if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            dataBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..",
                "Library", databaseName);
        }

        return dataBasePath;
    }
}