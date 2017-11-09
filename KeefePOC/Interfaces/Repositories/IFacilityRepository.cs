using KeefePOC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeefePOC.Interfaces.Repositories
{
    public interface IFacilityRepository
    {
        List<Facility> GetFacilities(string programId);

        Facility GetFacility(string facilityId);
    }
}
