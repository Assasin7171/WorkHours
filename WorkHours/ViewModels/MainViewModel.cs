using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WorkHours.Services;
using Place = WorkHours.Database.Entities.Place;
using Worksession = WorkHours.Database.Entities.Worksession;

namespace WorkHours.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly DataStoreService _dataStoreService;
    private bool _changeImage = true;

    [ObservableProperty] private DateTime _date = DateTime.Now;
    [ObservableProperty] private DateTime _minDate = DateTime.Now - TimeSpan.FromDays(7);
    [ObservableProperty] private DateTime _maxDate = DateTime.Now;
    [ObservableProperty] private string _workHours;
    [ObservableProperty] private Place _selectedPlace;
    [ObservableProperty] private string _arrowImage = "arrow_down.png";

    public ObservableCollection<Place> Places { get; set; } = new ObservableCollection<Place>();
    public ObservableCollection<Worksession> Worksessions { get; set; } = new ObservableCollection<Worksession>();

    public MainViewModel(DataStoreService dataStoreService)
    {
        _dataStoreService = dataStoreService;
    }


    [RelayCommand]
    private async Task LoadDataFromDatabase()
    {
        await _dataStoreService.GetDataAsync();

        RefreshCollection(Places, _dataStoreService.Places);
        RefreshCollection(Worksessions, _dataStoreService.Worksessions);
    }

    private void RefreshCollection<T>(ObservableCollection<T> target, IEnumerable<T> source)
    {
        target.Clear();
        foreach (var item in source)
        {
            target.Add(item);
        }
    }

    [RelayCommand]
    private async Task AddSessionToDatabase()
    {
        if (int.TryParse(WorkHours, out int hoursCount))
        {
            Worksession worksession = new Worksession()
            {
                HoursWorked = hoursCount,
                PlaceId = SelectedPlace.Id,
                CreatedTime = DateTime.Now,
            };

            await _dataStoreService.AddWorksession(worksession);
            await LoadDataFromDatabase();
            //resetuje pola
            WorkHours = string.Empty;
            SelectedPlace = null;
            Date = DateTime.Now;
        }
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