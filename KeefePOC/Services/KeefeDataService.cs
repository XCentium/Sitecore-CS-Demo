using KeefePOC.Interfaces.Repositories;
using KeefePOC.Interfaces.Services;
using System.Collections.Generic;
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

        public Inmate GetInmate(string inmateId)
        {
            return inmateRepo.GetInmate(inmateId);
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

		public List<string> GetBlacklistedItemsForInmate(string inmateId)
		{
			return inmateRepo.GetBlacklistedItemsForInmate(inmateId);
		}

        public List<string> GetProductRestrictionsForInmate(string inmateId)
        {
            return inmateRepo.GetProductRestrictionsForInmate(inmateId);
        }

        public double GetCurrentQuarterOrderTotalWeightForInmate(string inmateId)
        {
            return inmateRepo.GetCurrentQuarterOrderTotalWeightForInmate(inmateId);
        }

        public decimal GetCurrentQuarterOrderTotalPriceForInmate(string inmateId)
        {
            return inmateRepo.GetCurrentQuarterOrderTotalPriceForInmate(inmateId);
        }
    }
}
