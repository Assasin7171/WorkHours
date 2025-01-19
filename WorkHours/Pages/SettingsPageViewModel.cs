using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WorkHours.Entities;

namespace WorkHours.Pages;

public partial class SettingsPageViewModel : ObservableObject
{
    [ObservableProperty] private string _name;
    [ObservableProperty] private List<Place> _places;
    [ObservableProperty] private Place _selectedPlace;

    private readonly WorkHoursDbContext _dbContext;

    public SettingsPageViewModel(WorkHoursDbContext dbContext)
    {
        _dbContext = dbContext;

        Places = _dbContext.Places.ToList();
    }

    [RelayCommand]
    private async Task AddPlaceToDb()
    {
        try
        {
            var newPlace = new Place() { Name = Name };
            await _dbContext.Places.AddAsync(newPlace);
            Name = string.Empty;

            await _dbContext.SaveChangesAsync();
            Places = new List<Place>(_dbContext.Places.ToList());

            await App.Current.MainPage.DisplayAlert($"Add New Place {newPlace.Name}", "Place was successfully added.",
                "OK");
        }
        catch (Exception e)
        {
            await App.Current.MainPage.DisplayAlert("Error", e.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task EditPlace()
    {
        _dbContext.Places.Update(SelectedPlace);
        await _dbContext.SaveChangesAsync();
        
        
        //do poprawy
        Places = _dbContext.Places.ToList();
    }
}