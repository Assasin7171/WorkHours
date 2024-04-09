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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _mainPageViewModel.GetListWorkplaces();
    }

    // private void Picker_OnSelectedIndexChanged(object? sender, EventArgs e)
    // {
    //     var selectedItem = ((Picker)sender).SelectedItem;
    // }
}