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

            return true;
        }
    }
}