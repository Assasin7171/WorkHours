using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WorkHours.Entities;

namespace WorkHours.Pages;

public partial class SettingsPageViewModel : ObservableObject
{
    [ObservableProperty] private string _name;

    private readonly WorkHoursDbContext _dbContext;

    public SettingsPageViewModel(WorkHoursDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [RelayCommand]
    private async Task AddPlaceToDb()
    {
        try
        {
            var newPlace = new Place() { Name = Name };
            await _dbContext.Places.AddAsync(newPlace);

            
            await _dbContext.SaveChangesAsync();

            await App.Current.MainPage.DisplayAlert($"Add New Place {newPlace.Name}", "Place was successfully added.", "OK");
        }
        catch (Exception e)
        {
            await App.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
        }
    }
}