using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Canteen
    {

        public int Id { get; set; }
    
        public string Name { get; set; }
        public City City { get; set; }
        public String PostalCode { get; set;}

        public String Street { get; set; }

        public int HouseNumber { get; set; }

        public bool ServesWarmMeals { get; set; }

    }
}
