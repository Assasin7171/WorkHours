using WorkHours.ViewModels;

namespace WorkHours.Views;

public partial class DataView : ContentPage
{
    private readonly DataViewViewModel _viewModel;

    public DataView(DataViewViewModel dataViewViewModel)
    {
        InitializeComponent();
        BindingContext = dataViewViewModel;
        _viewModel = dataViewViewModel;

        this.Appearing += OnAppearing;
    }

    private void OnAppearing(object? sender, EventArgs e)
    {
        _viewModel.StartRefreshingData();
    }
}