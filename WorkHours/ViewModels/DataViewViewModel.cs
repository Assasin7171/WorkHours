using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    [ObservableProperty] private List<ChartsData> _chartsData = new List<ChartsData>();


    public DataViewViewModel(DBService dbService)
    {
        _dbService = dbService;
        _sessionModel = new WorkSession();

        WorkingDaysInMonth = HowManyDaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
        CountedHours = CountHours(SessionsList);

        StartRefreshingData();
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

    public async void StartRefreshingData()
    {
        await UpdateCollection(SessionsList);
        UpdateChartData(ChartsData);
    }

    private async Task UpdateCollection(ObservableCollection<WorkSession> collection)
    {
        var newItems = await _dbService.GetWorkSessionsListAsync();
        collection.Clear();
        foreach (var item in newItems) collection.Add(item);
    }
    private void UpdateChartData(List<ChartsData> chartsDatas)
    {
        chartsDatas.Add(new ChartsData("Przepracowane godziny", CountedHours));
    }
}