using Castle.Core.Internal;
using Sitecore.Analytics.Automation;
using Sitecore.Diagnostics;

namespace CSDemo.Configuration.Social
{
    public class CustomSocialGoalTrigger : IAutomationAction
    {
        public AutomationActionResult Execute(AutomationActionContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            if (!Sitecore.Analytics.Tracker.IsActive || Sitecore.Analytics.Tracker.Current.CurrentPage == null)
                return AutomationActionResult.Continue;
            Sitecore.Data.Items.Item socialMessageGoalItem = Sitecore.Context.Database.GetItem(Constants.Cart.AbandonedCartSocialGoalItemId);
            if (socialMessageGoalItem != null)
            {
                Sitecore.Analytics.Data.Items.PageEventItem registerthegoal = new Sitecore.Analytics.Data.Items.PageEventItem(socialMessageGoalItem);
                Sitecore.Analytics.Model.PageEventData eventData = Sitecore.Analytics.Tracker.Current.CurrentPage.Register(registerthegoal);
                eventData.Data = socialMessageGoalItem["Description"];
                Sitecore.Analytics.Tracker.Current.Interaction.AcceptModifications();
            }
            return AutomationActionResult.Continue;
        }
    }
}
