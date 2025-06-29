using CommunityToolkit.Maui;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using WorkHours.Services;
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
            .UseSkiaSharp()
            .UseMicrocharts()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        // klasa do inicjalizowania bazy danych
        builder.Services.AddSingleton<DatabaseContext>();
        builder.Services.AddSingleton<DataStoreService>();

        //services
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddTransient<MainView>(b => new MainView()
        {
            BindingContext = b.GetRequiredService<MainViewModel>(),
        });
        builder.Services.AddSingleton<DataViewModel>();
        builder.Services.AddTransient<DataView>(b => new DataView()
        {
            BindingContext = b.GetRequiredService<DataViewModel>(),
        });
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddTransient<SettingsView>(b => new SettingsView()
        {
            BindingContext = b.GetRequiredService<SettingsViewModel>(),
        });
        builder.Services.AddSingleton<SettlementsViewModel>();
        builder.Services.AddTransient<SettingsView>(b => new SettingsView()
        {
            BindingContext = b.GetRequiredService<SettingsViewModel>(),
        });


        return builder.Build();
    }
}