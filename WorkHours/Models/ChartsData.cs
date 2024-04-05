using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkHours.Models
{
    public class ChartsData
    {
        public string WorkHours { get; set; }
        public int Value { get; set; }

        public ChartsData(string workedHours, int howMatch)
        {
            this.WorkHours = workedHours;
            this.Value = howMatch;
        }
    }
}
