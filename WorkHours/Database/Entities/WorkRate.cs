using SQLite;

namespace WorkHours.Database.Entities;

public class WorkRate
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public decimal ValueRate { get; set; }
    public DateTime CreatedTime { get; set; }
}