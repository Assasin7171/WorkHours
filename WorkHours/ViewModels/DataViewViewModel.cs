using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public partial class DataViewViewModel : ObservableObject
{
    private readonly DBService _dbService;
    private readonly WorkSession _sessionModel;
    [ObservableProperty]
    private ObservableCollection<WorkSession> _sessionsList = new();
    private bool isActive;

    public DataViewViewModel(DBService dbService)
    {
        _dbService = dbService;
        _sessionModel = new WorkSession();
        //CountHours(_sessionsList);
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


    //public ObservableCollection<WorkSession> SessionsList
    //{
    //    get
    //    {
    //        UpdateCollection(_sessionsList);

    //        return _sessionsList;
    //    }
    //}

    public void StartRefreshingData()
    {
        UpdateCollection(SessionsList);
        CountHours(SessionsList);
    }

    private async void UpdateCollection(ObservableCollection<WorkSession> collection)
    {
        var newItems = await _dbService.GetWorkSessionsListAsync();
        collection.Clear();
        foreach (var item in newItems) collection.Add(item);
    }

    private int CountHours(ObservableCollection<WorkSession> workSessions)
    {
        int countedHours = 0;
        if (workSessions.Count > 0)
        {
            foreach (var session in workSessions)
            {
                if (int.TryParse(session.WorkTime, out int result))
                {
                    countedHours += result;
                }

                return default;
            }
        }

        return countedHours;
    }
}