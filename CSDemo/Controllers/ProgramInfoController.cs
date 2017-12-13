using CSDemo.Models.ProgramInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSDemo.Controllers
{
	public class ProgramInfoController : Controller
	{
		public ActionResult Message()
		{
			var item = Sitecore.Context.Item;
			Sitecore.Data.Database contextDB = Sitecore.Configuration.Factory.GetDatabase(item.Database.Name);
			var siteName = Sitecore.Context.Site.ContentStartPath;
			var siteNode = contextDB.GetItem(siteName);
			var programId = siteNode["Program"];
			var program = contextDB.GetItem(programId);

			var model = new MessageViewModel();
			model.Message = program["Program Message"];
			Sitecore.Data.Fields.ImageField linkField = program.Fields["Program Image"];
			
			model.ImageSrc = Sitecore.Resources.Media.MediaManager.GetMediaUrl(linkField.MediaItem);
			return View(model);
		}
	}
}