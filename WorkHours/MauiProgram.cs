using System;
using System.IO;
using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Storage;
using WorkHours.Services;
using WorkHours.ViewModels;
using WorkHours.Views;
using WorkHoursDb;
using WorkHoursDb.Entities;

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
        
        //services
        builder.Services.AddSingleton<DataStoreService>();
        builder.Services.AddScoped<IDbInitializer, DbInitializer>();
        
        //Inicjalizacja bazy danych
        using (var scope = builder.Services.BuildServiceProvider().CreateScope())
        {
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            dbInitializer.Initialize();
        }

        
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainView>(b => new MainView()
        {
            BindingContext = b.GetService<MainViewModel>(),
        });
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<SettingsView>(b => new SettingsView()
        {
            BindingContext = b.GetService<SettingsViewModel>(),
        });

        return builder.Build();
    }
}