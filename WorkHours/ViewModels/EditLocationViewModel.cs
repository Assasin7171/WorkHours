using System.Collections.ObjectModel;
using System.Windows.Input;
using WorkHours.Models;
using WorkHours.Services;

namespace WorkHours.ViewModels;

public class EditLocationViewModel
{
    private readonly DBService _dbService;
    private string _name = string.Empty;

    public EditLocationViewModel(DBService dbService)
    {
        _dbService = dbService;
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

    public ObservableCollection<Workplace> Workplaces { get; } = new();

    public ICommand AddToListCommand => new Command(AddToList);

    private void AddToList()
    {
        try
        {
            Workplaces.Add(new Workplace { Name = _name });

            if (_dbService.ListOfWorkplaces.Count > 0)
                _dbService.ListOfWorkplaces.Clear();

            foreach (var workplace in Workplaces) _dbService.ListOfWorkplaces.Add(workplace);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}