using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.Diagnostics;
using CSDemo.Models.Checkout.Cart;

namespace CSDemo.Rules.Conditions
{
	public class TotalItemsCondition<T> : OperatorCondition<T> where T : RuleContext
	{		
		public int TotalItemsInCart { get; set; }

		protected override bool Execute(T ruleContext)
		{
			Assert.ArgumentNotNull(ruleContext, "ruleContext");
			if (ruleContext.Item != null)
			{
				CartHelper c = new CartHelper();
				var cart = c.GetMiniCart();

				var cellValue = cart.Total;

				switch (base.GetOperator())
				{
					case ConditionOperator.Equal:
						return cellValue == TotalItemsInCart;
					case ConditionOperator.GreaterThan:
						return cellValue > TotalItemsInCart;
					case ConditionOperator.GreaterThanOrEqual:
						return cellValue >= TotalItemsInCart;
					case ConditionOperator.LessThan:
						return cellValue < TotalItemsInCart;
					case ConditionOperator.LessThanOrEqual:
						return cellValue <= TotalItemsInCart;
					case ConditionOperator.NotEqual:
						return cellValue != TotalItemsInCart;
				}
			}
			return false;
		}
	}
}