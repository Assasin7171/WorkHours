using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public class MainPageViewModel : ViewModelBase
{
    private readonly DBService _dbServiceService;
    private readonly UserService _userService;

    private string? _description;

    private string? _location;
    private DateTime _workDate = DateTime.Now;
    private string? _workTime;
    private string? _currentTime;
    private ObservableCollection<Workplace> _workplaces;

    public ObservableCollection<Workplace> Workplaces
    {
        get
        {
            if (_dbServiceService.ListOfWorkplaces.Count > 0)
            {
                foreach (var item in _dbServiceService.ListOfWorkplaces)
                {
                    _workplaces.Add(item);
                }
            }

            return _workplaces;
        }
    }

    public User User => _userService.User;

    public MainPageViewModel(DBService dbService, UserService userService)
    {
        _dbServiceService = dbService;
        _userService = userService;

        #region clock code
        
        var timer = new System.Timers.Timer(1000);
        timer.Elapsed += Timer_Elapsed;
        timer.Start();
        
        #endregion
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

    public string CurrentTime
    {
        get => _currentTime;
        set
        {
            if (_currentTime != value)
            {
                SetField(ref _currentTime, value);
            }
        }
    }

    public DateTime MinDate
    {
        get => GetMinimalDate();
    }

    public DateTime MaxDate
    {
        get => GetMaxDate();
    }

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

    public string? Location
    {
        get => _location;
        set => SetField(ref _location, value);
    }

    public string? Description
    {
        get => _description;
        set => SetField(ref _description, value);
    }

    public ICommand AddToDataBase => new Command(ClickAddToBase);

    private void ClickAddToBase()
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
}