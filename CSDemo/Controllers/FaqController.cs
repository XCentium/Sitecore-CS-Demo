using CSDemo.Models.Faq;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
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
			var model = new FaqViewModel();

			var faqItem = Sitecore.Context.Database.GetItem(FAQ_LOCATION);
			foreach (Item faq in faqItem.GetChildren())
			{
				var faqModel = new FaqItem();
				faqModel.Question = faq["Question"];
				faqModel.Answer = faq["Answer"];
				model.Faqs.Add(faqModel);
			}

			return View(model);
		}
	}
}