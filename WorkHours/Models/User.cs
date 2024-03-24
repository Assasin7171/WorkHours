using SQLite;

namespace WorkHours.Models;

[Table("Users")]
public class User
{
    [PrimaryKey, AutoIncrement, Column("Id")]
    public int Id { get; set; }
    [Column("Name")]
    public string Name { get; set; }
}