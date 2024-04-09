using WorkHours.ViewModels;

namespace WorkHours.Views;

public partial class MonthSummary : ContentPage
{
    public MonthSummary()
    {
        InitializeComponent();

        MonthSummaryViewModel viewModel = new MonthSummaryViewModel();

        BindingContext = viewModel;
    }
}