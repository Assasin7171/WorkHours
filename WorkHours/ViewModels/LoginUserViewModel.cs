// using System.Windows.Input;
// using WorkHours.Models;
// using WorkHours.Services;
//
// namespace WorkHours.ViewModels;
//
// public class LoginUserViewModel(DBService dbService, LoggedUserService loggedUserService) : ViewModelBase
// {
//     private string _email = string.Empty;
//     private string _password = string.Empty;
//     private User _user;
//
//     public string Email
//     {
//         get => _email;
//         set => SetField(ref _email, value);
//     }
//
//     public string Password
//     {
//         get => _password;
//         set => SetField(ref _password, value);
//     }
//
//     public ICommand LoginUserCommand => new Command(LoginUser);
//     public ICommand GoToRegisterViewCommand => new Command(GoToRegisterView);
//
//     private async void GoToRegisterView()
//     {
//         await Shell.Current.GoToAsync("//registerView");
//     }
//
//     private async void LoginUser()
//     {
//         _user = new User() { Password = _password, Email = _email };
//         if (string.IsNullOrWhiteSpace(_password) && string.IsNullOrWhiteSpace(_email))
//         {
//             await Shell.Current.DisplayAlert("Login", "Nie wpisano hasła lub nazwy użytkownika", "OK");
//         }
//
//         else
//         {
//             var result = await dbService.CheckUserExistsAsync(_user);
//
//             if (result != null)
//             {
//                 loggedUserService.User = _user;
//                 await Shell.Current.GoToAsync("//MainApp");
//             }
//             else
//             {
//                 await Shell.Current.DisplayAlert("Login", "Błędne hasło lub nazwa użytkownika", "OK");
//             }
//         }
//     }
// }