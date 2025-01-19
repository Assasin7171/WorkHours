using WorkHours.Entities;

namespace WorkHours.Models;

public class WorkSessionModel
{
    public int Hours { get; set; }
    public DateTime DateTime { get; set; }
    public string? Descriptions { get; set; }
    public Place Place { get; set; }
    
}