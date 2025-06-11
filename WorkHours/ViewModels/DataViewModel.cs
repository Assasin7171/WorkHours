using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SkiaSharp;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public partial class DataViewModel : ObservableObject
{
    private readonly DataStoreService _dataStoreService;
    private bool _changeImage = true;
    private List<ChartEntry> _chartData = new List<ChartEntry>();
    private readonly Random _random = new();

    [ObservableProperty] private Chart _chart;
    [ObservableProperty] private string _arrowImageWeekly = "arrow_down.png";
    [ObservableProperty] private string _arrowImageMonthly = "arrow_down.png";
    [ObservableProperty] private string _arrowImageYearly = "arrow_down.png";
    [ObservableProperty] private bool _isWeeklyExpanded;
    [ObservableProperty] private bool _isMonthlyExpanded;
    [ObservableProperty] private bool _isYearlyExpanded;
    [ObservableProperty] private int _workedHours;

    public DataViewModel(DataStoreService dataStoreService)
    {
        _dataStoreService = dataStoreService;

        var today = DateTime.Today;
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday); // początek tygodnia (poniedziałek)
        var endOfWeek = startOfWeek.AddDays(7); // koniec tygodnia (poniedziałek następnego tygodnia)

        var thisWeek = _dataStoreService.Worksessions
            .Where(w => w.CreatedTime >= startOfWeek && w.CreatedTime < endOfWeek)
            .ToList();
        
        foreach (var worksession in thisWeek)
        {
            //zliczam przepracowane godziny
            WorkedHours += worksession.HoursWorked;
            
            _chartData.Add(new ChartEntry(worksession.HoursWorked)
            {
                Label = worksession.CreatedTime.DayOfWeek.ToString(),
                ValueLabel = $"{worksession.HoursWorked}",
                Color = new SKColor(
                    (byte)_random.Next(0, 256),
                    (byte)_random.Next(0, 256),
                    (byte)_random.Next(0, 256))
            });
        }

        while (_chartData.Count < 7)
        {
            _chartData.Add(new ChartEntry(0));
        }
        
        Chart = new BarChart()
        {
            Entries = _chartData,
            LabelOrientation = Orientation.Horizontal,
            LabelTextSize = 40,
            ValueLabelTextSize = 40,
            ValueLabelOrientation = Orientation.Horizontal,
            CornerRadius = 10,
        };
    }

    //pomyśleć nad refaktoryzacją kodu
    [RelayCommand]
    private void ChangeArrowImage(string type)
    {
        if (type == "weekly")
        {
            if (!IsWeeklyExpanded)
            {
                ArrowImageWeekly = "arrow_up.png";
                IsWeeklyExpanded = true;

                return;
            }

            ArrowImageWeekly = "arrow_down.png";
            IsWeeklyExpanded = false;
        }
        else if (type == "monthly")
        {
            if (!IsMonthlyExpanded)
            {
                ArrowImageMonthly = "arrow_up.png";
                IsMonthlyExpanded = true;

                return;
            }

            ArrowImageMonthly = "arrow_down.png";
            IsMonthlyExpanded = false;
        }
        else if (type == "yearly")
        {
            if (!IsYearlyExpanded)
            {
                ArrowImageYearly = "arrow_up.png";
                IsYearlyExpanded = true;

                return;
            }

            ArrowImageYearly = "arrow_down.png";
            IsYearlyExpanded = false;
        }
    }
}