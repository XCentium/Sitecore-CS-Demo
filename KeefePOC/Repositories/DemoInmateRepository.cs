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
        List<Inmate> DemoInmates = new List<Inmate>();

        public DemoInmateRepository()
        {
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "One", Id = "123451", InmateNumber = "1", Tier = "Tier1", Block = "Block1", Cell = "Cell1" });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Two", Id = "123452", InmateNumber = "2", Tier = "Tier2", Block = "Block2", Cell = "Cell2" });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Three", Id = "123453", InmateNumber = "3", Tier = "Tier3", Block = "Block3", Cell = "Cell3" });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Four", Id = "123454", InmateNumber = "4", Tier = "Tier4", Block = "Block4", Cell = "Cell4" });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Five", Id = "123455", InmateNumber = "5", Tier = "Tier5", Block = "Block5", Cell = "Cell5" });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Six", Id = "123456", InmateNumber = "6", Tier = "Tier6", Block = "Block6", Cell = "Cell6WW" });

        }
        public Inmate GetInmate(string facilityId, string inmateNumber)
        {
            return DemoInmates.First();
        }

        public List<Inmate> GetInmates(string facilityId)
        {
            return DemoInmates;
        }

        public List<Inmate> SearchInmates(Inmate request)
        {
            return DemoInmates;
            //return DemoInmates.Where(i => i.InmateNumber == request.InmateNumber || i.FirstName == request.FirstName || i.MiddleName == request.MiddleName || i.LastName == request.LastName).ToList();
        }

        public List<Inmate> SearchInmates(string facilityId,Inmate request)
        {
            return DemoInmates;
            //return DemoInmates.Where(i => i.InmateNumber == request.InmateNumber || i.FirstName == request.FirstName || i.MiddleName == request.MiddleName || i.LastName == request.LastName).ToList();
        }
    }
}
