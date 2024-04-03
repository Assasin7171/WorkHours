using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using WorkHours.Models;
using WorkHours.Services;
using Timer = System.Timers.Timer;

namespace WorkHours.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly AuthUserService _authUserService;
    private readonly DBService _dbServiceService;

    [ObservableProperty]
    private string _currentTime = string.Empty;
    [ObservableProperty]
    private string _description = string.Empty;
    [ObservableProperty]
    private Workplace _location;
    [ObservableProperty]
    private DateTime _workDate = DateTime.Now;
    [ObservableProperty]
    private List<Workplace> _workplaces = new();
    [ObservableProperty]
    private string _workTime = string.Empty;

    public MainPageViewModel(DBService dbService, AuthUserService authUserService)
    {
        _dbServiceService = dbService;
        _authUserService = authUserService;

        GetListWorkplaces();

        var timer = new Timer(1000);
        timer.Elapsed += Timer_Elapsed;
        timer.Start();
    }

    public async void GetListWorkplaces()
    {
        Workplaces = await _dbServiceService.GetWorkplacesAsync();
    }

    public string Name => _authUserService.Name;

    public DateTime MinDate => GetMinimalDate();

    public DateTime MaxDate => GetMaxDate();

    [RelayCommand]
    private void AddToDataBase()
    {
        if ((WorkTime != null) & (Location != null))
        {
            _dbServiceService.CreateWorkSessionAsync(new WorkSession(WorkTime, Location.Name, Description));

            WorkTime = string.Empty;
            Location = null;
            Description = null;
        }
    }

    private DateTime GetMinimalDate()
    {
        var minDate = DateTime.Now - new TimeSpan(14, 0, 0, 0, 0);

        return minDate;
    }

    private DateTime GetMaxDate()
    {
        var maxDate = DateTime.Now + new TimeSpan(14, 0, 0, 0, 0);

        return maxDate;
    }

    #region clock code

    private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        UpdateActualDateTime();
    }

    private void UpdateActualDateTime()
    {
        MainThread.BeginInvokeOnMainThread(() => { CurrentTime = DateTime.Now.ToString("dddd HH:mm:ss"); });
    }

    #endregion


}