using Microsoft.Extensions.Logging;
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
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        //database starting config
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "db.db3");
        builder.Services.AddSingleton<DBService>(new DBService(dbPath));

        //view models
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<DataViewViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();
        builder.Services.AddTransient<RegisterUserViewModel>();
        builder.Services.AddTransient<LoginUserViewModel>();

        //views - pages
        builder.Services.AddTransient<MainApp>();
        builder.Services.AddTransient<DataView>();
        builder.Services.AddTransient<SettingsView>();
        builder.Services.AddTransient<RegisterUserView>();
        builder.Services.AddTransient<LoginView>();
        
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}