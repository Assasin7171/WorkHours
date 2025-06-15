using WorkHours.Database.Entities;


namespace WorkHours.Services;

public class DataStoreService
{
    private readonly DatabaseContext _db;
    public List<Place> Places { get; set; } = new List<Place>();
    public List<Worksession> Worksessions { get; set; } = new List<Worksession>();
    public List<WorkRate> WorkRate { get; set; } = new List<WorkRate>();
    

    public DataStoreService(DatabaseContext db)
    {
        _db = db;
    }

    // pobieranie danych z bazy 
    public async Task GetDataAsync()
    {
        try
        {
            Places = await _db.SqLiteAsyncConnection.Table<Place>().ToListAsync();
            Worksessions = await _db.SqLiteAsyncConnection.Table<Worksession>().ToListAsync();
            // naprawienia musi byc pobrany jeden.
            WorkRate = await _db.SqLiteAsyncConnection.Table<WorkRate>().ToListAsync();

            foreach (var worksession in Worksessions)
            {
                worksession.Place = Places.First(x=>x.Id == worksession.PlaceId);
            }
        }
        catch (Exception e)
        {
            await Shell.Current.DisplayAlert("Error", e.Message, "OK");
        }
    }

    public async Task<int> RemoveWorkSession(Worksession worksession)
    {
        var result = await _db.SqLiteAsyncConnection.DeleteAsync(worksession);

        await GetDataAsync();

        return result;
    }
    
    public async Task<int> AddPlace(Place place)
    {
        var result = await _db.SqLiteAsyncConnection.InsertAsync(place);

        await GetDataAsync();

        return result;
    }

    public async Task<int> AddWorksession(Worksession worksession)
    {
        int result = await _db.SqLiteAsyncConnection.InsertAsync(worksession);

        await GetDataAsync();

        if (result == 0)
        {
            await Shell.Current.DisplayAlert("Error", "Worksession must be linked to a Place(PlaceId can't be null)",
                "Ok");
        }

        return result;
    }

    public async Task<int> AddWorkRate(WorkRate workRate)
    {
        int result = await _db.SqLiteAsyncConnection.InsertAsync(workRate);

        await GetDataAsync();

        if (result == 0)
        {
            await Shell.Current.DisplayAlert("Error", "Work rate must be a decimal value",
                "Ok");
        }

        return result;
    }
}