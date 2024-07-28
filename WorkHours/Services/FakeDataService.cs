using System.Collections.ObjectModel;
using WorkHours.Models;
using DateTime = System.DateTime;

namespace WorkHours.Services;

public class FakeDataService
{
    private ObservableCollection<WorkSession> _workSessions;

    public FakeDataService()
    {
        InitList();
    }

    private void InitList()
    {
        _workSessions =
        [
            new WorkSession("", "Rawa", "8", new DateTime(2024,4,19)),
            new WorkSession("", "Wałowice", "10", new DateTime(2024,4,20)),
            new WorkSession("", "Rawa", "4", new DateTime(2024,4,21)),
            new WorkSession("", "Rawa", "7", new DateTime(2024,4,22)),
            new WorkSession("", "Rawa", "10", new DateTime(2024,4,23)),
            new WorkSession("", "Rawa", "10", new DateTime(2024,4,24)),
            new WorkSession("", "Rawa", "11", new DateTime(2024,4,25)),
            new WorkSession("", "Rawa", "10", new DateTime(2024,4,26)),
            new WorkSession("", "Rawa", "10", new DateTime(2024,4,27))
        ];
    }
    
    public ObservableCollection<WorkSession> GetFakeData()
    {
        return _workSessions;
    }

    public void AddToFakeList(WorkSession workSession)
    {
        _workSessions.Add(workSession);
    }
}