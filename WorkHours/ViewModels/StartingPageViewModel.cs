using System.Diagnostics;
using System.Windows.Input;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public class StartingPageViewModel : ViewModelBase
{
    private readonly AuthUserService _authUserService;
    private string _name;

    public StartingPageViewModel(AuthUserService authUserService)
    {
        _authUserService = authUserService;
    }

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                SetField(ref _name, value);
                _authUserService.Name = _name;
            }
        }
    }

    public ICommand CreateUserCommand => new Command(CreateUser);

    private async void CreateUser()
    {
        // await Shell.Current.DisplayAlert("Błąd", $"test", "OK");
        try
        {
            if (string.IsNullOrWhiteSpace(_name))
                return;

            var user = new User
            {
                Name = _name
            };

            _authUserService.User = user;
            _authUserService.IsUserLogged = true;

            //login and redirect to main app
            _authUserService.Login();
        }

        catch (Exception e)
        {
            // await Shell.Current.DisplayAlert("Błąd", $"Wystąpił błąd {e.Message}", "OK");
            Debug.WriteLine($"Wystąpił błąd {e.Message}");
        }
    }
}