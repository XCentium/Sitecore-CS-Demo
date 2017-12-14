using CSDemo.Models.Faq;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSDemo.Controllers
{
	public class FaqController : Controller
	{
		const string FAQ_LOCATION = "/sitecore/content/Global Configuration/FAQs";
		public ActionResult List()
		{
			Item item = Sitecore.Context.Item;
			Sitecore.Data.Database contextDB = Sitecore.Configuration.Factory.GetDatabase(item.Database.Name);
			var siteName = Sitecore.Context.Site.ContentStartPath;

			var siteNode = contextDB.GetItem(siteName);

			var program = siteNode["Program"];
			Debug.WriteLine("Current Program: " + program);

			var model = new FaqViewModel();

			var faqItem = Sitecore.Context.Database.GetItem(FAQ_LOCATION);
			foreach (Item faq in faqItem.GetChildren())
			{
				Debug.WriteLine("FAQ Program: " + faq["Program"]);
				if (faq["All Programs"] == "1" || faq["Program"].Contains(program))
				{

					var faqModel = new FaqItem();
					faqModel.Question = faq["Question"];
					faqModel.Answer = faq["Answer"];
					model.Faqs.Add(faqModel);
				}
				else
				{
					//var faqModel = new FaqItem();
					//faqModel.Question = "SKIPPED: " + faq["Question"];
					//faqModel.Answer = faq["Answer"];
					//model.Faqs.Add(faqModel);
				}
			}

			return View(model);
		}
	}
}