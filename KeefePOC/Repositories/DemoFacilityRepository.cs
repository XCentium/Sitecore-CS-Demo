using KeefePOC.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeefePOC.Models;
using KeefePOC.Models.Enumerations;

namespace KeefePOC.Repositories
{
    public class DemoFacilityRepository : IFacilityRepository
    {
        List<Facility> DemoFacilities = new List<Facility>();

        public DemoFacilityRepository()
        {
            DemoFacilities.Add(new Facility() { Name = "CA Hospital Facility 1", ExternalId = "CAHOSP1", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Santa Ana", State = "CA", Zipcode = "92705", FacilityType = FacilityType.Hospital });
            DemoFacilities.Add(new Facility() { Name = "CA Hospital Facility 2", ExternalId = "CAHOSP2", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "CA", Zipcode = "92705", FacilityType = FacilityType.Hospital });
            DemoFacilities.Add(new Facility() { Name = "CA Jail Facility 1", ExternalId = "CAJAIL1", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "CA", Zipcode = "92705", FacilityType = FacilityType.Jail });
            DemoFacilities.Add(new Facility() { Name = "CA Jail Facility 2", ExternalId = "CAJAIL2", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "CA", Zipcode = "92705", FacilityType = FacilityType.Jail });
            DemoFacilities.Add(new Facility() { Name = "CA Jail Facility 3", ExternalId = "CAJAIL3", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "CA", Zipcode = "12345", FacilityType = FacilityType.Jail });
            DemoFacilities.Add(new Facility() { Name = "CA Other Facility 1", ExternalId = "CAOTHER1", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "CA", Zipcode = "12345", FacilityType = FacilityType.Other });
            DemoFacilities.Add(new Facility() { Name = "OH Hospital Facility 1", ExternalId = "OHHOSP1", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "OH", Zipcode = "12345", FacilityType = FacilityType.Hospital });
            DemoFacilities.Add(new Facility() { Name = "OH Hospital Facility 2", ExternalId = "OHHOSP2", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "OH", Zipcode = "12345", FacilityType = FacilityType.Hospital });
            DemoFacilities.Add(new Facility() { Name = "OH Jail Facility 1", ExternalId = "OHJAIL1", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "OH", Zipcode = "12345", FacilityType = FacilityType.Jail });
            DemoFacilities.Add(new Facility() { Name = "OH Jail Facility 2", ExternalId = "OHJAIL2", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "OH", Zipcode = "12345", FacilityType = FacilityType.Jail });
        }

        public List<Facility> GetFacilities(string programId)
        {
            return DemoFacilities;
        }

        public Facility GetFacility(string facilityId)
        {
            return DemoFacilities.First();
        }
    }
}
