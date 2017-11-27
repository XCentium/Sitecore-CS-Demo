using KeefePOC.Models.Enumerations;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeefePOC.Models
{
    public class Facility
    {
        public Facility() { }

        public Facility(Item facilityItem)
        {
            this.Id = facilityItem.ID.Guid.ToString();
            this.Name = facilityItem["Name"];
            this.ExternalId = facilityItem["External ID"];
            //this.FacilityType = (FacilityType)Enum.Parse(typeof(FacilityType), facilityItem["Facility Type"]); TODO: fix
            Sitecore.Data.Fields.CheckboxField hippa = facilityItem.Fields["HIPPA Facility"];
            this.IsHippa = hippa.Checked;
        }

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
