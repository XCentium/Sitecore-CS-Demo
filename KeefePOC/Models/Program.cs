using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeefePOC.Models
{
    public class Program
    {
        public string Name { get; set; }

        public Guid CatalogId { get; set; }

        public double WeightLimit { get; set; }
        public List<object> PurchaserWhitelist { get; set; } 

        public List<Facility> Facilities { get; set; }
    }
}
