using CSDemo.Configuration.Conditions;
using CSDemo.Models.Account;
using Sitecore.Analytics;
using Sitecore.Analytics.Automation.Rules.Workflows;
using Sitecore.Analytics.Data;
using Sitecore.Analytics.Tracking;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Analytics.Models;
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
        public int Number { get; set; }

        protected override bool Execute(T ruleContext)
        {
            if (ruleContext == null || Number<=0) return false;

            var automationRuleContext = ruleContext as AutomationRuleContext;
            if (automationRuleContext == null) return false;
            var contact = automationRuleContext.Contact;

            var lastVisitDate = GetLastVisitDate(contact);
            if (lastVisitDate == DateTime.MinValue) return false;
            return lastVisitDate > DateTime.Now.AddMinutes(-Number).ToUniversalTime();
        }

        private static DateTime GetLastVisitDate(Contact contact)
        {
            if (contact == null) return DateTime.MinValue;

            var lastVisitDate = DateTime.MinValue;
            var index = ContentSearchManager.GetIndex(Constants.Sitecore.AnalyticsIndexName);
            using (var context = index.CreateSearchContext())
            {
                var visits = context.GetQueryable<IndexedVisit>()
                    .Where(x => x.ContactId ==
                    contact.ContactId).OrderByDescending(v => v.EndDateTime).ToList();
                if (!visits.Any()) return lastVisitDate;
                lastVisitDate = visits.First().EndDateTime;
                return lastVisitDate;
            }
        }
    }
}