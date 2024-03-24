using System.Collections.ObjectModel;
using System.Windows.Input;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public class EditLocationViewModel
{
    private readonly DBService _dbService;
    private ObservableCollection<Workplace> _workplaces = new();
    public Workplace Workplace { get; set; }

    public ObservableCollection<Workplace> Workplaces
    {
        get => _workplaces;
        set
        {
            if (_workplaces != value)
            {
                _workplaces = value;
            }
        }
    }

    public ICommand AddToListCommand => new Command(AddToList);
    public EditLocationViewModel(DBService dbService)
    {
        _dbService = dbService;
    }

    private void AddToList()
    {
        _workplaces.Add(Workplace);
        
        if(_dbService.ListOfWorkplaces.Count > 0)
            _dbService.ListOfWorkplaces.Clear();
        
        foreach (var workplace in _workplaces)
        {
            _dbService.ListOfWorkplaces.Add(workplace);
        }
    }
}