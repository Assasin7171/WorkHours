using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WorkHours.Database.Entities;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels
{
    public partial class SettlementsViewModel : ObservableObject
    {
        public ObservableCollection<WeekRange> WeeksList { get; set; } = new ObservableCollection<WeekRange>();
        public ObservableCollection<MonthEntry> MonthsList { get; set; } = new();

        private readonly DataStoreService _dataStoreService;
        private List<Worksession> _filteredSessions;
        private List<Worksession> _filteredSessionsMonths;
        private List<Worksession> _worksessions;

        [ObservableProperty] private WeekRange _selectedWeek;
        [ObservableProperty] private decimal _earnedMoneyWeekly;
        [ObservableProperty] private int _hoursCountWeekly;

        [ObservableProperty] private string _selectedMonth;
        [ObservableProperty] private decimal _earnedMoneyMonthly;
        [ObservableProperty] private int _hoursCountMonthly;
        [ObservableProperty] private MonthEntry _selectedMonthEntry;

        [ObservableProperty] private bool _weekly;
        [ObservableProperty] private bool _monthly;
        [ObservableProperty] private bool _other;

        [ObservableProperty] private int _totalWorkedHours;

        [ObservableProperty] private DateTime _selectedStartDate = DateTime.Today.AddDays(-7);
        [ObservableProperty] private DateTime _selectedEndDate = DateTime.Today;

        [ObservableProperty] private int _hoursCountRange;
        [ObservableProperty] private decimal _earnedMoneyRange;

        public SettlementsViewModel(DataStoreService dataStoreService)
        {
            _dataStoreService = dataStoreService;
        }

        [RelayCommand]
        private async Task Init()
        {
            //pobieram wszystkie dany z bazy
            await _dataStoreService.GetDataAsync();
            //reset właściwości aby uniknąć duplikatów
            TotalWorkedHours = 0;
            HoursCountWeekly = 0;
            EarnedMoneyWeekly = 0;
            WeeksList.Clear();

            _worksessions = _dataStoreService.Worksessions;

            var today = DateTime.Today;
            var daysToSubstruct = (int)today.DayOfWeek - 1;
            var startOfThisWeek = today.AddDays(-daysToSubstruct);

            for (int i = 0; i < 4; i++)
            {
                var weekStart = startOfThisWeek.AddDays(-7 * i);
                WeeksList.Add(new WeekRange { Start = weekStart });
            }

            //zliczam wszystkie nierozliczone godziny
            foreach (var w in _worksessions)
            {
                if (!w.IsSettled)
                {
                    TotalWorkedHours += w.HoursWorked;
                }
            }

            //Do miesięcznych rozliczeń
            var current = DateTime.Today;
            MonthsList.Clear();

            for (int i = 0; i < 6; i++)
            {
                var month = current.AddMonths(-i);
                MonthsList.Add(new MonthEntry
                {
                    Year = month.Year,
                    Month = month.Month
                });
            }
        }

        [RelayCommand]
        private async Task SettleTheSelectedWeek()
        {
            if (_filteredSessions.Any())
            {
                await _dataStoreService.GetDataAsync();
                var sessions = _dataStoreService.Worksessions;

                foreach (var worksession in _filteredSessions)
                {
                    var orginal = sessions.FirstOrDefault(x => x.Id == worksession.Id);
                    if (orginal != null)
                    {
                        orginal.IsSettled = true;
                        orginal.SettledDate = DateTime.Now;
                    }
                }

                var count = await _dataStoreService.UpdateWorksessions(sessions);

                HoursCountWeekly = 0;
                EarnedMoneyWeekly = 0;

                await Shell.Current.DisplayAlert("INFO", $"Rozliczono tydzien {SelectedWeek.Display}", "OK");
            }
        }

        [RelayCommand]
        private async Task SettleCustomRange()
        {
            var filtered = _worksessions
                .Where(x => !x.IsSettled &&
                            x.CreatedTime >= SelectedStartDate &&
                            x.CreatedTime <= SelectedEndDate)
                .ToList();

            foreach (var session in filtered)
            {
                session.IsSettled = true;
                session.SettledDate = DateTime.Now;
            }

            await _dataStoreService.UpdateWorksessions(_worksessions);
            await Init();
            HoursCountRange = 0;
            EarnedMoneyRange = 0;
            await Shell.Current.DisplayAlert("INFO", "Rozliczono wybrany zakres dat", "OK");
        }

        partial void OnSelectedStartDateChanged(DateTime value) => CalculateRange();
        partial void OnSelectedEndDateChanged(DateTime value) => CalculateRange();

        partial void OnSelectedWeekChanged(WeekRange value)
        {
            if (value == null || _worksessions == null)
                return;

            HoursCountWeekly = 0;
            EarnedMoneyWeekly = 0;

            _filteredSessions = _worksessions
                .Where(x =>
                    x != null &&
                    x.CreatedTime != default && // lub x.CreatedTime.HasValue, jeśli to DateTime?
                    x.CreatedTime >= value.Start &&
                    x.CreatedTime <= value.Start.AddDays(7))
                .ToList();

            foreach (var worksession in _filteredSessions)
            {
                if (!worksession.IsSettled)
                {
                    HoursCountWeekly += worksession.HoursWorked;
                }
            }

            var hourlyRate = Preferences.Get("HourlyRate", 0);
            EarnedMoneyWeekly = HoursCountWeekly * hourlyRate;
        }

        [RelayCommand]
        private async Task SettleSelectedMonth()
        {
            if (_filteredSessionsMonths.Any())
            {
                await _dataStoreService.GetDataAsync();
                var sessions = _dataStoreService.Worksessions;

                foreach (var worksession in _filteredSessionsMonths)
                {
                    var orginal = sessions.FirstOrDefault(x => x.Id == worksession.Id);
                    if (orginal != null)
                    {
                        orginal.IsSettled = true;
                        orginal.SettledDate = DateTime.Now;
                    }
                }

                var count = await _dataStoreService.UpdateWorksessions(sessions);

                HoursCountMonthly = 0;
                EarnedMoneyMonthly = 0;

                await Shell.Current.DisplayAlert("INFO", $"Rozliczono miesiąc {SelectedMonthEntry.Display}", "OK");
            }
        }

        partial void OnSelectedMonthEntryChanged(MonthEntry value)
        {
            if (value == null || _worksessions == null)
                return;

            EarnedMoneyMonthly = 0;
            HoursCountMonthly = 0;

            _filteredSessionsMonths = _worksessions
                .Where(x =>
                    x != null &&
                    x.CreatedTime != default && // lub x.CreatedTime.HasValue, jeśli to DateTime?
                    x.CreatedTime >= value.Start &&
                    x.CreatedTime <= value.Start.AddDays(7))
                .ToList();

            foreach (var worksession in _filteredSessionsMonths)
            {
                if (!worksession.IsSettled)
                {
                    HoursCountMonthly += worksession.HoursWorked;
                }
            }

            var hourlyRate = Preferences.Get("HourlyRate", 0);
            EarnedMoneyMonthly = HoursCountMonthly * hourlyRate;
        }

        private void CalculateRange()
        {
            if (_worksessions == null || SelectedEndDate < SelectedStartDate)
                return;

            var filtered = _worksessions
                .Where(x => !x.IsSettled &&
                            x.CreatedTime >= SelectedStartDate &&
                            x.CreatedTime <= SelectedEndDate)
                .ToList();

            HoursCountRange = filtered.Sum(x => x.HoursWorked);
            var rate = Preferences.Get("HourlyRate", 0);
            EarnedMoneyRange = HoursCountRange * rate;
        }
    }
}