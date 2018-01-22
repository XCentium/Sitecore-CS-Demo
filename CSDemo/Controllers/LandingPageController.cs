using CSDemo.Models.LandingPage;
using KeefePOC.Interfaces.Services;
using KeefePOC.Repositories;
using KeefePOC.Services;
using System.Collections.Generic;
using System.Web.Mvc;
using KeefePOC.Models;
using Sitecore.Data.Items;
using Newtonsoft.Json;
using CSDemo.Helpers;
using CSDemo.Models;

namespace CSDemo.Controllers
{
	public class LandingPageController : Controller
	{
		private readonly IDataService _dataService = new KeefeDataService(new DemoFacilityRepository(), new DemoProgramRepository(), new DemoInmateRepository());
		const string ProgramTemplateId = "{3D80C537-D7A4-4DBB-9C1B-B2B810F0A2C8}";
		const string FacilityTemplateId = "{E2232845-B8A5-4651-BA44-0CB1ED54AA9E}";
		const string StatesLocation = "/sitecore/content/Global Configuration/States";
		const string ProgramLocation = "/sitecore/content/Global Configuration/Programs";
		const string FacilityLocation = "/sitecore/content/Global Configuration/Facilities";

		public ActionResult Home()
		{
			var states = GetStates();
			var model = new HomeViewModel(states);
			return View(model);
		}

		[HttpPost]
		public ActionResult SelectProgram(HomeViewModel model)
		{
			if (!ModelState.IsValid)
			{
				var states = _dataService.GetStates();
				var refreshModel = new HomeViewModel(states)
				{
					SelectedProgram = model.SelectedProgram,
					SelectedState = model.SelectedState
				};

				// TODO: populate error message

				return View(refreshModel);
			}

			var program = Sitecore.Context.Database.GetItem(model.SelectedProgram);

			//save program
			var selectedProgram = GlassHelper.Cast<ProgramModel>(program);
			ProgramHelper.SaveSelectedProgram(selectedProgram);

			Sitecore.Data.Fields.LinkField linkField = program.Fields["Program Home Page"];
			var redirectUrl = linkField.GetFriendlyUrl();

			if (!string.IsNullOrEmpty(model.SelectedHospital))
			{
				var finalUrl = string.Concat(redirectUrl, "/inmate-selector?facilityId=", model.SelectedHospital);
				return Redirect(finalUrl);
			}

			if (!string.IsNullOrEmpty(model.SelectedInstitution))
			{
				var finalUrl = string.Concat(redirectUrl, "/inmate-selector?facilityId=", model.SelectedInstitution);
				return Redirect(finalUrl);
			}
			else
			{
				var finalUrl = string.Concat(redirectUrl, "/inmate-confirmation?inmateId=", model.SelectedInmate);
				return Redirect(finalUrl);
			}
		}

		private List<State> GetStates()
		{
			// Get this from sitecore now.
			var states = new List<State>();
			var statesItem = Sitecore.Context.Database.GetItem(StatesLocation);

			//if null: check if the data is published to web db. Also add isActive check
			if (statesItem != null)
			{
				foreach (Item state in statesItem.Children)
				{
					var model = new State()
					{
						Code = state["State"],
						Name = state.Name
					};
					states.Add(model);

				}
			}

			return states;
		}

		public ActionResult GetPrograms(string state)
		{
			// Get this from sitecore now.
			var programs = new List<Program>();
			var programItem = Sitecore.Context.Database.GetItem(ProgramLocation);

			//if null: check if the data is published to web db. Also add isActive check
			if (programItem != null)
			{
				foreach (Item program in programItem.Children)
				{
					if (program["State"] == state)
					{
						var model = new Program(program);
						programs.Add(model);
					}
				}
			}

			var converted = JsonConvert.SerializeObject(programs);
			return Content(converted, "application/json");
		}

		public ActionResult GetProgramFacilities(string programId)
		{
			var program = Sitecore.Context.Database.GetItem(programId);
			Sitecore.Data.Fields.MultilistField linkField = program.Fields["Facilities"];

			var facilityList = new List<Facility>();
			foreach (var item in linkField.GetItems())
			{
				var model = new Facility(item);
				facilityList.Add(model);
			}

			var converted = JsonConvert.SerializeObject(facilityList);
			return Content(converted, "application/json");
		}

		public ActionResult SearchInmates(Inmate search)
		{
			List<Inmate> result;

			var facilityId = search.AssociatedFacilityId;

			if (!string.IsNullOrEmpty(facilityId))
			{
				var facilityItem = Sitecore.Context.Database.GetItem(facilityId);
				if (facilityItem != null)
				{
					var facility = new Facility(facilityItem);
					search.AssociatedFacilityId = facility.ExternalId;
				}

				result = _dataService.SearchInmates(search.AssociatedFacilityId, search);
			}
			else
			{
				result = _dataService.SearchInmates(search);
			}



			var converted = JsonConvert.SerializeObject(result);
			return Content(converted, "application/json");
		}
	}
}