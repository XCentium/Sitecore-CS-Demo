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
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "CA", LastName = "One", Id = "123451", InmateNumber = "123451", Tier = "Tier1", Block = "Block1", Cell = "Cell1", AssociatedFacilityId = "CAHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "CA", LastName = "Two", Id = "123452", InmateNumber = "123452", Tier = "Tier2", Block = "Block2", Cell = "Cell2", AssociatedFacilityId = "CAHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "CA", LastName = "Three", Id = "123453", InmateNumber = "123453", Tier = "Tier3", Block = "Block3", Cell = "Cell3", AssociatedFacilityId = "CAJAIL3", CurrentQuarterTotalOrderPrice = 100, CurrentQuarterTotalOrderWeight = 100 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "CA", LastName = "Four", Id = "123454", InmateNumber = "123454", Tier = "Tier4", Block = "Block4", Cell = "Cell4", AssociatedFacilityId = "CAHOSP2", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "CA", LastName = "Five", Id = "123455", InmateNumber = "123455", Tier = "Tier5", Block = "Block5", Cell = "Cell5", AssociatedFacilityId = "CAHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "CA", LastName = "Six", Id = "123456", InmateNumber = "123456", Tier = "Tier6", Block = "Block6", Cell = "Cell6WW", AssociatedFacilityId = "CAJAIL1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "CA", LastName = "Seven", Id = "123457", InmateNumber = "123457", Tier = "Tier7", Block = "Block7", Cell = "Cell7", AssociatedFacilityId = "CAJAIL4", CurrentQuarterTotalOrderPrice = 100, CurrentQuarterTotalOrderWeight = 100 });

			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "OH", LastName = "One", Id = "700001", InmateNumber = "700001", Tier = "Tier1", Block = "Block1", Cell = "Cell1", AssociatedFacilityId = "OHHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "OH", LastName = "Two", Id = "700002", InmateNumber = "700002", Tier = "Tier1", Block = "Block1", Cell = "Cell1", AssociatedFacilityId = "OHHOSP2", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "OH", LastName = "Three", Id = "700003", InmateNumber = "700003", Tier = "Tier1", Block = "Block1", Cell = "Cell1", AssociatedFacilityId = "OHHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "OH", LastName = "Four", Id = "700004", InmateNumber = "700004", Tier = "Tier1", Block = "Block1", Cell = "Cell1", AssociatedFacilityId = "OHHOSP1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "OH", LastName = "Five", Id = "700005", InmateNumber = "700005", Tier = "Tier4", Block = "Block4", Cell = "Cell4", AssociatedFacilityId = "OHJAIL1", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });

			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "MI", LastName = "One", Id = "900001", InmateNumber = "900001", Tier = "Tier1", Block = "Block1", Cell = "Cell1", AssociatedFacilityId = "21903", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "MI", LastName = "Two", Id = "900002", InmateNumber = "900002", Tier = "Tier1", Block = "Block1", Cell = "Cell1", AssociatedFacilityId = "21903", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "MI", LastName = "Three", Id = "900003", InmateNumber = "900003", Tier = "Tier1", Block = "Block1", Cell = "Cell1", AssociatedFacilityId = "21903", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "MI", LastName = "Four", Id = "900004", InmateNumber = "900004", Tier = "Tier1", Block = "Block1", Cell = "Cell1", AssociatedFacilityId = "21903", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
			DemoInmates.Add(new Inmate() { FirstName = "Inmate", MiddleName = "MI", LastName = "Five", Id = "900005", InmateNumber = "900005", Tier = "Tier4", Block = "Block4", Cell = "Cell4", AssociatedFacilityId = "21903", CurrentQuarterTotalOrderPrice = 10, CurrentQuarterTotalOrderWeight = 10 });
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
			return DemoInmates.FirstOrDefault(i => i.InmateNumber == inmateNumber && i.AssociatedFacilityId == facilityId);
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
			var results = DemoInmates.AsQueryable();

			if (!string.IsNullOrEmpty(request.FirstName))
				results = results.Where(i => i.FirstName.ToLower().Contains(request.FirstName));

			if (!string.IsNullOrEmpty(request.MiddleName))
				results = results.Where(i => i.MiddleName.ToLower().Contains(request.MiddleName));

			if (!string.IsNullOrEmpty(request.LastName))
				results = results.Where(i => i.LastName.ToLower().Contains(request.LastName));

			if (!string.IsNullOrEmpty(request.InmateNumber))
				results = results.Where(i => i.InmateNumber.Contains(request.InmateNumber));

			if (!string.IsNullOrEmpty(request.AssociatedFacilityId))
				results = results.Where(i => i.AssociatedFacilityId == facilityId);

			return results.ToList();
		}

		public List<string> GetInmateWhitelist(string inmateId)
		{
			if (inmateId == "123457")
			{
				return new List<string>
				{
					"inmate7@CAJAIL4.com",
					"testtwo@site.com",
					"john@site.com"
				};
			}
			else
				return new List<string>();
		}
	}
}
