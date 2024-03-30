using System.Collections.ObjectModel;
using System.Windows.Input;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public class EditLocationViewModel : ViewModelBase
{
    private readonly DBService _dbService;
    private string _name = string.Empty;

    private ObservableCollection<Workplace> _workplaces = new();

    public EditLocationViewModel(DBService dbService)
    {
        _dbService = dbService;
        
        LoadWorkplacesAsync();
    }

    private async void LoadWorkplacesAsync()
    {
        List<Workplace> list = await _dbService.GetWorkplacesAsync();
        Workplaces = new ObservableCollection<Workplace>(list);
    }

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
                _name = value;
        }
    }

    public ObservableCollection<Workplace> Workplaces
    {
        get => _workplaces;
        set
        {
            if (_workplaces != value)
            {
                SetField(ref _workplaces, value);
            }
        }
    }

    public ICommand AddToListCommand => new Command(AddToList);

    private void AddToList()
    {
        try
        {
            var item = new Workplace { Name = _name };
            if (!_workplaces.Contains(item))
            {
                _workplaces.Add(item);
                _dbService.CreateWorkplace(item);
            }
            else
            {
                Shell.Current.DisplayAlert("Informacja", "To miejsce znajduje się już w bazie", "OK");
            }

            // if (_dbService.ListOfWorkplaces.Count > 0)
            //     _dbService.ListOfWorkplaces.Clear();
            //
            // foreach (var workplace in Workplaces) _dbService.ListOfWorkplaces.Add(workplace);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Shell.Current.DisplayAlert("Test", $"Error{e.Message}", "OK");
        }
    }
}