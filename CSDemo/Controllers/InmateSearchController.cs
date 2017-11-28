using CSDemo.Models.InmateSearch;
using KeefePOC.Interfaces.Services;
using KeefePOC.Models;
using KeefePOC.Repositories;
using KeefePOC.Services;
using System.Web.Mvc;
using CSDemo.Helpers;

namespace CSDemo.Controllers
{
    public class InmateSearchController : Controller
    {
        private readonly IDataService _dataService = new KeefeDataService(new DemoFacilityRepository(), new DemoProgramRepository(), new DemoInmateRepository());
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

            if (string.IsNullOrEmpty(facilityId))
            {
                var inmate = _dataService.GetInmate(inmateId);
                model.SelectedInmate = inmate;
                model.SelectedFacility = new Facility();
            }
            else
            {
                var inmate = _dataService.GetInmate(facilityId, inmateId);
                model.SelectedInmate = inmate;
                var facility = Sitecore.Context.Database.GetItem(facilityId);
                model.SelectedFacility = new KeefePOC.Models.Facility(facility);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult SaveInmate(SelectInmateViewModel model)
        {
            // TODO: save inmate to session
            Inmate inmate;
            if (model.SelectedFacility == null || string.IsNullOrEmpty(model.SelectedFacility.ExternalId))
            {
                inmate = _dataService.GetInmate(model.SelectedInmateId);
				var facilityItem = FacilityHelper.GetFacilityByExternalId(inmate.AssociatedFacilityId);
				if (facilityItem != null)
				{
					model.SelectedFacility = new KeefePOC.Models.Facility(facilityItem);
				}

			}
            else
            {
                inmate = _dataService.GetInmate(model.SelectedFacility.ExternalId, model.SelectedInmateId);
            }

            FacilityHelper.SaveSelectedFacility(model.SelectedFacility);
            InmateHelper.SaveSelectedInmate(inmate);

            return Redirect("/");
        }
    }
}