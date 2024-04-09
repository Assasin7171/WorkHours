using CommunityToolkit.Mvvm.ComponentModel;
using System.Web;
using WorkHours.Models;

namespace WorkHours.ViewModels;

public partial class MonthSummaryViewModel : ObservableObject, IQueryAttributable
{
    private double _countedDays;
    private double _countedFreeDays;

    [ObservableProperty] private Tuple<int, int> _workingDaysInMonth;
    [ObservableProperty] private int _allWorkingHoursInMonth;
    [ObservableProperty] private List<ChartData> _chartsData = new();

    public MonthSummaryViewModel()
    {
        InitDataInCharts();
    }

    private void InitDataInCharts()
    {
        _countedFreeDays = HowManyDaysInMonth(DateTime.Now.Day, DateTime.Now.Month).Item1 - _countedDays;

        ChartsData.Add(new ChartData("Przepracowane dni (1 dzieñ = 10H)", _countedDays));
        ChartsData.Add(new ChartData("Wolne dni", _countedFreeDays));
    }

    /// <returns>
    ///     First return - working days in month
    ///     Second return - free days in month like (Saturday/Sunday)
    /// </returns>
    private Tuple<int, int> HowManyDaysInMonth(int year, int month)
    {
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var workingDays = new List<int>();
        var freeDays = new List<int>();

        for (var i = 1; i < daysInMonth + 1; i++)
        {
            var date = new DateTime(year, month, i);
            var dayOfWeek = date.DayOfWeek;

            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                freeDays.Add(i);
            else
                workingDays.Add(i);
        }

        return new Tuple<int, int>(workingDays.Count, freeDays.Count);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _countedDays = int.Parse(HttpUtility.UrlDecode(query["countedDays"].ToString()));
    }
}