using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Timer = System.Timers.Timer;

namespace WorkHours.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public DateTime MinDate => GetMinimalDate();
    public DateTime MaxDate => GetMaxDate();
    [ObservableProperty] private DateTime _actualDate = DateTime.Now;

    public MainViewModel()
    {
        var timer = new Timer(1000);
        timer.Elapsed += (sender, args) =>
        {
            ActualDate = DateTime.Now;
        };
        timer.Start();
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

    [RelayCommand]
    private void AddToDb()
    {
        
    }
}