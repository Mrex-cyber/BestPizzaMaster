using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaMaster
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Ages { get; set; }
        public string Position { get; set; }
        public double Salary { get; set; }
        public Location Location { get; set; }
    }
}
