using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WorkHours.Database.Entities;
using WorkHours.Services;
using Place = WorkHours.Database.Entities.Place;

namespace WorkHours.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly DataStoreService _dataStoreService;
    private bool _changeImage = true;

    [ObservableProperty] private decimal _workRateValue;
    [ObservableProperty] private string _newLocation = string.Empty;
    [ObservableProperty] private string _newLocationDescription = string.Empty;
    [ObservableProperty] private string _arrowImage = "arrow_down.png";

    public ObservableCollection<Place> Places { get; set; } = new ObservableCollection<Place>();

    public SettingsViewModel(DataStoreService dataStoreService)
    {
        _dataStoreService = dataStoreService;
        // inicjalizacja danych o lokalizacjach
        InitializeData();
    }

    private void InitializeData()
    {
        Places.Clear();
        var data = _dataStoreService.Places.OrderByDescending(x => x.Id).ToList();
        foreach (var item in data)
        {
            Places.Add(item);
        }

        var rate = _dataStoreService.WorkRate.LastOrDefault();
        WorkRateValue = rate?.ValueRate ?? 0;
    }

    [RelayCommand]
    private async Task SetWorkRate()
    {
        WorkRate workRate = new WorkRate() { ValueRate = WorkRateValue, CreatedTime = DateTime.Now };
        await _dataStoreService.AddWorkRate(workRate);

        await Shell.Current.DisplayAlert("Info", "Ustawiono stawke godzinową", "OK");
    }

    [RelayCommand]
    private async Task AddNewPlaceToDb()
    {
        var newPlace = new Place()
        {
            Name = NewLocation,
            Description = NewLocationDescription
        };

        await _dataStoreService.AddPlace(newPlace);
        InitializeData();
        //czyszczenie pól po dodaniu i informacja zwrotna do użytkownika.
        NewLocation = string.Empty;
        NewLocationDescription = string.Empty;
        await Shell.Current.DisplayAlert("Information", "New place added", "OK");
    }

    [RelayCommand]
    private void ChangeArrowImage()
    {
        if (_changeImage)
        {
            ArrowImage = "arrow_up.png";
            _changeImage = false;
        }
        else
        {
            ArrowImage = "arrow_down.png";
            _changeImage = true;
        }
    }
}