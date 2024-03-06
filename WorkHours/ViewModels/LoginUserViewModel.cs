using System.Windows.Input;
using Microsoft.Maui.Controls;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public class LoginUserViewModel : ViewModelBase
{
    private readonly DBService _dbService;
    private string _email = string.Empty;
    private string _password = string.Empty;

    public LoginUserViewModel(DBService dbService)
    {
        _dbService = dbService;
    }

    public string Email
    {
        get => _email;
        set => SetField(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetField(ref _password, value);
    }

    public ICommand LoginUserCommand => new Command(LoginUser);
    public ICommand GoToRegisterViewCommand => new Command(GoToRegisterView);

    private async void GoToRegisterView()
    {
        await Shell.Current.GoToAsync("//registerView");
    }

    private async void LoginUser()
    {
        var result = await _dbService.CheckUserExistsAsync(new User() { Password = _password, Email = _email });
        
        if (result != null) await Shell.Current.GoToAsync("//MainApp");
        else await Application.Current.MainPage.DisplayAlert("Login", "Błędne hasło lub nazwa użytkownika", "OK");
    }
}