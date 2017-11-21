using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.Diagnostics;
using CSDemo.Models.Checkout.Cart;


namespace KeefePOC.Rules.Conditions
{
	public class PreTaxTotal<T> : OperatorCondition<T> where T : RuleContext
	{
		public decimal CartTotal { get; set; }

		protected override bool Execute(T ruleContext)
		{
			Assert.ArgumentNotNull(ruleContext, "ruleContext");
			if (ruleContext.Item != null)
			{
				CartHelper c = new CartHelper();
				var cart = c.GetMiniCart();

				var cellValue = cart.LineTotal;
				
				switch (base.GetOperator())
				{
					case ConditionOperator.Equal:
						return cellValue == CartTotal;
					case ConditionOperator.GreaterThan:
						return cellValue > CartTotal;
					case ConditionOperator.GreaterThanOrEqual:
						return cellValue >= CartTotal;
					case ConditionOperator.LessThan:
						return cellValue < CartTotal;
					case ConditionOperator.LessThanOrEqual:
						return cellValue <= CartTotal;
					case ConditionOperator.NotEqual:
						return cellValue != CartTotal;
				}				
			}
			return false;
		}
	}
}
