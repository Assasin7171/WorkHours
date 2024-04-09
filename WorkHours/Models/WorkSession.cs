using SQLite;

namespace WorkHours.Models;

[Table("WorkSessions")]
public class WorkSession
{
    private string? _description;
    private string? _location;
    private string? _workTime;

    [PrimaryKey, AutoIncrement, Column("Id")]
    public int Id { get; set; }

    [Column(nameof(WorkTime))]
    public string WorkTime
    {
        get => _workTime;
        set => _workTime = value;
    }

    [Column(nameof(Location))]
    public string Location
    {
        get => _location;
        set => _location = value;
    }

    [Column(nameof(Description))]
    public string Description
    {
        get => _description;
        set => _description = value;
    }

    [Column(nameof(DateOfWorkTime))]
    public DateTime DateOfWorkTime { get; set; }

    public WorkSession(string? workTime, string? location, DateTime dateOfWorkTime, string? description = "Brak")
    {
        _workTime = workTime;
        _location = location;
        _description = description;
        DateOfWorkTime = dateOfWorkTime;
    }

    public WorkSession()
    {
    }
}