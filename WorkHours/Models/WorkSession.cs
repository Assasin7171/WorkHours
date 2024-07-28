using CommunityToolkit.Mvvm.ComponentModel;

namespace WorkHours.Models;

public partial class WorkSession(string description, string location, string workTime, DateTime date) : ObservableObject
{
    [ObservableProperty]private string _description = description;
    [ObservableProperty]private string _location = location;
    [ObservableProperty]private string _workTime = workTime;
    [ObservableProperty] private DateTime _dateTime = date;
}