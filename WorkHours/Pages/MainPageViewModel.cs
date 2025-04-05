using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WorkHours.Entities;
using WorkHours.Models;

namespace WorkHours.Pages;

public partial class MainPageViewModel : ObservableObject
{
    private readonly WorkHoursDbContext _dbContext;

    [ObservableProperty] private List<Place> _places;
    [ObservableProperty] private DateTime _selectedDate;
    [ObservableProperty] private WorkSession _workSession = new WorkSession();
    [ObservableProperty] private DateTime _minDate;
    [ObservableProperty] private DateTime _maxDate;

    public MainPageViewModel(WorkHoursDbContext dbContext)
    {
        _dbContext = dbContext;
        UpdatePlaces();

        MinDate = DateTime.Now - TimeSpan.FromDays(7);
        MaxDate = DateTime.Now;
        SelectedDate = DateTime.Now;
    }

    [RelayCommand]
    private async Task AddWorkSessionToDbAsync()
    {
        if (WorkSession.Hours != 0 && WorkSession.Place != null && WorkSession.DateTime != null)
        {
            WorkSession.DateTime = SelectedDate;
            await _dbContext.WorkSessions.AddAsync(WorkSession);
            await _dbContext.SaveChangesAsync();

            WorkSession = new WorkSession();
            
            await App.Current.MainPage.DisplayAlert("Info", "Dodano nową sesje pracy", "OK");

        }
        else
        {
            await App.Current.MainPage.DisplayAlert("Error", "Brak danych do zapisania", "OK");
        }
    }

    public void UpdatePlaces()
    {
        Places = [.._dbContext.Places.ToList()];
    }
}