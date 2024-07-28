using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public partial class DataViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<WorkSession> _collection;

    public DataViewModel()
    {
        var fakeDataService = new FakeDataService();
        _collection = fakeDataService.GetFakeData();
    }
}