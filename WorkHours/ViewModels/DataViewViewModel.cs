using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SkiaSharp;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public partial class DataViewViewModel : ObservableObject
{
    private readonly DBService _dbService;
    private readonly WorkSession _sessionModel;
    [ObservableProperty] private int _countedHours;
    [ObservableProperty] private bool _isRefreshing;
    [ObservableProperty] private ObservableCollection<WorkSession> _sessionsList = [];
    [ObservableProperty] private Tuple<int, int> _workingDaysInMonth;

    [ObservableProperty]
    private ChartEntry[] _entries =
    [
        new ChartEntry(212)
        {
            Label = "UWP",
            ValueLabel = "112",
            Color = SKColor.Parse("#2c3e50")
        },
        new ChartEntry(248)
        {
            Label = "Android",
            ValueLabel = "648",
            Color = SKColor.Parse("#77d065")
        },
        new ChartEntry(128)
        {
            Label = "iOS",
            ValueLabel = "428",
            Color = SKColor.Parse("#b455b6")
        },
        new ChartEntry(514)
        {
            Label = "Forms",
            ValueLabel = "214",
            Color = SKColor.Parse("#3498db")
        }
    ];
        
    

    public DataViewViewModel(DBService dbService)
    {
        _dbService = dbService;
        _sessionModel = new WorkSession();

        WorkingDaysInMonth = HowManyDaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        CountedHours = CountHours(SessionsList);
    }

    public string WorkTime
    {
        get => _sessionModel.WorkTime;
        set => _sessionModel.WorkTime = value;
    }

    public string Location
    {
        get => _sessionModel.Location;
        set => _sessionModel.Location = value;
    }

    public string Description
    {
        get => _sessionModel.Description;
        set => _sessionModel.Description = value;
    }


    [RelayCommand]
    private async Task RefreshView()
    {
        try
        {
            IsRefreshing = true;
            WorkingDaysInMonth = HowManyDaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            CountedHours = CountHours(SessionsList);
        }
        catch (Exception e)
        {
            await Shell.Current.DisplayAlert("Error", $"Fatal error {e.Message}", "OK");
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    /// <returns>
    ///     First return - working days in month
    ///     Second return - free days in month like (Saturday/Sunday)
    /// </returns>
    private Tuple<int, int> HowManyDaysInMonth(int year, int month)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var workingDays = new List<int>();
        var otherDays = new List<int>();

        for (var i = 1; i < daysInMonth + 1; i++)
        {
            var date = new DateTime(year, month, i);
            var dayOfWeek = date.DayOfWeek;

            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                otherDays.Add(i);
            else
                workingDays.Add(i);
        }

        return new Tuple<int, int>(workingDays.Count, otherDays.Count);
    }

    private int CountHours(ObservableCollection<WorkSession> workSessions)
    {
        var countedHours = 0;
        if (workSessions.Count > 0)
        {
            foreach (var session in workSessions)
                if (int.TryParse(session.WorkTime, out var result))
                    countedHours += result;

            return countedHours;
        }

        return default;
    }

    public void StartRefreshingData()
    {
        UpdateCollection(SessionsList);
    }

    private async void UpdateCollection(ObservableCollection<WorkSession> collection)
    {
        var newItems = await _dbService.GetWorkSessionsListAsync();
        collection.Clear();
        foreach (var item in newItems) collection.Add(item);
    }
}