using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkHours.Pages;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsPageViewModel _viewModel;

    public SettingsPage(SettingsPageViewModel viewModel)
    {
        _viewModel = viewModel;
        
        BindingContext = _viewModel;
        InitializeComponent();
    }
}