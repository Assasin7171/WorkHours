using System;
using Microsoft.Maui.Controls;
using WorkHours.ViewModels;

namespace WorkHours.Views;

public partial class DataView : ContentPage
{
    private readonly DataViewViewModel _viewModel;

    public DataView(DataViewViewModel dataViewViewModel)
    {
        InitializeComponent();
        BindingContext = dataViewViewModel;
    }
}