
/* Unmerged change from project 'WorkHours (net8.0-maccatalyst)'
Before:
using System.Collections.Generic;
After:
using SQLite;
using System.Collections.Generic;
*/

/* Unmerged change from project 'WorkHours (net8.0-windows10.0.19041.0)'
Before:
using System.Collections.Generic;
After:
using SQLite;
using System.Collections.Generic;
*/

/* Unmerged change from project 'WorkHours (net8.0-ios)'
Before:
using System.Collections.Generic;
After:
using SQLite;
using System.Collections.Generic;
*/
using SQLite;
using System.Collections.ObjectModel;
using
/* Unmerged change from project 'WorkHours (net8.0-maccatalyst)'
Before:
using SQLite;
using WorkHours.Models;
After:
using WorkHours.Models;
*/

/* Unmerged change from project 'WorkHours (net8.0-windows10.0.19041.0)'
Before:
using SQLite;
using WorkHours.Models;
After:
using WorkHours.Models;
*/

/* Unmerged change from project 'WorkHours (net8.0-ios)'
Before:
using SQLite;
using WorkHours.Models;
After:
using WorkHours.Models;
*/
WorkHours.Models;

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

    public async Task<bool> DeleteWorkSession(WorkSession x, ObservableCollection<WorkSession> workSession)
    {
        await _connection.DeleteAsync<WorkSession>(x.Id);

        if (workSession.Remove(x))
            return true;

        return false;
    }

    public async Task<List<Workplace>> GetWorkplacesAsync()
    {
        return await _connection.Table<Workplace>().ToListAsync();
    }

    public void CreateWorkplace(Workplace workplace)
    {
        _connection.InsertAsync(workplace);
    }

    public async Task<bool> DeleteWorkplace(Workplace x, ObservableCollection<Workplace> workplaces)
    {
        await _connection.DeleteAsync<Workplace>(x.Id);

        if (workplaces.Remove(x))
            return true;

        return false;
    }
}