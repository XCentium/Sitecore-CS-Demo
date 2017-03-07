using CSDemo.Models.Account;
using Sitecore.Analytics.Automation.Rules.Workflows;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using System.Web;

namespace CSDemo.Configuration.Conditions
{
    public class InmateIdCheck<T> :  IntegerComparisonCondition<T> where T : RuleContext
    {
        public int InmateId { get; set; }

        protected override bool Execute(T ruleContext)
        {
            Log.Info("CS DEMO: Checking inmate id.", this);
            if (ruleContext == null) return false;
            var currentInmateId = HttpContext.Current.Request.QueryString["iid"];
            Log.Info("CS DEMO: checking inmate id from querystring:" + currentInmateId, this);
            int inmateIdInt = 0;
            var conditionResult = !string.IsNullOrWhiteSpace(currentInmateId) && int.TryParse(currentInmateId, out inmateIdInt) 
                && inmateIdInt == InmateId;
            Log.Info("CS DEMO: Inmate ID check condition result:" + conditionResult, this);
            return conditionResult;

        }
    }
}