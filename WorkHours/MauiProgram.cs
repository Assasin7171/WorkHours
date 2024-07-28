using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
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
                fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddTransient<MainView>(s=> new MainView()
        {
            BindingContext = s.GetRequiredService<MainViewModel>(),
        });
        builder.Services.AddTransient<MainViewModel>();

        builder.Services.AddTransient<DataView>(s => new DataView()
        {
            BindingContext = s.GetRequiredService<DataViewModel>(),
        });
        builder.Services.AddTransient<DataViewModel>();

        builder.Services.AddTransient<SettingsView>(s => new SettingsView()
        {
            BindingContext = s.GetRequiredService<SettingsViewModel>(),
        });
        builder.Services.AddTransient<SettingsViewModel>();

        return builder.Build();
    }
}