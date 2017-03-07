using CSDemo.Models.Account;
using Sitecore.Analytics.Automation.Rules.Workflows;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace CSDemo.Configuration.Conditions
{
    public class InmateIdCheck<T> :  IntegerComparisonCondition<T> where T : RuleContext
    {
        public long InmateId { get; set; }

        protected override bool Execute(T ruleContext)
        {
            if (ruleContext == null) return false;
          
            var currentInmateId = 123;
            return currentInmateId == InmateId;
        }
    }
}