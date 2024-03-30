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