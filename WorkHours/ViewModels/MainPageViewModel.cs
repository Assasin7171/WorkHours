using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using WorkHours.Models;
using WorkHours.Services;
using Timer = System.Timers.Timer;

namespace WorkHours.ViewModels;

public class MainPageViewModel : ViewModelBase
{
    private readonly AuthUserService _authUserService;
    private readonly DBService _dbServiceService;
    private string? _currentTime = string.Empty;

    private string? _description = string.Empty;

    private string? _location = string.Empty;
    private DateTime _workDate = DateTime.Now;
    private List<Workplace> _workplaces = new();
    private string? _workTime = string.Empty;

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
    
    public List<Workplace> Workplaces
    {
        get => _workplaces;
        set => SetField(ref _workplaces, value);
    }

    public string Name => _authUserService.Name;

    public string CurrentTime
    {
        get => _currentTime;
        set
        {
            if (_currentTime != value) SetField(ref _currentTime, value);
        }
    }

    public DateTime MinDate => GetMinimalDate();

    public DateTime MaxDate => GetMaxDate();

    public string? WorkTime
    {
        get => _workTime;
        set => SetField(ref _workTime, value);
    }

    public DateTime WorkDate
    {
        get => _workDate;
        set => SetField(ref _workDate, value);
    }

    public string Location
    {
        get => _location;
        set => SetField(ref _location, value);
    }

    public string? Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    public ICommand LogoutCommand => new Command(Logout);

    public ICommand AddToDataBaseCommand => new Command(AddToBase);

    public ICommand SelectLocationFromPickerCommand => new Command(SelectLocation);

    private void SelectLocation(object? sender)
    {
        throw new NotImplementedException();
    }

    private void Logout()
    {
        _authUserService.Logout();
    }

    private void AddToBase()
    {
        if ((_workTime != null) & (_location != null))
        {
            _dbServiceService.CreateWorkSessionAsync(new WorkSession(_workTime, _location, _description));

            WorkTime = string.Empty;
            Location = string.Empty;
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