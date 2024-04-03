using Microsoft.Maui.Controls;
using WorkHours.Models;
using WorkHours.ViewModels;

namespace WorkHours.Views.Pages;

public partial class EditLocationsPage : ContentPage
{
    private readonly EditLocationViewModel _editLocationViewModel;

    public EditLocationsPage(EditLocationViewModel editLocationViewModel)
    {
        InitializeComponent();
        _editLocationViewModel = editLocationViewModel;
        BindingContext = editLocationViewModel;
    }
}