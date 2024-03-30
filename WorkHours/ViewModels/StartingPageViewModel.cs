using System.Diagnostics;
using System.Windows.Input;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public class StartingPageViewModel : ViewModelBase
{
    private readonly UserService _userService;
    private bool _checkingUserInBase;
    private string _name;

    public StartingPageViewModel(UserService userService)
    {
        _userService = userService;
    }

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
                SetField(ref _name, value);
        }
    }

    public ICommand CreateUserCommand => new Command(LoginOrCreateUser);

    private async void LoginOrCreateUser()
    {
        // await Shell.Current.DisplayAlert("Błąd", $"test", "OK");
        try
        {
            if (string.IsNullOrWhiteSpace(_name))
                return;

            _checkingUserInBase = true;
            var user = new User
            {
                Name = _name,
                IsLogged = true
            };
            var isUserInDb = await _userService.CheckUserExistsAsync(user);
            if (!isUserInDb)
                _userService.CreateUserAsync(user);

            _userService.User = user;
            _userService.User.IsLogged = true;
            if (_userService.User.IsLogged) await Shell.Current.GoToAsync("//MainApp");
        }

        catch (Exception e)
        {
            // await Shell.Current.DisplayAlert("Błąd", $"Wystąpił błąd {e.Message}", "OK");
            Debug.WriteLine($"Wystąpił błąd {e.Message}");
        }

        finally
        {
            _checkingUserInBase = false;
        }
    }
}