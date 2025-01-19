using CommunityToolkit.Mvvm.ComponentModel;
using WorkHours.Entities;

namespace WorkHours.Pages;

[QueryProperty(nameof(WorkSession), "WorkSession")]
public partial class WorkSessionDetailsViewModel : ObservableObject
{
    [ObservableProperty]
    private WorkSession _workSession;

    public WorkSessionDetailsViewModel()
    {
        
    }
}