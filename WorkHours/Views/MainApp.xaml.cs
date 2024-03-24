using WorkHours.ViewModels;

namespace WorkHours.Views;

public partial class MainApp : ContentPage
{
    private readonly MainPageViewModel _mainPageViewModel;

    public MainApp(MainPageViewModel mainPageViewModel)
    {
        _mainPageViewModel = mainPageViewModel;
        InitializeComponent();

        BindingContext = _mainPageViewModel;
    }
}