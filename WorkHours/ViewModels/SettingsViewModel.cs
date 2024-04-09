using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WorkHours.Services;

namespace WorkHours.ViewModels
{
    public partial class SettingsViewModel(DBService database) : ObservableObject
    {
        private readonly DBService _database = database;


        [RelayCommand]
        private async Task OpenEditLocationsPage()
        {
            await Shell.Current.GoToAsync("EditLocationsPage");
        }
    }
}