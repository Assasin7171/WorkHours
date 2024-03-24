using SQLite;
using WorkHours.Models;

namespace WorkHours.Services
{
    public class DBService
    {
        private string _dbPath;
        public SQLiteAsyncConnection _connection;
        private bool authState = false;
        public User user;
        public List<Workplace> ListOfWorkplaces = new List<Workplace>();

        public DBService(string dbPath)
        {
            _dbPath = dbPath;

            _connection = new SQLiteAsyncConnection(_dbPath);
            _connection.CreateTablesAsync<WorkSession, Workplace, User>();
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