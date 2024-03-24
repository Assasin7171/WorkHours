using SQLite;

namespace WorkHours.Models
{
    [Table("Workplace")]
    public class Workplace
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        [Column("Name")]
        public string Name { get; set; }
    }
}