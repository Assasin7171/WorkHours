using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WorkHours.Models;
using WorkHours.Services;
using WorkHours.Views;

namespace WorkHours.ViewModels;

public partial class DataViewViewModel : ObservableObject
{
    //services
    private readonly DBService _dbService;

    //models
    private readonly WorkSession _sessionModel;


    //others
    [ObservableProperty] private string _description;
    [ObservableProperty] private bool _isRefreshing;
    [ObservableProperty] private string _location;
    [ObservableProperty] private ObservableCollection<WorkSession> _sessionsList = new();
    [ObservableProperty] private WorkSession _workSession;
    [ObservableProperty] private string _workTime;
    [ObservableProperty] private DateTime _dateOfWorkTime;

    public DataViewViewModel(DBService dbService)
    {
        _dbService = dbService;
        _sessionModel = new WorkSession();

        LoadWorkSessions();
    }

    [RelayCommand]
    private async Task RefreshView()
    {
        try
        {
            IsRefreshing = true;
            LoadWorkSessions();
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

    [RelayCommand]
    private void Appearing()
    {
        try
        {
            LoadWorkSessions();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async void LoadWorkSessions()
    {
        var collectionList = await _dbService.GetWorkSessionsListAsync();

        if (SessionsList.Count < collectionList.Count)
        {
            SessionsList.Clear();

            foreach (var item in collectionList)
                SessionsList.Add(item);
        }
    }

    /// <summary>
    ///     This function counts days from hours, 10 hours = 1 day.
    /// </summary>
    /// <returns>Days worked</returns>
    private int CountDaysFromHours(ObservableCollection<WorkSession> workSessions)
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
    private async void DeleteElementAsync(object o)
    {
        await _dbService.DeleteWorkSession(o as WorkSession, _sessionsList);
    }

    [RelayCommand]
    private async Task GoToMonthSummaryViewAsync()
    {
        var countedDays = CountDaysFromHours(SessionsList);

        await Shell.Current.GoToAsync($"{nameof(MonthSummary)}?countedDays={countedDays}");
    }
}