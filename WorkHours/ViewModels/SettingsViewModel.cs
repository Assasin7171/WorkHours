using System.Windows.Input;
using WorkHours.Services;
using WorkHours.Views;

namespace WorkHours.ViewModels
{
    public class SettingsViewModel
    {
        private readonly DBService _database;

        public SettingsViewModel(DBService database)
        {
            _database = database;
        }

        public ICommand OpenSettingsPage => new Command(OpenPage);

        private async void OpenPage()
        {
            await Shell.Current.GoToAsync("//EditLocationsPage");
        }
    }
}