using Microsoft.Maui.Controls;

namespace WorkHours;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}