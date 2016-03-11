using CSDemo.Models.Checkout.Cart;
using CSDemo.Models.Product;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Configuration.Conditions
{
    public class ReturnedVisitorAfterPurchase<T> : IntegerComparisonCondition<T> where T : RuleContext
    {
        public decimal Amount { get; set; }

        protected override bool Execute(T ruleContext)
        {
            if (ruleContext == null) return false;
            var orders = ProductHelper.GetCustomerOrders(new CartHelper());
            decimal lastPurchaseAmount = 1;
            switch (GetOperator())
            {
                case ConditionOperator.Equal:
                    return lastPurchaseAmount == Amount;
                case ConditionOperator.GreaterThan:
                    return lastPurchaseAmount > Amount;
                case ConditionOperator.GreaterThanOrEqual:
                    return lastPurchaseAmount >= Amount;
                case ConditionOperator.LessThan:
                    return lastPurchaseAmount < Amount;
                case ConditionOperator.LessThanOrEqual:
                    return lastPurchaseAmount <= Amount;
                case ConditionOperator.NotEqual:
                    return lastPurchaseAmount != Amount;
                default:
                    return false;
            }
        }
    }
}