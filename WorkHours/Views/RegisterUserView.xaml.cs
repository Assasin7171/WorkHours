using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkHours.Services;
using WorkHours.ViewModels;

namespace WorkHours.Views;

public partial class RegisterUserView : ContentPage
{
    private readonly RegisterUserViewModel _userViewModel;
    
    public RegisterUserView(RegisterUserViewModel userViewModel)
    {
        _userViewModel = userViewModel;
        InitializeComponent();

        this.BindingContext = _userViewModel;
    }
}