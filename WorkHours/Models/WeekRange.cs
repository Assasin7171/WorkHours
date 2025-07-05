namespace WorkHours.Models;

public class WeekRange
{
    public DateTime Start { get; set; }
    public string Display => $"{Start:dd.MM} - {Start.AddDays(6):dd.MM}";
}