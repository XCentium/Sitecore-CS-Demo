using Sitecore.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Configuration.Actions
{
    public class ShowCertainProducts<T> : Sitecore.Rules.Actions.RuleAction<T> 
        where T : Sitecore.Rules.ConditionalRenderings.ConditionalRenderingsRuleContext
    {
        public string ProductType { get; set; }

        public override void Apply(T ruleContext)
        {
            string key = "ProductType";
            if (String.IsNullOrEmpty(ruleContext.Reference.Settings.Parameters))
            {
                ruleContext.Reference.Settings.Parameters =
                key + "=" + this.ProductType.ToString();
                return;
            }
            SafeDictionary<string> parameters = Sitecore.Web.WebUtil.ParseQueryString(
            ruleContext.Reference.Settings.Parameters);
            foreach (string check in parameters.Keys)
            {
                if (string.Compare(check, key, true) == 0)
                {
                    key = check;
                    break;
                }
            }
            parameters[key] = this.ProductType.ToString();
            ruleContext.Reference.Settings.Parameters =
            Sitecore.Web.WebUtil.BuildQueryString(parameters, false);
        }
    }
}