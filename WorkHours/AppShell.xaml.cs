using WorkHours.Pages;

namespace WorkHours;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
    }
}