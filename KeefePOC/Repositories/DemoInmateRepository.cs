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
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "One", Id = "123451", InmateNumber = "123451", Tier = "Tier1", Block = "Block1", Cell = "Cell1" });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Two", Id = "123452", InmateNumber = "123452", Tier = "Tier2", Block = "Block2", Cell = "Cell2" });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Three", Id = "123453", InmateNumber = "123453", Tier = "Tier3", Block = "Block3", Cell = "Cell3" });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Four", Id = "123454", InmateNumber = "123454", Tier = "Tier4", Block = "Block4", Cell = "Cell4" });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Five", Id = "123455", InmateNumber = "123455", Tier = "Tier5", Block = "Block5", Cell = "Cell5" });
            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Six", Id = "123456", InmateNumber = "123456", Tier = "Tier6", Block = "Block6", Cell = "Cell6WW" });
        }

        public Inmate GetInmate(string inmateNumber)
        {
            return DemoInmates.First();
        }

		public List<string> GetBlacklistedItemsForInmate(string inmateId)
		{
			BlackListedProductsForInmateModel model = new BlackListedProductsForInmateModel();
			model.PopulateSampleData();

			foreach(var item in model.Blacklist)
			{
				if(string.Equals(item.InmateId, inmateId))
				{
					return item.Products;
				}
			}

			return new List<string>();

		}

        public List<string> GetProductRestrictionsForInmate(string inmateId)
        {
            var model = new InmateProductRestrictions();
            model.PopulateSampleData(inmateId);

            return model.Restrictions;
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

        public List<Inmate> SearchInmates(string facilityId, Inmate request)
        {
            return DemoInmates;
            //return DemoInmates.Where(i => i.InmateNumber == request.InmateNumber || i.FirstName == request.FirstName || i.MiddleName == request.MiddleName || i.LastName == request.LastName).ToList();
        }
    }
}
