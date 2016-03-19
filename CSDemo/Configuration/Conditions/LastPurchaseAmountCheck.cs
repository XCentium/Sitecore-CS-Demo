using CSDemo.Configuration.Facets;
using Sitecore.Analytics.Automation.Rules.Workflows;
using Sitecore.Analytics.Tracking;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

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

            decimal lastPurchaseAmount = GetLastPurchaseAmount(contact);
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

        private static decimal GetLastPurchaseAmount(Contact contact)
        {
            decimal amount = 0;
            if (contact == null) return amount;
            var lastOrderTotalFacet = contact.GetFacet<ILastOrderTotal>(LastOrderTotal._FACET_NAME);
            if (lastOrderTotalFacet == null) return amount;
            return lastOrderTotalFacet.Amount;
        }
    }
}