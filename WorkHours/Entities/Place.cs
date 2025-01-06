namespace WorkHours.Entities;

public class Place
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<WorkSession> WorkSessions { get; set; } = new List<WorkSession>();
}