using System.Windows.Input;
using WorkHours.DataServices;
using WorkHours.Views;

namespace WorkHours.ViewModels
{
    public class SettingsViewModel
    {
        private readonly DataBaseService _database;

        public SettingsViewModel(DataBaseService database)
        {
            _database = database;
        }

        public ICommand OpenSettingsPage => new Command(OpenPage);

        private async void OpenPage()
        {
            //await Shell.Current.GoToAsync();
        }
    }
}
