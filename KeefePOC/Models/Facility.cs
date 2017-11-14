using KeefePOC.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeefePOC.Models
{
    public class Facility
    {
        public string Id { get; set; }
        public string Name { get; set; }
		public string ExternalId { get; set; }
        public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
		public string Country { get; set; }
        public FacilityType FacilityType { get; set; }
		public bool IsHippa { get; set; }
    }
}
