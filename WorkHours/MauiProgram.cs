using Microsoft.Extensions.Logging;
using WorkHours.Services;
using WorkHours.ViewModels;
using WorkHours.Views;
using WorkHours.Views.Pages;

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
                fonts.AddFont("Poppins-Medium.ttf", "Poppins-Medium");
            });
        //database starting config or services
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "db.db3");
        builder.Services.AddSingleton(new DBService(dbPath));
        builder.Services.AddSingleton<UserService>();

        //view models
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<DataViewViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<EditLocationViewModel>();
        builder.Services.AddTransient<StartingPageViewModel>();

        //views - pages
        builder.Services.AddTransient<MainApp>();
        builder.Services.AddTransient<DataView>();
        builder.Services.AddTransient<SettingsView>();
        builder.Services.AddTransient<EditLocationsPage>();
        builder.Services.AddTransient<LoginView>();

        //views - components

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}