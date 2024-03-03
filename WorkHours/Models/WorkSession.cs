using SQLite;

namespace WorkHours.Models;

[Table("WorkSessions")]
public class WorkSession
{
    private string? _description;
    private string? _location;
    private string? _workTime;

    public WorkSession(string? workTime, string? location, string? description = null)
    {
        _workTime = workTime;
        _location = location;
        _description = description;
    }

    public WorkSession()
    {

    }

    [PrimaryKey, AutoIncrement, Column("Id")]
    public int Id { get; set; }

    [Column("WorkTime")]
    public string WorkTime
    {
        get => _workTime;
        set => _workTime = value;
    }

    [Column("WorkLocation")]
    public string Location
    {
        get => _location;
        set => _location = value;
    }

    [Column("Description")]
    public string Description
    {
        get => _description;
        set => _description = value;
    }
}