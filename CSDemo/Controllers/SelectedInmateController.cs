using CSDemo.Helpers;
using CSDemo.Models;
using CSDemo.Models.SelectedInmate;
using KeefePOC.Interfaces.Services;
using KeefePOC.Models;
using KeefePOC.Repositories;
using KeefePOC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSDemo.Controllers
{
	public class SelectedInmateController : Controller
	{
		IDataService dataService = new KeefeDataService(new DemoFacilityRepository(), new DemoProgramRepository(), new DemoInmateRepository());
		const string ProgramTemplateId = "{3D80C537-D7A4-4DBB-9C1B-B2B810F0A2C8}";
		const string FacilityTemplateId = "{E2232845-B8A5-4651-BA44-0CB1ED54AA9E}";
		const string ProgramLocation = "/sitecore/content/Global Configuration/Programs";
		const string FacilityLocation = "/sitecore/content/Global Configuration/Facilities";

		public ActionResult Home()
		{
			var inmate = InmateHelper.GetSelectedInmate();
			if (inmate == null)
			{
				return View("Empty");
			}

			var model = new SelectedInmateViewModel();
			model.SelectedInmate = inmate;

			var facility = FacilityHelper.GetFacilityByExternalId(inmate.AssociatedFacilityId);
			if (facility != null)
			{
				var facilityModel = GlassHelper.Cast<FacilityModel>(facility);
				model.SelectedInmate.IsHippa = facilityModel?.IsHippa ?? false;
				model.FacilityName = facility.DisplayName;
			}

			return View(model);
		}

		[HttpPost]
		public ActionResult CancelOrder()
		{
			InmateHelper.SaveSelectedInmate(null);
			FacilityHelper.SaveSelectedFacility(null);

			// TODO: Clear cart

			return Redirect("/");
		}
	}
}