using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkHours.Models
{
    public class ChartData
    {
        public string WorkHours { get; set; }
        public double Value { get; set; }

        public ChartData(string name, double howMatch)
        {
            this.WorkHours = name;
            this.Value = howMatch;
        }
    }
}
