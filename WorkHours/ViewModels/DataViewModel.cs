using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SkiaSharp;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public partial class DataViewModel : ObservableObject
{
    private readonly Random _random = new();
    private readonly DataStoreService _dataStoreService;
    private readonly decimal _ratio;

    private readonly List<ChartEntry> _chartDataWeekly = new List<ChartEntry>();
    private readonly List<ChartEntry> _chartDataMonthly = new List<ChartEntry>();
    private readonly List<ChartEntry> _chartDataYearly = new List<ChartEntry>();

    [ObservableProperty] private int _workedHoursWeekly;
    [ObservableProperty] private int _workedHoursMonthly;
    [ObservableProperty] private int _workedHoursYearly;
    [ObservableProperty] private int _freeDaysYearly;
    
    [ObservableProperty] private double _averageHoursWeekly;
    
    [ObservableProperty] private decimal _earnedMoneyWeekly;
    [ObservableProperty] private decimal _earnedMoneyMonthly;
    [ObservableProperty] private decimal _earnedMoneyYearly;
    
    [ObservableProperty] private Chart _chartWeekly;
    [ObservableProperty] private Chart _chartMonthly;
    [ObservableProperty] private Chart _chartYearly;

    [ObservableProperty] private string _arrowImageWeekly = "arrow_down.png";
    [ObservableProperty] private string _arrowImageMonthly = "arrow_down.png";
    [ObservableProperty] private string _arrowImageYearly = "arrow_down.png";
    
    [ObservableProperty] private bool _isWeeklyExpanded;
    [ObservableProperty] private bool _isMonthlyExpanded;
    [ObservableProperty] private bool _isYearlyExpanded;
    [ObservableProperty] private bool _isLoading;
    

    public DataViewModel(DataStoreService dataStoreService)
    {
        _dataStoreService = dataStoreService;

        _ratio = _dataStoreService.WorkRate.LastOrDefault()?.ValueRate ?? 0;
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
        //Do tygodniowych statystyk
        LoadWeeklyData();
        //Do miesięcznych statystyk
        LoadMonthlyData();
        //Do rocznych statystyk
        LoadYearlyData();
    }

    private void LoadYearlyData()
    {
        try
        {
            IsLoading = true;
            
            
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void LoadMonthlyData()
    {
        try
        {
            IsLoading = true;

            _chartDataMonthly.Clear();
            EarnedMoneyMonthly = 0;
            WorkedHoursMonthly = 0;

            var month = DateTime.Now.Month;
            var daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, month);

            var thisMonthData = _dataStoreService.Worksessions
                .Where(w => w.CreatedTime.Month == month)
                .ToList();

            var workingDays = thisMonthData.Count;
            var freeDays = daysInMonth - workingDays;


            foreach (var item in thisMonthData)
            {
                WorkedHoursMonthly += item.HoursWorked;
            }

            _chartDataMonthly.Add(new ChartEntry(workingDays)
            {
                Label = "Pracujące dni",
                ValueLabel = $"{workingDays}",
                Color = SKColors.DarkGreen
            });
            _chartDataMonthly.Add(new ChartEntry(freeDays)
            {
                Label = "Wolne dni",
                ValueLabel = $"{freeDays}",
                Color = SKColors.DarkMagenta
            });

            ChartMonthly = new DonutChart()
            {
                Entries = _chartDataMonthly,
                LabelTextSize = 40,
                MaxValue = daysInMonth,
            };

            EarnedMoneyMonthly = (_ratio * WorkedHoursMonthly);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void LoadWeeklyData()
    {
        try
        {
            IsLoading = true;

            //Wymaga resetowania co ładowanie.
            _chartDataWeekly.Clear();
            EarnedMoneyWeekly = 0;
            WorkedHoursWeekly = 0;

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
                WorkedHoursWeekly += hours;

                var isToday = day == DateTime.Today.DayOfWeek;

                _chartDataWeekly.Add(new ChartEntry(hours)
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
            EarnedMoneyWeekly = (WorkedHoursWeekly * _ratio);
            //średnia dzienna przepracowanych godzin z tygodnia.
            AverageHoursWeekly = WorkedHoursWeekly / 7;

            while (_chartDataWeekly.Count < 7)
            {
                _chartDataWeekly.Add(new ChartEntry(0));
            }

            ChartWeekly = new BarChart()
            {
                Entries = _chartDataWeekly,
                LabelOrientation = Orientation.Horizontal,
                LabelTextSize = 40,
                ValueLabelTextSize = 40,
                ValueLabelOrientation = Orientation.Horizontal,
                CornerRadius = 10,
            };
        }
        finally
        {
            IsLoading = false;
        }
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