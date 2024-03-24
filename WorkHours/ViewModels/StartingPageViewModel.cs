using System.Diagnostics;
using System.Windows.Input;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public class StartingPageViewModel : ViewModelBase
{
    private readonly UserService _userService;
    private bool _checkingUserInBase = false;
    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
                SetField(ref _name, value);
        }
    }

    public StartingPageViewModel(UserService userService)
    {
        _userService = userService;
    }

    public ICommand CreateUserCommand => new Command(CreateUser);

    private async void CreateUser()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(_name))
            {
                _checkingUserInBase = true;
                var user = new User
                {
                    Name = _name
                };
                _userService.CreateUserAsync(user);

                await Shell.Current.GoToAsync("//MainApp");
                _userService.User = user;
                _userService.IsUserLogged = true;
            }
            else
                Shell.Current.DisplayAlert("error", "test", "OK");
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