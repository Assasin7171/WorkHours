using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkHours.Pages;

public partial class WorkSessionDetailsPage : ContentPage
{
    private readonly WorkSessionDetailsViewModel _viewModel;

    public WorkSessionDetailsPage(WorkSessionDetailsViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = _viewModel;
        
        InitializeComponent();
    }
}