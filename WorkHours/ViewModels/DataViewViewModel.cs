using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public partial class DataViewViewModel : ObservableObject
{
    //services
    private readonly DBService _dbService;

    //models
    private readonly WorkSession _sessionModel;
    [ObservableProperty] private int _allWorkingHoursInMonth;

    [ObservableProperty] private List<ChartData> _chartsData = new();

    //others
    private double _countedDays;
    private double _countedFreeDays;
    [ObservableProperty] private string _description;
    [ObservableProperty] private bool _isRefreshing;
    [ObservableProperty] private string _location;
    [ObservableProperty] private List<WorkSession> _sessionsList = new();
    [ObservableProperty] private Tuple<int, int> _workingDaysInMonth;
    [ObservableProperty] private WorkSession _workSession;
    [ObservableProperty] private string _workTime;

    public DataViewViewModel(DBService dbService)
    {
        _dbService = dbService;
        _sessionModel = new WorkSession();

        // InitDataInCharts();
    }

    [RelayCommand]
    private async Task RefreshView()
    {
        try
        {
            IsRefreshing = true;
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
        var freeDays = new List<int>();

        for (var i = 1; i < daysInMonth + 1; i++)
        {
            var date = new DateTime(year, month, i);
            var dayOfWeek = date.DayOfWeek;

            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                freeDays.Add(i);
            else
                workingDays.Add(i);
        }

        return new Tuple<int, int>(workingDays.Count, freeDays.Count);
    }

    /// <summary>
    ///     This function counts days from hours, 10 hours = 1 day.
    /// </summary>
    /// <returns>Days worked</returns>
    private int CountDaysFromHours(List<WorkSession> workSessions)
    {
        var countedHours = 0;
        if (workSessions.Count > 0)
        {
            foreach (var session in workSessions)
                if (int.TryParse(session.WorkTime, out var result))
                    countedHours += result;

            return countedHours / 10;
        }

        return default;
    }

    [RelayCommand]
    private async Task Appearing()
    {
        try
        {
            var list = await _dbService.GetWorkSessionsListAsync();
            await UpdateCollectionAsync(SessionsList, list);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task UpdateCollectionAsync<T>(List<T> collectionToUpdate, List<T> collectionList)
    {
        // var newItems = await _dbService.GetWorkSessionsListAsync();
        var tasks = new List<Task>();

        if (collectionToUpdate.Count < collectionList.Count)
        {
            collectionToUpdate.Clear();

            foreach (var item in collectionList)
                tasks.Add(Task.Run(() => { collectionToUpdate.Add(item); }));

            await Task.WhenAll(tasks);
        }
    }

    private void InitDataInCharts()
    {
        _countedDays = CountDaysFromHours(SessionsList);
        _countedFreeDays = HowManyDaysInMonth(DateTime.Now.Day, DateTime.Now.Month).Item1 - _countedDays;

        ChartsData.Add(new ChartData("Przepracowane dni (1 dzień = 10H)", _countedDays));
        ChartsData.Add(new ChartData("Wolne dni", _countedFreeDays));
    }
}