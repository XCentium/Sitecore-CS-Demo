using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Helpers
{
	public static class LimitsHelper
	{
		public static decimal GetCurrentPeriodWeight(string customerEmail)
		{
			if (string.IsNullOrEmpty(customerEmail)) return 0;

			if (customerEmail.Equals("john@site.com", StringComparison.OrdinalIgnoreCase))
				return 20.75M;

			if (customerEmail.Equals("testtwo@site.com", StringComparison.OrdinalIgnoreCase))
				return 85.4M;

			return 15.25M;
		}

		public static decimal GetCurrentPeriodSpend(string customerEmail)
		{
			if (string.IsNullOrEmpty(customerEmail)) return 0;

			if (customerEmail.Equals("john@site.com", StringComparison.OrdinalIgnoreCase))
				return 125.75M;

			if (customerEmail.Equals("testtwo@site.com", StringComparison.OrdinalIgnoreCase))
				return 90.25M;

			return 150.75M;
		}

		public static decimal GetProgramWeightLimit()
		{
			var item = Sitecore.Context.Item;
			Sitecore.Data.Database contextDB = Sitecore.Configuration.Factory.GetDatabase(item.Database.Name);
			var siteName = Sitecore.Context.Site.ContentStartPath;

			var siteNode = contextDB.GetItem(siteName);

			var program = siteNode["Program"];

			var ruleItem = contextDB.GetItem(program);
			var weightValue = decimal.Parse(ruleItem.Fields["Quarterly Order Weight Limit"].Value);
			return weightValue;
		}

		public static decimal GetProgramSpendLimit()
		{
			var item = Sitecore.Context.Item;
			Sitecore.Data.Database contextDB = Sitecore.Configuration.Factory.GetDatabase(item.Database.Name);
			var siteName = Sitecore.Context.Site.ContentStartPath;

			var siteNode = contextDB.GetItem(siteName);

			var program = siteNode["Program"];

			var ruleItem = contextDB.GetItem(program);
			var priceValue = decimal.Parse(ruleItem.Fields["Quarterly Order Weight Limit"].Value);
			return priceValue;
		}

		public static List<string> GetPromos()
		{
			return new List<string>()
			{
				"Promo 1 Discount Description",
				"Promo 2 Bonus Item 1",
				"Promo 3 Bonus Item 2"
			};
		}
	}
}