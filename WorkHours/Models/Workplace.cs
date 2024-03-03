using SQLite;

namespace WorkHours.Models
{
    [Table("Workplace")]
    public class Workplace
    {
        private string _name;

        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        [Column("Name")]
        public string Name
        {
            get => _name;
            set => _name = value;
        }
    }
}