using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SkiaSharp;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public partial class DataViewModel : ObservableObject
{
    private readonly DataStoreService _dataStoreService;
    private readonly List<ChartEntry> _chartData = new List<ChartEntry>();
    private readonly Random _random = new();

    [ObservableProperty] private Chart _chart;
    [ObservableProperty] private string _arrowImageWeekly = "arrow_down.png";
    [ObservableProperty] private string _arrowImageMonthly = "arrow_down.png";
    [ObservableProperty] private string _arrowImageYearly = "arrow_down.png";
    [ObservableProperty] private bool _isWeeklyExpanded;
    [ObservableProperty] private bool _isMonthlyExpanded;
    [ObservableProperty] private bool _isYearlyExpanded;
    [ObservableProperty] private int _workedHours;
    [ObservableProperty] private decimal _earnedMoney;
    [ObservableProperty] private bool _isLoading;

    public DataViewModel(DataStoreService dataStoreService)
    {
        _dataStoreService = dataStoreService;
    }

    private (DateTime start, DateTime end) GetWeekRange(DateTime data)
    {
        int daysToSubtract = (int)data.DayOfWeek - (int)DayOfWeek.Monday;
        if (daysToSubtract < 0)
            daysToSubtract += 7;

        var start = data.Date.AddDays(-daysToSubtract);
        var end = start.AddDays(7);

        return (start, end);
    }

    [RelayCommand]
    private void Init()
    {
        //Wymaga resetowania co ładowanie.
        _chartData.Clear();
        EarnedMoney = 0;
        WorkedHours = 0;
        
        IsLoading = true;

        var week = GetWeekRange(DateTime.Now);

        var thisWeek = _dataStoreService.Worksessions
            .Where(w => w.CreatedTime >= week.start && w.CreatedTime <= week.end)
            .ToList();

        var orderedDays = new[]
        {
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday,
            DayOfWeek.Sunday
        };

        var sorted = orderedDays
            .Select(day => (
                day,
                hours: thisWeek
                    .Where(w => w.CreatedTime.DayOfWeek == day)
                    .Sum(w => w.HoursWorked)))
            .ToList();

        var dayShortNames = new Dictionary<DayOfWeek, string>
        {
            { DayOfWeek.Monday, "Pn" },
            { DayOfWeek.Tuesday, "Wt" },
            { DayOfWeek.Wednesday, "Śr" },
            { DayOfWeek.Thursday, "Cz" },
            { DayOfWeek.Friday, "Pt" },
            { DayOfWeek.Saturday, "Sb" },
            { DayOfWeek.Sunday, "Nd" },
        };

        foreach (var (day, hours) in sorted)
        {
            WorkedHours += hours;

            var isToday = day == DateTime.Today.DayOfWeek;

            _chartData.Add(new ChartEntry(hours)
            {
                Label = dayShortNames[day],
                ValueLabel = hours.ToString(),
                Color = isToday
                    ? SKColors.Red
                    : new SKColor(
                        (byte)_random.Next(0, 256),
                        (byte)_random.Next(0, 256),
                        (byte)_random.Next(0, 256))
            });
        }

        //obliczam ile zarobiono i z ilu godzin.
        var ratio = _dataStoreService.WorkRate.LastOrDefault()?.ValueRate ?? 0;

        EarnedMoney = (WorkedHours * ratio);
        

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

        IsLoading = false;
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