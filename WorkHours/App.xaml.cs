using WorkHours.ViewModels;
using WorkHours.Views;

namespace WorkHours;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        if (!Preferences.ContainsKey("HasLaunchedBefore"))
            MainPage = new FirstLaunchView();
        else
            MainPage = new AppShell();
        

        if (Application.Current != null)
            Application.Current.RequestedThemeChanged += (s, e) =>
            {
                if (Shell.Current.CurrentPage?.BindingContext is DataViewModel vm)
                {
                    vm.Init();
                }
            };
    }
}