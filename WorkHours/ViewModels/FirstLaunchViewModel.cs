using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace WorkHours.ViewModels;

public partial class FirstLaunchViewModel : ObservableObject
{
    public ObservableCollection<string> Languages { get; } = new(["Polski", "English"]);
    public ObservableCollection<string> Currencies { get; } = new(["zł", "$", "€"]);

    [ObservableProperty] private string _selectedLanguage = string.Empty;
    [ObservableProperty] private string _selectedCurrency = string.Empty;
    [ObservableProperty] private string _workRateValue = string.Empty;


    [RelayCommand]
    private void SaveSettings()
    {
        Preferences.Set("HourlyRate", WorkRateValue);
        Preferences.Set("SelectedLanguage", SelectedLanguage);
        Preferences.Set("SelectedCurrency", SelectedCurrency);
        Preferences.Set("HasLaunchedBefore", true);

        // Przejście do głównej aplikacji
        Application.Current!.MainPage = new AppShell();
        Console.WriteLine();
    }

}