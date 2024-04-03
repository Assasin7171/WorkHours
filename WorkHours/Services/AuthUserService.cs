using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using WorkHours.Models;
using WorkHours.Views;

namespace WorkHours.Services;

public class AuthUserService
{
    private const string AuthUserStateKey = "StateKey";
    private const string AuthUserNameKey = "NameKey";
    private readonly DBService _dbService;
    public bool IsUserLogged = false;

    public AuthUserService(DBService dbService)
    {
        _dbService = dbService;
    }

    public User User { get; set; }
    public string Name { get; set; }

    public bool CheckUserAuth()
    {
        var name = string.Empty;
        var isUserLogged = Preferences.Default.Get(AuthUserStateKey, false);
        if (isUserLogged) Name = Preferences.Default.Get(AuthUserNameKey, string.Empty);

        return isUserLogged;
    }

    public async void Login()
    {
        Preferences.Set(AuthUserStateKey, IsUserLogged);
        Preferences.Set(AuthUserNameKey, User.Name);

        await Shell.Current.GoToAsync("//MainApp");
    }

    public async void Logout()
    {
        Preferences.Remove(AuthUserStateKey);
        Preferences.Remove(AuthUserNameKey);

        await Shell.Current.GoToAsync($"//{nameof(LoadingView)}");
    }
}