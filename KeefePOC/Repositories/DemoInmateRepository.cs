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
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "One", Id = "123451", InmateNumber = "123451", Tier = "Tier1", Block = "Block1", Cell = "Cell1", AssociatedFacilityId = "CAHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Two", Id = "123452", InmateNumber = "123452", Tier = "Tier2", Block = "Block2", Cell = "Cell2", AssociatedFacilityId = "CAHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Three", Id = "123453", InmateNumber = "123453", Tier = "Tier3", Block = "Block3", Cell = "Cell3", AssociatedFacilityId = "CAJAIL1", CurrentQuarterTotalOrderPrice = 100, CurrentQuarterTotalOrderWeight = 100 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Four", Id = "123454", InmateNumber = "123454", Tier = "Tier4", Block = "Block4", Cell = "Cell4", AssociatedFacilityId = "OHHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Five", Id = "123455", InmateNumber = "123455", Tier = "Tier5", Block = "Block5", Cell = "Cell5", AssociatedFacilityId = "CAHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Six", Id = "123456", InmateNumber = "123456", Tier = "Tier6", Block = "Block6", Cell = "Cell6WW", AssociatedFacilityId = "CAJAIL1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });

            DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Test", Id = "700001", InmateNumber = "700001", Tier = "Tier1", Block = "Block1", Cell = "Cell1", AssociatedFacilityId = "CAHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Test", Id = "700002", InmateNumber = "700002", Tier = "Tier1", Block = "Block1", Cell = "Cell1", AssociatedFacilityId = "CAHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Test", Id = "700003", InmateNumber = "700003", Tier = "Tier1", Block = "Block1", Cell = "Cell1", AssociatedFacilityId = "CAHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "Test", Id = "700004", InmateNumber = "700004", Tier = "Tier1", Block = "Block1", Cell = "Cell1", AssociatedFacilityId = "CAHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", LastName = "700005", Id = "700005", InmateNumber = "700005", Tier = "Tier4", Block = "Block4", Cell = "Cell4", AssociatedFacilityId = "OHHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
		}

		public Inmate GetInmate(string inmateNumber)
		{
			return DemoInmates.First(i => i.InmateNumber == inmateNumber);
		}

		public List<string> GetBlacklistedItemsForInmate(string inmateId)
		{
			var model = new BlackListedProductsForInmateModel();
			model.PopulateSampleData();

			foreach (var item in model.Blacklist)
			{
				if (string.Equals(item.InmateId, inmateId))
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

		public double GetCurrentQuarterOrderTotalWeightForInmate(string inmateId)
		{
			return DemoInmates.Where(i => i.InmateNumber == inmateId).Select(i => i.CurrentQuarterTotalOrderWeight).FirstOrDefault();
		}

		public decimal GetCurrentQuarterOrderTotalPriceForInmate(string inmateId)
		{
			return DemoInmates.Where(i => i.InmateNumber == inmateId).Select(i => i.CurrentQuarterTotalOrderPrice).FirstOrDefault();
		}

		public Inmate GetInmate(string facilityId, string inmateNumber)
		{
			return DemoInmates.First(i => i.InmateNumber == inmateNumber && i.AssociatedFacilityId == facilityId);
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
			if (!string.IsNullOrEmpty(request.InmateNumber))
				return DemoInmates.Where(i => i.InmateNumber.Contains(request.InmateNumber)).ToList();
			else
				return DemoInmates.Where(i => (i.InmateNumber.Contains(request.InmateNumber) || (i.FirstName == request.FirstName && i.MiddleName == request.MiddleName && i.LastName == request.LastName)) && i.AssociatedFacilityId == facilityId).ToList();
		}
	}
}
