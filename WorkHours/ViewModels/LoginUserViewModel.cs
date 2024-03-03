using System.Windows.Input;

namespace WorkHours.ViewModels;

public class LoginUserViewModel
{
    public ICommand SignUpCommand => new Command(SignUp);
    
    private async void SignUp()
    {
        await Shell.Current.GoToAsync("//registerView");
    }
}