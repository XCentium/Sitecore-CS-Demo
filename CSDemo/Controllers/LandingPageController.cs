using CSDemo.Models.LandingPage;
using KeefePOC.Interfaces.Services;
using KeefePOC.Repositories;
using KeefePOC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSDemo.Controllers
{
    public class LandingPageController : Controller
    {
        IDataService dataService = new KeefeDataService(new DemoFacilityRepository(), new DemoProgramRepository(), new DemoInmateRepository());

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
            var result = dataService.GetPrograms(state);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}