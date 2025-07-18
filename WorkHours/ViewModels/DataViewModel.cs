using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microcharts;
using SkiaSharp;
using System.Globalization;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public partial class DataViewModel : ObservableObject
{
    private readonly Random _random = new();
    private readonly DataStoreService _dataStoreService;

    private readonly List<ChartEntry> _chartDataWeekly = new List<ChartEntry>();
    private readonly List<ChartEntry> _chartDataMonthly = new List<ChartEntry>();
    private readonly List<ChartEntry> _chartDataYearly = new List<ChartEntry>();
    private readonly List<ChartEntry> _chartDataYearlyFreeDays = new List<ChartEntry>();

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
    [ObservableProperty] private Chart _chartYearlyFreeDays;

    [ObservableProperty] private bool _showChartWeekly;
    [ObservableProperty] private bool _showChartMonthly;
    [ObservableProperty] private bool _showChartYearly;

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
    public async Task Init()
    {
        //Do tygodniowych statystyk
        LoadWeeklyData();
        //Do miesięcznych statystyk
        await LoadMonthlyData();
        //Do rocznych statystyk
        await LoadYearlyData();
        //Sprawdzanie czy wyświetlić wykresy
        CheckChartEntry();
    }

    private void CheckChartEntry()
    {
        foreach (var chartEntry in _chartDataWeekly)
        {
            if (chartEntry.Value > 0)
            {
                ShowChartWeekly = true;
            }
        }

        foreach (var chartEntry in _chartDataMonthly)
        {
            if (chartEntry is { Value: > 0, Label: "Pracujące dni" })
            {
                ShowChartMonthly = true;
            }
        }

        foreach (var chartEntry in _chartDataYearly)
        {
            if (chartEntry.Value > 0)
            {
                ShowChartYearly = true;
            }
        }
    }

    private async Task LoadYearlyData()
    {
        try
        {
            IsLoading = true;
            EarnedMoneyYearly = 0;
            AppTheme currentTheme = Application.Current!.RequestedTheme;
            var freeColor = currentTheme == AppTheme.Dark ? SKColor.Parse("#4B5563") : SKColor.Parse("#D1D5DB");
            var workColor = currentTheme == AppTheme.Dark ? SKColor.Parse("#6EE7B7") : SKColor.Parse("#10B981");
            var labelColor = currentTheme == AppTheme.Dark ? SKColors.White : SKColors.Black;
            var backgroundColor = currentTheme == AppTheme.Dark ? SKColors.Black : SKColors.White;

            _chartDataYearly.Clear();
            var shortMonthNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
            var hoursInMonths = _dataStoreService.Worksessions.Select(x => new { x.HoursWorked, x.CreatedTime })
                .ToList();

            var hourlyRate = Preferences.Get("HourlyRate", 0);

            await _dataStoreService.GetDataAsync();
            var data = _dataStoreService.Worksessions;

            var countedHours = data.Where(x => x.IsSettled && x.CreatedTime.Year == DateTime.Now.Year).Sum(x => x.HoursWorked);

            EarnedMoneyYearly += (countedHours * hourlyRate);

            for (int i = 0; i <= 11; i++)
            {
                var hoursInMonth = hoursInMonths.Where(x => x.CreatedTime.Month == i + 1).Select(x => x.HoursWorked)
                    .Sum();



                var isCurrentMonth = (i + 1) == DateTime.Today.Month;
                var entryColor = isCurrentMonth
                    ? (currentTheme == AppTheme.Dark ? SKColor.Parse("#F87171") : SKColor.Parse("#DC2626"))
                    : (currentTheme == AppTheme.Dark ? SKColor.Parse("#60A5FA") : SKColor.Parse("#2563EB"));

                _chartDataYearly.Add(new ChartEntry(hoursInMonth)
                {
                    Label = $"{shortMonthNames[i]}",
                    ValueLabel = $"{hoursInMonth}",
                    Color = entryColor,
                    ValueLabelColor = labelColor,
                    TextColor = entryColor,
                });
            }

            ChartYearly = new BarChart()
            {
                Entries = _chartDataYearly,
                LabelOrientation = Orientation.Horizontal,
                LabelTextSize = 40,
                ValueLabelTextSize = 40,
                ValueLabelOrientation = Orientation.Horizontal,
                CornerRadius = 10,
                BackgroundColor = backgroundColor,
                LabelColor = labelColor,
            };


            _chartDataYearlyFreeDays.Clear();

            int totalDays = DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365;
            var workedDays = _dataStoreService.Worksessions.Where(x => x.CreatedTime.Year == DateTime.Now.Year)
                .Select(x => x.CreatedTime.Date)
                .Distinct()
                .Count();
            var freeDays = totalDays - workedDays;



            _chartDataYearlyFreeDays.Add(new ChartEntry(freeDays)
            {
                Label = "Wolne dni",
                ValueLabel = $"{freeDays}",
                Color = freeColor,
                ValueLabelColor = labelColor,
            });
            _chartDataYearlyFreeDays.Add(new ChartEntry(workedDays)
            {
                Label = "Pracujące dni",
                ValueLabel = $"{workedDays}",
                Color = workColor,
                ValueLabelColor = labelColor,
            });

            ChartYearlyFreeDays = new DonutChart()
            {
                Entries = _chartDataYearlyFreeDays,
                MaxValue = totalDays,
                LabelTextSize = 40,
                BackgroundColor = backgroundColor,
            };


        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadMonthlyData()
    {
        try
        {
            IsLoading = true;

            AppTheme currentTheme = Application.Current.RequestedTheme;
            var freeColor = currentTheme == AppTheme.Dark ? SKColor.Parse("#4B5563") : SKColor.Parse("#D1D5DB");
            var workColor = currentTheme == AppTheme.Dark ? SKColor.Parse("#6EE7B7") : SKColor.Parse("#10B981");
            var labelColor = currentTheme == AppTheme.Dark ? SKColors.White : SKColors.Black;
            var backgroundColor = currentTheme == AppTheme.Dark ? SKColors.Black : SKColors.White;

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
                Color = workColor,
                ValueLabelColor = labelColor,
            });
            _chartDataMonthly.Add(new ChartEntry(freeDays)
            {
                Label = "Wolne dni",
                ValueLabel = $"{freeDays}",
                Color = freeColor,
                ValueLabelColor = labelColor,
            });

            ChartMonthly = new DonutChart()
            {
                Entries = _chartDataMonthly,
                LabelTextSize = 40,
                MaxValue = daysInMonth,
                BackgroundColor = backgroundColor,
                LabelColor = labelColor,
            };

            var hourlyRate = Preferences.Get("HourlyRate", 0);

            await _dataStoreService.GetDataAsync();
            var data = _dataStoreService.Worksessions;

            var countedHours = data.Where(x => x.IsSettled && x.CreatedTime.Month == DateTime.Now.Month).Sum(x => x.HoursWorked);


            EarnedMoneyMonthly = (hourlyRate * countedHours);
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
            AppTheme currentTheme = Application.Current.RequestedTheme;

            var labelColor = currentTheme == AppTheme.Dark ? SKColors.White : SKColors.Black;
            var backgroundColor = currentTheme == AppTheme.Dark ? SKColors.Black : SKColors.White;
            var regularBarColor = currentTheme == AppTheme.Dark ? SKColor.Parse("#60A5FA") : SKColor.Parse("#2563EB");
            var todayBarColor = currentTheme == AppTheme.Dark ? SKColor.Parse("#F87171") : SKColor.Parse("#DC2626");

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
                    ValueLabelColor = (currentTheme == AppTheme.Dark ? SKColors.White : SKColors.Black),
                    Color = isToday ? todayBarColor : regularBarColor,
                    TextColor = isToday ? todayBarColor : regularBarColor,
                });
            }

            var hourlyRate = Preferences.Get("HourlyRate", 0);

            //obliczam ile zarobiono i z ilu godzin.
            EarnedMoneyWeekly = (WorkedHoursWeekly * hourlyRate);
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
                BackgroundColor = backgroundColor,
                LabelColor = labelColor,
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