using WorkHoursDb;
using WorkHoursDb.Entities;

namespace WorkHours.Services;

public interface IDataStoreService
{
    public List<Place> Places { get; set; }
    public List<Worksession> Worksessions { get; set; }
    public WorkHoursContext WorkHoursContext { get; set; }
    event EventHandler PlacesChanged;
}