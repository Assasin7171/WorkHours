using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WorkHours.Services;
using WorkHoursDb;
using WorkHoursDb.Entities;

namespace WorkHours.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly DataStoreService _dataStoreService;
    [ObservableProperty] private DateTime _date = DateTime.Now;
    [ObservableProperty] private DateTime _minDate = DateTime.Now - TimeSpan.FromDays(7);
    [ObservableProperty] private DateTime _maxDate = DateTime.Now;
    [ObservableProperty] private string _workHours;
    [ObservableProperty] private Place _selectedPlace;
    [ObservableProperty] private string _arrowImage = "arrow_down.png";

    private bool _changeImage = true;
    public ObservableCollection<Place> Places { get; set; } = new ObservableCollection<Place>();
    public ObservableCollection<Worksession> Worksessions { get; set; } = new ObservableCollection<Worksession>();

    public MainViewModel(DataStoreService dataStoreService)
    {
        _dataStoreService = dataStoreService;
        LoadData();
        _dataStoreService.PlacesChanged += (sender, args) =>
        {
            LoadData();
        };
        _dataStoreService.WorksessionsChanged += (sender, args) =>
        {
            LoadData();
        };
    }

    private void LoadData()
    {
        var places = _dataStoreService.Places;
        if (places.Count > 0)
        {
            Places.Clear();

            foreach (Place place in places)
            {
                Places.Add(place);
            }
        }


        var sessions = _dataStoreService.Worksessions
            .OrderByDescending(s => s.CreatedTime)
            .Take(5)
            .ToList();

        if (sessions.Count > 0)
        {
            Worksessions.Clear();

            foreach (var session in sessions)
            {
                Worksessions.Add(session);
            }
        }
    }

    [RelayCommand]
    private void AddSessionToDatabase()
    {
        if (int.TryParse(WorkHours, out int hoursCount))
        {
            Worksession worksession = new Worksession()
            {
                HoursWorked = hoursCount,
                Place = SelectedPlace,
                CreatedTime = DateTime.Now,
                Id = Guid.NewGuid(),
            };
            
            _dataStoreService.AddWorksession(worksession);

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