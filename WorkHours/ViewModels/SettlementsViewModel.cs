using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

            //ryzykowna metoda, przy jakimś niespodziewanym błędzie mogą zniknąć wszystkie sesje...
            await _dataStoreService.UpdateWorksessions(sessions);
            
            //await Init();
            
            await Shell.Current.DisplayAlert("INFO", $"Rozliczono tydzien {SelectedWeek.Display}", "OK");
        }

        partial void OnSelectedWeekChanged(WeekRange value)
        {
            HoursCountWeekly = 0;
            EarnedMoneyWeekly = 0;
            _filteredSessions = _worksessions.Where(x =>
                x.CreatedTime >= value.Start && x.CreatedTime <= (value.Start + TimeSpan.FromDays(7))).ToList();

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
    }
}