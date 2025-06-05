using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WorkHours.Services;
using WorkHoursDb.Entities;

namespace WorkHours.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly DataStoreService _dataStoreService;
    private bool _changeImage = true;
    
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
        var data = _dataStoreService.Places;
        foreach (var item in data)
        {
            Places.Add(item);
        }
    }

    [RelayCommand]
    private void AddNewPlaceToDb()
    {
        var newPlace = new Place()
        {
            Id = Guid.NewGuid(),
            Name = NewLocation,
            Description = NewLocationDescription
        };

        _dataStoreService.PlacesChanged += (sender, args) =>
        {
            Application.Current?.MainPage?.DisplayAlert("Informacja", "Dodano nową lokalizację do bazy", "OK");
            NewLocation = string.Empty;
            NewLocationDescription = string.Empty;
            
            InitializeData();
        };
        _dataStoreService.AddPlace(newPlace);
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