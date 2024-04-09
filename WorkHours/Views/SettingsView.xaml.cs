using WorkHours.ViewModels;

namespace WorkHours.Views;

public partial class SettingsView : ContentPage
{
    private readonly SettingsViewModel _settingsViewModel;

    public SettingsView(SettingsViewModel settingsViewModel)
    {

        /* Unmerged change from project 'WorkHours (net8.0-maccatalyst)'
        Before:
                _settingsViewModel = settingsViewModel;

                InitializeComponent();
        After:
                _settingsViewModel = settingsViewModel;

                InitializeComponent();
        */

        /* Unmerged change from project 'WorkHours (net8.0-windows10.0.19041.0)'
        Before:
                _settingsViewModel = settingsViewModel;

                InitializeComponent();
        After:
                _settingsViewModel = settingsViewModel;

                InitializeComponent();
        */

        /* Unmerged change from project 'WorkHours (net8.0-ios)'
        Before:
                _settingsViewModel = settingsViewModel;

                InitializeComponent();
        After:
                _settingsViewModel = settingsViewModel;

                InitializeComponent();
        */
        _settingsViewModel = settingsViewModel;

        InitializeComponent();

        this.BindingContext = _settingsViewModel;
    }
}