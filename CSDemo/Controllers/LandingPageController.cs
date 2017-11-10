using CSDemo.Models.LandingPage;
using KeefePOC.Interfaces.Services;
using KeefePOC.Repositories;
using KeefePOC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeefePOC.Models;
using Sitecore.Data.Items;

namespace CSDemo.Controllers
{
    public class LandingPageController : Controller
    {
        IDataService dataService = new KeefeDataService(new DemoFacilityRepository(), new DemoProgramRepository(), new DemoInmateRepository());
		const string ProgramTemplateId = "{3D80C537-D7A4-4DBB-9C1B-B2B810F0A2C8}";
		const string FacilityTemplateId = "{E2232845-B8A5-4651-BA44-0CB1ED54AA9E}";
		const string ProgramLocation = "/sitecore/content/Global Configuration/Programs";
		const string FacilityLocation = "/sitecore/content/Global Configuration/Facilities";

		//public LandingPageController(IDataService dataService)
		//{
		//    this.dataService = dataService;
		//}
		// GET: LandingPage
		public ActionResult Home()
        {
            var states = dataService.GetStates();
            var model = new HomeViewModel(states);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChooseProgram(HomeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var states = dataService.GetStates();
                var refreshModel = new HomeViewModel(states);
                refreshModel.SelectedProgram = model.SelectedProgram;
                refreshModel.SelectedState = model.SelectedState;

                // TODO: populate error message

                return View(refreshModel);
            }

            return Content("TODO: Redirect to next page");
        }

        public ActionResult GetPrograms(string state)
        {

			// Get this from sitecore now.
			List<Program> programs = new List<Program>();
			var programItem = Sitecore.Context.Database.GetItem(ProgramLocation);


			//if null: check if the data is published to web db. Also add isActive check
			if (programItem != null)
			{
				foreach (Item program in programItem.Children)
				{
					if (program["State"] == state)
					{
						programs.Add(new Program() { Name = program["Name"] });
					}
				}
			}

			//var result = dataService.GetPrograms(state);
            return Json(programs, JsonRequestBehavior.AllowGet);
        }
    }
}