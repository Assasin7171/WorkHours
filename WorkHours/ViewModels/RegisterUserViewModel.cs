using System.Windows.Input;

namespace WorkHours.ViewModels;

public class RegisterUserViewModel
{
    public ICommand GoToLoginViewCommand => new Command(GoToLoginView);
    private async void GoToLoginView()
    {
        await Shell.Current.GoToAsync("//loginView");
    }
}