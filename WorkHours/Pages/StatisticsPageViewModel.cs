using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WorkHours.Entities;

namespace WorkHours.Pages;

public partial class StatisticsPageViewModel : ObservableObject
{
    private readonly WorkHoursDbContext _dbContext;

    [ObservableProperty] private List<WorkSession> _workSessions;
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private WorkSession _selectedWorkSession;

    public StatisticsPageViewModel(WorkHoursDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [RelayCommand]
    private async Task SelectedWorkSessionAsync()
    {
        await Shell.Current.GoToAsync(nameof(WorkSessionDetailsPage), new Dictionary<string, object>()
        {
            { "WorkSession", SelectedWorkSession },
        });
    }

    [RelayCommand]
    private async Task GetWorkSessionsFromDbAsync()
    {
        try
        {
            IsLoading = true;
            await Task.Delay(1000);

            WorkSessions = new List<WorkSession>(await _dbContext.WorkSessions.ToListAsync());
        }
        finally
        {
            IsLoading = false;
        }
    }
}