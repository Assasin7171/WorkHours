using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHours.ViewModels;

namespace WorkHours.Views;

public partial class LoginView : ContentPage
{
    private readonly LoginUserViewModel _loginUserViewModel;

    public LoginView(LoginUserViewModel loginUserViewModel)
    {
        _loginUserViewModel = loginUserViewModel;
        InitializeComponent();


        this.BindingContext = _loginUserViewModel;
    }
}