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
        List<State> GetStates();

        List<Program> GetPrograms();
        List<Program> GetPrograms(string state);
        Program GetProgram(string id);

        List<Facility> GetFacilities(string programId);
        Facility GetFacility(string facilityId);

        List<Inmate> GetInmates(string facilityId);

        Inmate GetInmate(string inmateNumber);
        Inmate GetInmate(string facilityId, string inmateNumber);
        List<Inmate> SearchInmates(Inmate searchRequest);
        List<Inmate> SearchInmates(string facilityId, Inmate searchRequest);
    }
}
