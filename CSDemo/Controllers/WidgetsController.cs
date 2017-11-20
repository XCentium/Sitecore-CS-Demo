using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using CSDemo.Models.Widgets;
using Sitecore.Mvc.Controllers;
using Sitecore.Data;

namespace CSDemo.Controllers
{
	public class WidgetsController : SitecoreController
	{
		// GET: Widgets
		public ActionResult ExtraContentWidget()
		{
			var model = new ExtraContentWidgetModel();
			try
			{
				var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;

				if(dataSourceId != null)
				{
					var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);

					model.ExtraContent = dataSource["Content"];
				}				

			}
			catch (Exception ex)
			{
				//LogManager<ILogProvider>.Error(ex, this);
			}

			return View(model);
		}
	}
	
}