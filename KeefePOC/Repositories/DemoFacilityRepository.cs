using KeefePOC.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeefePOC.Models;

namespace KeefePOC.Repositories
{
    public class DemoFacilityRepository : IFacilityRepository
    {
        List<Facility> DemoFacilities = new List<Facility>();

        public DemoFacilityRepository()
        {
            DemoFacilities.Add(new Facility() { Name = "Facility 1", ExternalId = "1", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Santa Ana", State = "CA", Zipcode = "92705" });
            DemoFacilities.Add(new Facility() { Name = "Facility 2", ExternalId = "2", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "CA", Zipcode = "92705" });
            DemoFacilities.Add(new Facility() { Name = "Facility 3", ExternalId = "3", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "CA", Zipcode = "92705" });
            DemoFacilities.Add(new Facility() { Name = "Facility 4", ExternalId = "4", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "Ca", Zipcode = "92705" });
            DemoFacilities.Add(new Facility() { Name = "Facility 5", ExternalId = "5", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "TX", Zipcode = "12345" });
            DemoFacilities.Add(new Facility() { Name = "Facility 6", ExternalId = "6", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "TX", Zipcode = "12345" });
            DemoFacilities.Add(new Facility() { Name = "Facility 7", ExternalId = "7", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "TX", Zipcode = "12345" });
            DemoFacilities.Add(new Facility() { Name = "Facility 8", ExternalId = "8", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "TX", Zipcode = "12345" });
            DemoFacilities.Add(new Facility() { Name = "Facility 9", ExternalId = "9", AddressLine1 = "123 Fake St", AddressLine2 = "Line 2", City = "Anytown", State = "TX", Zipcode = "12345" });
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
