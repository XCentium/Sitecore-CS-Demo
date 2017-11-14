using KeefePOC.Interfaces.Repositories;
using KeefePOC.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeefePOC.Models;


namespace KeefePOC.Services
{
    public class KeefeDataService : IDataService
    {
        readonly IFacilityRepository facilityRepo;
        readonly IProgramRepository programRepo;
        readonly IInmateRepository inmateRepo;

        public KeefeDataService(IFacilityRepository facilityRepository, IProgramRepository programRepository, IInmateRepository inmateRepository)
        {
            this.facilityRepo = facilityRepository;
            this.programRepo = programRepository;
            this.inmateRepo = inmateRepository;
        }

        public List<Facility> GetFacilities(string programId)
        {
            return facilityRepo.GetFacilities(programId);
        }

        public Facility GetFacility(string facilityId)
        {
            return facilityRepo.GetFacility(facilityId);
        }

        public Inmate GetInmate(string facilityId, string inmateId)
        {
            return inmateRepo.GetInmate(facilityId, inmateId);
        }
        
        public List<Inmate> GetInmates(string facilityId)
        {
            return inmateRepo.GetInmates(facilityId);
        }

        public Program GetProgram(string id)
        {
            return programRepo.GetProgram(id);
        }

        public List<Program> GetPrograms()
        {
            return programRepo.GetPrograms();
        }

        public List<Program> GetPrograms(string state)
        {
            return programRepo.GetPrograms(state);
        }

        public List<State> GetStates()
        {
            return programRepo.GetStates();
        }

        public List<Inmate> SearchInmates(Inmate searchRequest)
        {
            return inmateRepo.SearchInmates(searchRequest);
        }

        public List<Inmate> SearchInmates(string facilityId, Inmate searchRequest)
        {
            return inmateRepo.SearchInmates(facilityId, searchRequest);
        }
    }
}
