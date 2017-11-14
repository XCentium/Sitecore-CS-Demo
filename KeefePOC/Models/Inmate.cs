using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeefePOC.Models
{
    public class Inmate
    {
        public string Id { get; set; }

        public string InmateNumber { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string FullName { get { return string.Concat(FirstName, " ", LastName); } }


        public List<string> Restrictions { get; set; } = new List<string>();
    }
}
