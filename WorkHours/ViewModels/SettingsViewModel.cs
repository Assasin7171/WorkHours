using System.Windows.Input;
using Microsoft.Maui.Controls;
using WorkHours.Services;

namespace WorkHours.ViewModels
{
    public class SettingsViewModel(DBService database)
    {
        private readonly DBService _database = database;

        public ICommand OpenSettingsPage => new Command(OpenPage);

        private async void OpenPage()
        {
            await Shell.Current.GoToAsync("EditLocationsPage");
        }
    }
}