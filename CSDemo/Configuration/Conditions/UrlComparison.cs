using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using System.Web;

namespace CSDemo.Configuration.Conditions
{
    public class CheckQuerystringCondition<T> : StringOperatorCondition<T> where T : RuleContext
    {
        public string Value { get; set; }

        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");
            var url = HttpContext.Current.Request.Url.ToString();
            return Compare(url, Value);
        }
    }
}