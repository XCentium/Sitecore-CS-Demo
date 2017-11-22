using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Sitecore.Rules;
using System.Xml.Linq;
using Sitecore.Commerce.Pipelines;
using Sitecore.Commerce.Services.Carts;
using Sitecore.Commerce.Services;

namespace KeefePOC.Pipelines.Validations.ViewCart
{
	public class CheckPromo : PipelineProcessor<ServicePipelineArgs>
	{

		public override void Process(ServicePipelineArgs args)
		{
			Item item = Sitecore.Context.Item;
			Sitecore.Data.Database contextDB = Sitecore.Configuration.Factory.GetDatabase(item.Database.Name);
			//Sitecore.Data.Items.Item ruleItem = contextDB.GetItem(new Sitecore.Data.ID(SitecoreConstants.ItemGUID), item.Language);
			//String rule = ruleItem.Fields["Rule"].Value;

			var rule = GetViewCartRules(contextDB);

			if (!string.IsNullOrEmpty(rule))
			{
				var rules = RuleFactory.ParseRules<RuleContext>(item.Database, XElement.Parse(rule));

				var ruleContext = new RuleContext()
				{
					Item = item
				};

				if (rules.Rules.Any())
				{
					var executed = false;
					foreach (var ruleItem in rules.Rules)
					{
						executed = ruleItem.Evaluate(ruleContext);
					}					
					
					args.Result.Success = executed;
				}
			}
			else
			{
				args.Result.Success = false;
			}
		}


		private string GetViewCartRules(Sitecore.Data.Database contextDB)
		{
			var siteName = Sitecore.Context.Site.ContentStartPath;

			var siteNode = contextDB.GetItem(siteName);

			var program = siteNode["Program"];

			if(!string.IsNullOrEmpty(program))
			{
				var ruleItem = contextDB.GetItem(program);
				var rules = ruleItem.Fields["Promo"].Value;
				return rules;
			}

			return "";

		}
	}
}
