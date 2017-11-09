using KeefePOC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeefePOC.Interfaces.Services
{
    public interface IDataService
    {
        List<Program> GetPrograms();
        List<Program> GetPrograms(string state);
        Program GetProgram(string id);

        List<Facility> GetFacilities(string programId);
        Facility GetFacility(string facilityId);

        List<Inmate> GetInmates(string facilityId);
        Inmate GetInmate(string facilityId, int inmateNumber);
        List<Inmate> SearchInmates(string facilityId, Inmate searchRequest);
    }
}
