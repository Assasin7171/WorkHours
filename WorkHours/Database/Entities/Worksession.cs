using SQLite;

namespace WorkHours.Database.Entities;

public class Worksession
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int HoursWorked { get; set; }
    public int PlaceId { get; set; }
    public string Description { get; set; }
    [Ignore]
    public Place Place { get; set; }
    public DateTime CreatedTime { get; set; }
    public bool IsSettled { get; set; }
    public DateTime SettledDate { get; set; }
}