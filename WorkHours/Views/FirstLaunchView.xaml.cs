using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHours.ViewModels;

namespace WorkHours.Views;

public partial class FirstLaunchView : ContentPage
{
    public FirstLaunchView()
    {
        InitializeComponent();
        BindingContext = new FirstLaunchViewModel();
    }
}