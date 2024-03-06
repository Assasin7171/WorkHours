using System.Windows.Input;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public class RegisterUserViewModel : ViewModelBase
{
    private readonly DBService _dbService;
    private string _name = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;

    public RegisterUserViewModel(DBService dbService)
    {
        _dbService = dbService;
    }
    public string Name
    {
        get => _name;
        set => SetField(ref _name, value);
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

    public ICommand GoToLoginViewCommand => new Command(GoToLoginView);
    public ICommand RegisterUserCommand => new Command(RegisterUser);

    private async void GoToLoginView()
    {
        await Shell.Current.GoToAsync("//loginView");
    }

    private async void RegisterUser()
    {
        var user = new User()
        {
            Email = _email,
            Name = _name,
            Password = _password
        };
        _dbService.CreateUserAsync(user);
    }
}