using SQLite;
using WorkHours.Models;

namespace WorkHours.DataServices
{
    public class DataBaseService
    {
        private string _dbPath;
        private SQLiteAsyncConnection _connection;

        public DataBaseService(string dbPath)
        {
            _dbPath = dbPath;

            _connection = new SQLiteAsyncConnection(_dbPath);
            _connection.CreateTablesAsync<WorkSession, Workplace>();
        }

        #region WorkSession

        public async Task<List<WorkSession>> GetWorkSessionsListAsync()
        {
            return await _connection.Table<WorkSession>().ToListAsync();
        }

        public void CreateWorkSessionAsync(WorkSession workSession)
        {
            _connection.InsertAsync(workSession);
        }

        #endregion

        #region Workplace

        public async Task<List<Workplace>> GetWorkplacesAsync()
        {
            return await _connection.Table<Workplace>().ToListAsync();
        }

        public void CreateWorkplace(Workplace workplace)
        {
            _connection.InsertAsync(workplace);
        }

        #endregion
    }
}