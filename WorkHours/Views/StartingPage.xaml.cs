using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHours.ViewModels;

namespace WorkHours.Views;

public partial class StartingPage : ContentPage
{
    private readonly StartingPageViewModel _startingPageViewModel;

    public StartingPage(StartingPageViewModel startingPageViewModel)
    {
        _startingPageViewModel = startingPageViewModel;
        InitializeComponent();

        BindingContext = _startingPageViewModel;
    }
}