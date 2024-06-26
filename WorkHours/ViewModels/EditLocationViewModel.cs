using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public partial class EditLocationViewModel : ObservableObject
{
    private readonly DBService _dbService;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Workplace> _workplaces = new();

    public EditLocationViewModel(DBService dbService)
    {
        _dbService = dbService;

        LoadWorkplacesAsync();
    }

    private async void LoadWorkplacesAsync()
    {
        var list = await _dbService.GetWorkplacesAsync();
        Workplaces = new ObservableCollection<Workplace>(list);
    }

    [RelayCommand]
    private void AddToList()
    {
        try
        {
            var item = new Workplace { Name = Name };
            if (!Workplaces.Contains(item))
            {
                Workplaces.Add(item);
                _dbService.CreateWorkplace(item);
            }
            else
            {
                Shell.Current.DisplayAlert("Informacja", "To miejsce znajduje się już w bazie", "OK");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Shell.Current.DisplayAlert("Test", $"Error{e.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task DeleteFromListAsync(Workplace x)
    {
        bool isDeleted = await _dbService.DeleteWorkplace(x, Workplaces);

        if (isDeleted)
        {
            Debug.WriteLine("Usunięte poprawnie");
        }
    }
}