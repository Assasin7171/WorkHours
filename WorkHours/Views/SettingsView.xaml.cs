using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHours.ViewModels;

namespace WorkHours.Views;

public partial class SettingsView : ContentPage
{
    private readonly SettingsViewModel _settingsViewModel;

    public SettingsView(SettingsViewModel settingsViewModel)
    {
        _settingsViewModel = settingsViewModel;
        
        InitializeComponent();
        
        this.BindingContext = _settingsViewModel;
    }
}