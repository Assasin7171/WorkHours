using Microsoft.Maui.Controls;
using WorkHours.ViewModels;

namespace WorkHours.Views;

public partial class LoginView : ContentPage
{
    private readonly StartingPageViewModel _startingPageViewModel;

    public LoginView(StartingPageViewModel startingPageViewModel)
    {
        _startingPageViewModel = startingPageViewModel;
        InitializeComponent();

        BindingContext = _startingPageViewModel;
    }
}