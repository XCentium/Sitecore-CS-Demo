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
        Inmate GetInmate(string facilityId, int inmateNumber);
        List<Inmate> SearchInmates(string facilityId,Inmate request);
    }
}
