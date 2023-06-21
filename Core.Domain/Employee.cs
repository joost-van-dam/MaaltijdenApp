using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Employee : Person
    {
        public int EmployeeNumber { get; set; }
        public Canteen Canteen { get; set; }
    }
}
