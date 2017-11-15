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
using KeefePOC.Models.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CSDemo.Controllers
{
    public class LandingPageController : Controller
    {
        IDataService dataService = new KeefeDataService(new DemoFacilityRepository(), new DemoProgramRepository(), new DemoInmateRepository());
        const string ProgramTemplateId = "{3D80C537-D7A4-4DBB-9C1B-B2B810F0A2C8}";
        const string FacilityTemplateId = "{E2232845-B8A5-4651-BA44-0CB1ED54AA9E}";
        const string ProgramLocation = "/sitecore/content/Global Configuration/Programs";
        const string FacilityLocation = "/sitecore/content/Global Configuration/Facilities";

        public ActionResult Home()
        {
            var states = dataService.GetStates();
            var model = new HomeViewModel(states);
            return View(model);
        }

        [HttpPost]
        public ActionResult SelectProgram(HomeViewModel model)
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

            var program = Sitecore.Context.Database.GetItem(model.SelectedProgram);
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
                var finalUrl = string.Concat(redirectUrl, "/inmate-selector?inmateId=", model.SelectedInmate);
                return Redirect(finalUrl);
            }
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
            var result = dataService.SearchInmates(search);

            var converted = JsonConvert.SerializeObject(result);
            return Content(converted, "application/json");
        }
    }
}