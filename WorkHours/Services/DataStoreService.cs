using WorkHoursDb;
using WorkHoursDb.Entities;

namespace WorkHours.Services;

public class DataStoreService : IDataStoreService
{
    public List<Place> Places { get; set; } = new List<Place>();
    public List<Worksession> Worksessions { get; set; } = new List<Worksession>();
    public WorkHoursContext WorkHoursContext { get; set; }
    public event EventHandler? PlacesChanged;
    public event EventHandler? WorksessionsChanged;

    public DataStoreService(WorkHoursContext workHoursContext)
    {
        WorkHoursContext = workHoursContext;
        //Inicjalizacja miejsc pracy oraz sesji pracy
        Places = workHoursContext.Places.ToList();
        Worksessions = workHoursContext.Worksessions.ToList();  
    }

    public void AddPlace(Place place)
    {
        Places.Add(place);
        PlacesChanged?.Invoke(this, EventArgs.Empty);
        WorkHoursContext.SaveChanges();
    }

    public void AddWorksession(Worksession worksession)
    {
        Worksessions.Add(worksession);
        WorksessionsChanged?.Invoke(this, EventArgs.Empty);
        WorkHoursContext.SaveChanges();
    }
}