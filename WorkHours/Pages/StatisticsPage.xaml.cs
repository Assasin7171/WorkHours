using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkHours.Pages;

public partial class StatisticsPage : ContentPage
{
    private readonly StatisticsPageViewModel _viewModel;

    public StatisticsPage(StatisticsPageViewModel viewModel)
    {
        _viewModel = viewModel;
        
        BindingContext = _viewModel;
        InitializeComponent();
    }
}