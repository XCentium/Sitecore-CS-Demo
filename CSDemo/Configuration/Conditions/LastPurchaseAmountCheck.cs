using CSDemo.Models.Account;
using CSDemo.Models.Checkout.Cart;
using CSDemo.Models.Product;
using Sitecore.Analytics.Automation.Rules.Workflows;
using Sitecore.Analytics.Tracking;
using Sitecore.Commerce.Connect.CommerceServer.Orders;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Contacts;
using Sitecore.Commerce.Services.Orders;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CSDemo.Configuration.Conditions
{
    public class LastPurchaseAmountCheck<T> :  IntegerComparisonCondition<T> where T : RuleContext
    {
        public decimal Amount { get; set; }

        protected override bool Execute(T ruleContext)
        {
            if (ruleContext == null) return false;
           
            var automationRuleContext = ruleContext as AutomationRuleContext;
            if (automationRuleContext == null) return false;
            var contact = automationRuleContext.Contact;

            var mostRecentOrder = Order.GetMostRecentOrder(contact);
            if (mostRecentOrder == null) return false;

            decimal lastPurchaseAmount = mostRecentOrder.Total.Amount;
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