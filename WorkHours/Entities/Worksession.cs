namespace WorkHours.Entities;

public class Worksession
{
    public Guid Id { get; set; }
    public int HoursWorked { get; set; }
    public Place Place { get; set; }
    public Guid PlaceId { get; set; }
    public DateTime CreatedTime { get; set; }
}