using KeefePOC.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeefePOC.Models;

namespace KeefePOC.Repositories
{
    public class DemoInmateRepository : IInmateRepository
    {
        public Inmate GetInmate(string facilityId, string programId)
        {
            throw new NotImplementedException();
        }

        public List<Inmate> GetInmates(string facilityId)
        {
            throw new NotImplementedException();
        }
    }
}
