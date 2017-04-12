using CSDemo.Models.Account;
using Sitecore.Analytics.Automation.Rules.Workflows;
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
            var lastPurchaseAmount = Order.GetLastOrderAmount(contact);

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