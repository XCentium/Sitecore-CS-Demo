using CSDemo.Models.Faq;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CSDemo.Controllers
{
	public class FaqController : Controller
	{
		const string FAQ_LOCATION = "/sitecore/content/Global Configuration/FAQs";
		const string VARIABLE_FAQ_LOCATION = "/sitecore/content/Global Configuration/Variable FAQs";

		public ActionResult List()
		{
			Item item = Sitecore.Context.Item;
			Sitecore.Data.Database contextDB = Sitecore.Configuration.Factory.GetDatabase(item.Database.Name);
			var siteName = Sitecore.Context.Site.ContentStartPath;

			var siteNode = contextDB.GetItem(siteName);

			var program = siteNode["Program"];
			var programItem = contextDB.GetItem(program);
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

			var varFaqItems = Sitecore.Context.Database.GetItem(VARIABLE_FAQ_LOCATION);
			foreach (Item faq in varFaqItems.GetChildren())
			{
				var faqModel = new FaqItem();
				faqModel.Question = faq["Question"];

				var answers = faq["Answers"];
				var values = HttpUtility.ParseQueryString(answers);

				var sb = new StringBuilder();

				foreach (string key in values.Keys)
				{
					var keys = key.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					if (keys.Length == 1)
					{
						var programValue = programItem[key];
						if (!string.IsNullOrEmpty(programValue))
						{
							var tempAnswer = values[key];
							tempAnswer = tempAnswer.Replace($"[{key}]", programValue);
							sb.Append(tempAnswer);
						}
					}
					else
					{

						string tempAnswer = values[key];

						foreach (var fieldName in keys)
						{
							var programValue = programItem[fieldName];
							if (string.IsNullOrEmpty(programValue))
							{
								tempAnswer = string.Empty;
								break;
							}
							else
							{

								tempAnswer = tempAnswer.Replace($"[{fieldName}]", programValue);
							}

						}


						sb.Append(tempAnswer);
					}
				}

				var answerText = sb.ToString();
				if (!string.IsNullOrEmpty(answerText))
				{
					faqModel.Answer = answerText;
					model.Faqs.Add(faqModel);
				}
			}

			return View(model);
		}
	}
}