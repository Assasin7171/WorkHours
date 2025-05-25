namespace WorkHours.Db.Tables;

public class Worksession
{
    public Guid Id { get; set; }
    public int HoursWorked { get; set; }
    public Place Place { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.Now;
}