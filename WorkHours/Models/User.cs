using SQLite;

namespace WorkHours.Models;

[Table("Users")]
public class User
{
    public User()
    {
        CreatedTime = DateTime.Today;
        IsLogged = false;
    }

    [PrimaryKey]
    [AutoIncrement]
    [Column(nameof(Id))]
    public int Id { get; set; }

    [Column(nameof(Name))] public string Name { get; set; }

    [Column(nameof(CreatedTime))] public DateTime CreatedTime { get; set; }

    [Column(nameof(IsLogged))] public bool IsLogged { get; set; }
}