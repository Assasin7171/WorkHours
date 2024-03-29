using System.Collections.ObjectModel;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public class DataViewViewModel : ViewModelBase
{
    private readonly DBService _dbService;
    private readonly WorkSession _sessionModel;
    private readonly ObservableCollection<WorkSession> _sessionsList = new();
    private bool isActive;

    public DataViewViewModel(DBService dbService)
    {
        _dbService = dbService;
        _sessionModel = new WorkSession();
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

    public ObservableCollection<WorkSession> SessionsList
    {
        get
        {
            UpdateCollection(_sessionsList);

            return _sessionsList;
        }
    }

    public async void StartRefreshingData()
    {
        UpdateCollection(_sessionsList);
    }

    private async void UpdateCollection(ObservableCollection<WorkSession> collection)
    {
        var newItems = await _dbService.GetWorkSessionsListAsync();
        collection.Clear();
        foreach (var item in newItems)
        {
            collection.Add(item);
        }
    }
}