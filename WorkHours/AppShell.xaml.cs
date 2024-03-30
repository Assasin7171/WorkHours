using WorkHours.Views.Pages;

namespace WorkHours;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(EditLocationsPage), typeof(EditLocationsPage));
        Routing.RegisterRoute(nameof(UserSettingsPage), typeof(UserSettingsPage));
    }
    
    
}