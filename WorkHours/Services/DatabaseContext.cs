using SQLite;
using WorkHours.Database.Entities;

namespace WorkHours.Services;

public class DatabaseContext
{
    private string _dbPath = Path.Combine(FileSystem.AppDataDirectory, "workhours.db");
    public SQLiteAsyncConnection SqLiteAsyncConnection { get; private set; }
    
    public DatabaseContext()
    {
        Initialize();
    }

    private void Initialize()
    {
        SqLiteAsyncConnection = new SQLiteAsyncConnection(_dbPath);
        //trzeba się upewnić że tabele są stworzone
        SqLiteAsyncConnection.CreateTableAsync<Worksession>().Wait();
        SqLiteAsyncConnection.CreateTableAsync<Place>().Wait();
        SqLiteAsyncConnection.CreateTableAsync<WorkRate>().Wait();
    }
}