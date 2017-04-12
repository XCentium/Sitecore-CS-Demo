using Sitecore.Analytics.Automation.Rules.Workflows;
using Sitecore.Analytics.Model.Entities;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace CSDemo.Configuration.Conditions
{
    public class ContactEmailCheck<T> : IntegerComparisonCondition<T> where T : RuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            if (ruleContext == null) return false;

            var automationRuleContext = ruleContext as AutomationRuleContext;
            if (automationRuleContext == null) return false;
            var contact = automationRuleContext.Contact;
            var emailFacet = contact.GetFacet<IContactEmailAddresses>("Emails");
            var personalEmail = emailFacet.Entries["Personal Email"];

            return !string.IsNullOrWhiteSpace(personalEmail.SmtpAddress);
        }
    }
}