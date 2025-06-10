using SQLite;

namespace WorkHours.Database.Entities;

public class Place
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}