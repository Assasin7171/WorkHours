using SQLite;

namespace WorkHours.Models;

[Table("Users")]
public class User
{
    [PrimaryKey, AutoIncrement, Column("Id")]
    public int Id { get; set; }
    [Column("Name")]
    public string Name { get; set; }
    [Column("Password")]
    public string Password { get; set; }
    [Column("Email")]
    public string Email { get; set; }
}