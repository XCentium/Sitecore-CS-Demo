using CSDemo.Configuration.Conditions;
using CSDemo.Models.Account;
using Sitecore.Analytics.Automation.Rules.Workflows;
using Sitecore.Analytics.Data;
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
        protected override bool Execute(T ruleContext)
        {
            if (ruleContext == null) return false;

            var automationRuleContext = ruleContext as AutomationRuleContext;
            if (automationRuleContext == null) return false;
            var contact = automationRuleContext.Contact;

            var mostRecentOrder = Order.GetMostRecentOrder(contact);

            var orderDate = mostRecentOrder.Created;
            var repository = new ContactRepository();
            var visit = repository.LoadHistoricalInteractions(contact.ContactId, 1, orderDate, DateTime.Now);
           
            return visit !=null && visit.Any();
        }
    }
}