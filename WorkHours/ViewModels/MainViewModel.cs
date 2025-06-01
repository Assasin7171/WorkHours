using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using WorkHours.Entities;

namespace WorkHours.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public WorkHoursContext DbContext { get; }

    [ObservableProperty] private DateTime _date = DateTime.Now;
    [ObservableProperty] private DateTime _minDate = DateTime.Now - TimeSpan.FromDays(7);
    [ObservableProperty] private DateTime _maxDate = DateTime.Now;
    [ObservableProperty] private int _workHours;
    [ObservableProperty] private Place _selectedPlace;
    [ObservableProperty] private string _arrowImage = "arrow_down.png";

    private bool _changeImage = true;
    public ObservableCollection<Place> Places { get; set; } = new ObservableCollection<Place>();
    public ObservableCollection<Worksession> Worksessions { get; set; } = new ObservableCollection<Worksession>();

    public MainViewModel(WorkHoursContext dbContext)
    {
        DbContext = dbContext;
        LoadSessionsFromDatabase();
    }

    private void LoadSessionsFromDatabase()
    {
        var sessions = DbContext.Worksessions
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
        Worksession worksession = new Worksession()
        {
            HoursWorked = WorkHours,
            Place = SelectedPlace,
            Id = Guid.NewGuid(),
        };

        DbContext.Worksessions.Add(worksession);
        DbContext.SaveChanges();
        
        Worksessions.Add(worksession);
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