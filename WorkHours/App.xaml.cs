using WorkHours.ViewModels;

namespace WorkHours;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

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