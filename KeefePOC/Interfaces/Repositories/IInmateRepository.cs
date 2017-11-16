using KeefePOC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeefePOC.Interfaces.Repositories
{
    public interface IInmateRepository
    {
        List<Inmate> GetInmates(string facilityId);
        Inmate GetInmate(string facilityId, string inmateNumber);
        List<Inmate> SearchInmates(Inmate request);
        List<Inmate> SearchInmates(string facilityId,Inmate request);
		List<string> GetBlacklistedItemsForInmate(string inmateId);
		
    }
}
