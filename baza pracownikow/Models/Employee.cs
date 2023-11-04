using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baza_pracownikow.Models
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESEL { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();

        public override string ToString()
        {
            return $"{FirstName[0]}, {LastName}";
        }
    }
}
