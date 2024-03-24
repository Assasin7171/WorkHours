using WorkHours.Models;

namespace WorkHours.Services;

public class UserService
{
    private readonly DBService _dbService;
    public User User { get; set; }
    public string Name { get; set; }
    public bool IsUserLogged = false;

    public UserService(DBService dbService)
    {
        _dbService = dbService;
    }

    public async void CreateUserAsync(User user)
    {
        await _dbService._connection.InsertAsync(user);
    }

    public async Task<List<User>?> CheckUserExistsAsync(User user)
    {
        var result = await _dbService._connection.Table<User>()
            .Where(x => x.Name == user.Name).ToListAsync();

        if (result.Count > 0)
        {
            return result;
        }

        return null;
    }
}