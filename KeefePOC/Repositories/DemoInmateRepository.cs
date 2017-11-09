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
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "One", Id = "123451", InmateNumber = 1 });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Two", Id = "123452", InmateNumber = 2 });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Three", Id = "123453", InmateNumber = 3 });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Four", Id = "123454", InmateNumber = 4 });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Five", Id = "123455", InmateNumber = 5 });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Six", Id = "123456", InmateNumber = 6 });

        }
        public Inmate GetInmate(string facilityId,int inmateNumber)
        {
            return DemoInmates.FirstOrDefault(n => n.InmateNumber == inmateNumber);
        }

        public List<Inmate> GetInmates(string facilityId)
        {
            return DemoInmates;
        }

        public List<Inmate> SearchInmates(string facilityId,Inmate request)
        {
            return DemoInmates.Where(i => i.InmateNumber == request.InmateNumber || i.FirstName == request.FirstName || i.MiddleName == request.MiddleName || i.LastName == request.LastName).ToList();
        }
    }
}
