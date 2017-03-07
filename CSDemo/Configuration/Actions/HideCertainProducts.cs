using Sitecore.Diagnostics;

namespace CSDemo.Configuration.Actions
{
    public class HideCertainProducts<T> : Sitecore.Rules.Actions.RuleAction<T> 
        where T : Sitecore.Rules.ConditionalRenderings.ConditionalRenderingsRuleContext
    {
        public string ProductType { get; set; }

        public override void Apply(T ruleContext)
        {
            string key = Constants.QueryStrings.HideProductType;
            Log.Info("CS DEMO: Evaluating Keefe Product Type Rule for " + key, this);
            
            if (string.IsNullOrWhiteSpace(ruleContext.Reference.Settings.Parameters))
            {
                ruleContext.Reference.Settings.Parameters =
                key + "=" + this.ProductType.ToString();
                return;
            }
            var parameters = Sitecore.Web.WebUtil.ParseQueryString(
                                    ruleContext.Reference.Settings.Parameters);
            foreach (var check in parameters.Keys)
            {
                if (string.Compare(check, key, true) == 0)
                {
                    key = check;
                    break;
                }
            }
            parameters[key] = ProductType.ToString();
            ruleContext.Reference.Settings.Parameters =
            Sitecore.Web.WebUtil.BuildQueryString(parameters, false);
        }
    }
}