using CSDemo.Models.InmateSearch;
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
    public class InmateSearchController : Controller
    {
        IDataService dataService = new KeefeDataService(new DemoFacilityRepository(), new DemoProgramRepository(), new DemoInmateRepository());
        const string ProgramTemplateId = "{3D80C537-D7A4-4DBB-9C1B-B2B810F0A2C8}";
        const string FacilityTemplateId = "{E2232845-B8A5-4651-BA44-0CB1ED54AA9E}";
        const string ProgramLocation = "/sitecore/content/Global Configuration/Programs";
        const string FacilityLocation = "/sitecore/content/Global Configuration/Facilities";

        public ActionResult Home(string facilityId)
        {
            var model = new InmateSearchViewModel();
            var facility = Sitecore.Context.Database.GetItem(facilityId);
            model.SelectedFacility = new KeefePOC.Models.Facility(facility);
            return View(model);
        }

        [HttpPost]
        public ActionResult SearchInmate(InmateSearchViewModel model)
        {
            // TODO: validate inmate


            var url = string.Concat("/inmate-confirmation?facilityId=", model.SelectedFacility.Id, "&inmateId=", model.SelectedInmateId);
            return Redirect(url);
        }

        public ActionResult SelectInmate(string facilityId, string inmateId)
        {
            var model = new SelectInmateViewModel();

            var inmate = dataService.GetInmate(facilityId, inmateId);
            model.SelectedInmate = inmate;

            var facility = Sitecore.Context.Database.GetItem(facilityId);
            model.SelectedFacility = new KeefePOC.Models.Facility(facility);

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveInmate(SelectInmateViewModel model)
        {
            // TODO: save inmate to session
            var inmate = dataService.GetInmate(model.SelectedFacility.Id, model.SelectedInmateId);
            Session["SELECTED_INMATE"] = inmate;

            return Redirect("/");
        }
    }
}