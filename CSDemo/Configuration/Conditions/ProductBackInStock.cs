using CSDemo.Models.Checkout.Cart;
using Sitecore.Analytics.Automation.Data;
using Sitecore.Analytics.Automation.Rules.Workflows;
using Sitecore.Commerce.Automation.MarketingAutomation;
using Sitecore.Commerce.Entities.Inventory;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Configuration.Conditions
{
    public class ProductBackInStock<T> : WhenCondition<T> where T : AutomationRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            var provider = (InventoryAutomationProvider)Factory.CreateObject("inventoryAutomationProvider", true);
            if (provider == null)
            {
                Sitecore.Diagnostics.Log.Error("XC Products Back ini Stock: Unable to get inventoryAutomationProvider.", this);
                return false;
            }

            
            var context = ruleContext.ContactState;
            var notificationRequests = provider.GetProductNotifications(context);
            if (notificationRequests == null || !notificationRequests.Any())
            {
                Log.Info("XC Products Back in Stock: contact is not signed up for any product notifications.", this);
                return false;
            }
            var cartHelper = new CartHelper("XCentiumCSDemo");
            var productsBcakInStock = provider.GetProductsBackInStock(context);
            var productNotificationsToRemove = new List<StockNotificationRequest>();
            var isAtLeastOneProductInStock = false;
            foreach (var notification in notificationRequests)
            {
                var stockInfo = cartHelper.GetProductStockInformation(notification.Product.ProductId, Settings.GetSetting("Site_XCentiumCSDemo_Catalog", "Adventure Works Catalog"));
                if (stockInfo != null && stockInfo.Status != null && stockInfo.Status.Name == "InStock")
                {
                    productsBcakInStock.Add(notification);
                    productNotificationsToRemove.Add(notification);
                    

                    isAtLeastOneProductInStock = true; 
                }
            }


            foreach(var notificationToRemove in productNotificationsToRemove)
            {
                notificationRequests.Remove(notificationToRemove);
            }
            provider.UpdateProductNotifications(context, notificationRequests);
            provider.UpdateProductsBackInStock(context, productsBcakInStock);
            AutomationStateManager.Create(ruleContext.Contact).SaveChanges(AutomationManager.Provider);
            return isAtLeastOneProductInStock;
        }
    }
}