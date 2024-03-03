using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHours.ViewModels;

namespace WorkHours.Views;

public partial class MainApp : ContentPage
{
    private readonly MainPageViewModel _mainPageViewModel;

    public MainApp()
    {
        InitializeComponent();

        BindingContext = _mainPageViewModel;
        // _mainPageViewModel = _mainPageViewModel;
    }
}