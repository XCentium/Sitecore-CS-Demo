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
            DemoFacilities.Add(new Facility() { Name = "Facility 1", Address = "123 Fake St", City = "Anytown", State = "TX", Zipcode = "12345" });
            DemoFacilities.Add(new Facility() { Name = "Facility 2", Address = "123 Fake St", City = "Anytown", State = "TX", Zipcode = "12345" });
            DemoFacilities.Add(new Facility() { Name = "Facility 3", Address = "123 Fake St", City = "Anytown", State = "TX", Zipcode = "12345" });
            DemoFacilities.Add(new Facility() { Name = "Facility 4", Address = "123 Fake St", City = "Anytown", State = "TX", Zipcode = "12345" });
            DemoFacilities.Add(new Facility() { Name = "Facility 5", Address = "123 Fake St", City = "Anytown", State = "TX", Zipcode = "12345" });
            DemoFacilities.Add(new Facility() { Name = "Facility 6", Address = "123 Fake St", City = "Anytown", State = "TX", Zipcode = "12345" });
            DemoFacilities.Add(new Facility() { Name = "Facility 7", Address = "123 Fake St", City = "Anytown", State = "TX", Zipcode = "12345" });
            DemoFacilities.Add(new Facility() { Name = "Facility 8", Address = "123 Fake St", City = "Anytown", State = "TX", Zipcode = "12345" });
            DemoFacilities.Add(new Facility() { Name = "Facility 9", Address = "123 Fake St", City = "Anytown", State = "TX", Zipcode = "12345" });
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
