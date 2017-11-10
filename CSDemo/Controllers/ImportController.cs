using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeefePOC.Interfaces.Services;
using KeefePOC.Repositories;
using KeefePOC.Services;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Exceptions;
using System.Text.RegularExpressions;
using KeefePOC.Models;
namespace CSDemo.Controllers
{
    public class KeefImportController : Controller
    {
		IDataService dataService = new KeefeDataService(new DemoFacilityRepository(), new DemoProgramRepository(), new DemoInmateRepository());

		const string ProgramTemplateId = "{3D80C537-D7A4-4DBB-9C1B-B2B810F0A2C8}";
		const string FacilityTemplateId = "{E2232845-B8A5-4651-BA44-0CB1ED54AA9E}";
		const string ProgramLocation = "/sitecore/content/Global Configuration/Programs";
		const string FacilityLocation = "/sitecore/content/Global Configuration/Facilities";
		private Database MasterDatabase { get; }
		// GET: Import
		public JsonResult ImportPrograms()
        {
			var messages = new List<string>();
			var result = dataService.GetPrograms();
			try
			{
				var database = Sitecore.Configuration.Factory.GetDatabase("master");
				var folder = database.GetItem(ProgramLocation);

				TemplateItem template = database.GetTemplate(ProgramTemplateId);

				foreach (var program in result)
				{
					try
					{
						var existing = folder.Children
											.FirstOrDefault(c => c["Name"] == program.Name);

						// Only do additions. Don't mess with it, if it is already present
						if (existing != null)
						{
							continue;
						}

						using (new EditContext(folder, Sitecore.SecurityModel.SecurityCheck.Disable))
						{
							var name = Regex.Replace(program.Name, @"[.'/]", " ").Trim();
							
							var newItem = folder.Add(name, template);

							using (new EditContext(newItem))
							{
								newItem["ExternalID"] = program.ExternalId;
								newItem["IsActive"] = "1"; //program.IsActive == true ? "1": "0";
								newItem["Program Type"] = program.ProgramType.ToString();
								newItem["Name"] = program.Name;
								newItem["State"] = program.State;
							}
						}
					}
					catch (InvalidItemNameException e)
					{
						messages.Add($"Unable to create program {program.Name} due to error {e.Message}");
					}
				}
			}
			catch (UnauthorizedAccessException)
			{
				messages.Add("Unable to create new program configurations. Please make sure program folder is set to allow anonymous users to write to database");
			}


			return new JsonResult()
			{
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				Data = new ResponseModel()
				{
					IsSuccessful = true,
					Messages = messages
				}
			};


			//return Json(result, JsonRequestBehavior.AllowGet);
		}

		public JsonResult ImportFacilities()
		{
			var messages = new List<string>();
			var result = dataService.GetFacilities("");
			try
			{
				var database = Sitecore.Configuration.Factory.GetDatabase("master");
				var folder = database.GetItem(FacilityLocation);

				TemplateItem template = database.GetTemplate(FacilityTemplateId);

				foreach (var program in result)
				{
					try
					{
						var existing = folder.Children
											.FirstOrDefault(c => c["Name"] == program.Name);

						// Only do additions. Don't mess with it, if it is already present
						if (existing != null)
						{
							continue;
						}

						using (new EditContext(folder, Sitecore.SecurityModel.SecurityCheck.Disable))
						{
							var name = Regex.Replace(program.Name, @"[.'/]", " ").Trim();

							var newItem = folder.Add(name, template);

							using (new EditContext(newItem))
							{
								newItem["ExternalID"] = program.ExternalId;
								newItem["IsActive"] = "1"; //program.IsActive == true ? "1": "0";								
								newItem["Name"] = program.Name;
								newItem["Address Line 1"] = program.AddressLine1;
								newItem["Address Line 2"] = program.AddressLine2;
								newItem["City"] = program.City;
								newItem["State"] = program.State;
								newItem["Postal Code"] = program.Zipcode;
								newItem["Country"] = "USA";
								newItem["HIPPA Facility"] = program.IsHippa == true ? "1" : "0" ;
							}
						}
					}
					catch (InvalidItemNameException e)
					{
						messages.Add($"Unable to create facility {program.Name} due to error {e.Message}");
					}
				}
			}
			catch (UnauthorizedAccessException)
			{
				messages.Add("Unable to create new program configurations. Please make sure program folder is set to allow anonymous users to write to database");
			}


			return new JsonResult()
			{
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				Data = new ResponseModel()
				{
					IsSuccessful = true,
					Messages = messages
				}
			};


			//return Json(result, JsonRequestBehavior.AllowGet);
		}
		
	}
}