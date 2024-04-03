using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using WorkHours.Models;

namespace WorkHours.Services;

public class DBService
{
    private readonly SQLiteAsyncConnection _connection;
    private readonly string _dbPath;
    private bool authState = false;
    public List<Workplace> ListOfWorkplaces = new();
    public User user;

    public DBService(string dbPath)
    {
        _dbPath = dbPath;

        _connection = new SQLiteAsyncConnection(_dbPath);
        _connection.CreateTablesAsync<WorkSession, Workplace>();
    }

    public async Task<List<WorkSession>> GetWorkSessionsListAsync()
    {
        return await _connection.Table<WorkSession>().ToListAsync();
    }

    public void CreateWorkSessionAsync(WorkSession workSession)
    {
        _connection.InsertAsync(workSession);
    }


    public async Task<List<Workplace>> GetWorkplacesAsync()
    {
        return await _connection.Table<Workplace>().ToListAsync();
    }

    public void CreateWorkplace(Workplace workplace)
    {
        _connection.InsertAsync(workplace);
    }

    public async void DeleteWorkplace(string name)
    {
        var workplaces = await GetWorkplacesAsync();
        var workplaceToRemove = (workplaces.FirstOrDefault(x => x.Name == name)).Id;
        await _connection.DeleteAsync<Workplace>(workplaceToRemove);
    }
}