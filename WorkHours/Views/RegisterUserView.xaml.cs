using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHours.ViewModels;

namespace WorkHours.Views;

public partial class RegisterUserView : ContentPage
{
    private readonly RegisterUserViewModel _registerUserViewModel;
    
    public RegisterUserView()
    {
        InitializeComponent();

        _registerUserViewModel = new RegisterUserViewModel();
        this.BindingContext = _registerUserViewModel;
    }
}