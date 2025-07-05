namespace WorkHours.Models;

public class MonthEntry
{
    public int Year { get; set; }
    public int Month { get; set; }
    
    public string Display => $"{new DateTime(Year, Month, 1):MMMM yyyy}";

    public DateTime Start => new DateTime(Year, Month, 1);
    public DateTime End => Start.AddMonths(1).AddDays(-1);
}