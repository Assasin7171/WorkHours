namespace WorkHours.Entities;

public class WorkSession
{
    public int Id { get; set; }
    public int? Hours { get; set; } = null;
    public DateTime DateTime { get; set; }
    public string? Descriptions { get; set; }
    
    //relacja w bazie
    public Place Place { get; set; }
    public int PlaceId { get; set; }
}