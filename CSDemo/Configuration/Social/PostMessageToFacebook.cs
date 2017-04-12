using CSDemo.Models.Checkout.Cart;
using CSDemo.Models.Product;
using Facebook;
using Sitecore.Analytics.Automation;
using Sitecore.Analytics.Automation.Data;
using Sitecore.Analytics.Tracking;
using Sitecore.Commerce.Automation.MarketingAutomation;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Social.Connector.Facets.Contact.SocialProfile;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace CSDemo.Configuration.Social
{
    public class PostMessageToFacebook : IAutomationAction
    {
        #region Fields

        private readonly string _catalogName;

        #endregion

        #region Constructor

        public PostMessageToFacebook()
        {
            _catalogName = Settings.GetSetting("Site_XCentiumCSDemo_Catalog", "Adventure Works Catalog");
        }

        #endregion

        public AutomationActionResult Execute(AutomationActionContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            var contact = context.Contact;
            if (contact == null)
            {
                Log.Error("Facebook Post Error: Unable to get the contact.", this);
                return AutomationActionResult.Continue; ;
            }

            var fbConfigItem = context.Action.Database.GetItem(Constants.Social.FacebookAppConfigItemId);
            if (fbConfigItem == null)
            {
                Log.Error("Facebook Post Error: Unable to get the FacebookAppConfigItemId.", this);
                return AutomationActionResult.Continue; 
            }
            var appId = fbConfigItem["Application ID"];
            var appSecret = fbConfigItem["Application Secret"];

            var products = GetBackInStockProducts(contact, context.AutomationStateContext);
            if (products == null || !products.Any())
            {
                Log.Error("Facebook Post Error: Unable to get product info.", this);
                return AutomationActionResult.Continue;
            }

            var firstName = GetContactFirsName(contact);
            if (string.IsNullOrWhiteSpace(firstName))
                firstName = "there";

            //var fbAuthClient = new FacebookOAuthClient() { AppId = appId, AppSecret = appSecret };
            //dynamic accessToken = fbAuthClient.GetApplicationAccessToken();
            //if (string.IsNullOrWhiteSpace((string)accessToken["access_token"]))
            //{
            //    Sitecore.Diagnostics.Log.Error("Facebook Post Error: Unable to get application access token.", this);
            //    return AutomationActionResult.Continue;
            //}
            var fb = new FacebookClient(appId, appSecret); //FacebookClient((string)accessToken["access_token"]); 
            const string defaultDomain = "http://csdemo.xcentium.net";

            foreach (var product in products)
            {
                dynamic messagePost = new ExpandoObject();
                messagePost.picture = defaultDomain + product.FirstImage.Replace("/sitecore/shell", string.Empty);
                messagePost.link = defaultDomain + product.Url.Replace("/sitecore/shell/csdemo/home", string.Empty);
                messagePost.name = $"We've got your {product.Title}!";
                messagePost.caption = _catalogName;
                messagePost.description = product.Description;
                messagePost.message = $"Hey {firstName}, the {product.Title} are back in stock! This pair seems to go quickly, get them before they are gone again.";
                
                var userId = contact.Identifiers.Identifier
                    .Replace($"{Constants.Commerce.DefaultSocialDomainForCommerce}\\", string.Empty)
                    .Replace("_facebook", string.Empty);


                dynamic notificationPost = new ExpandoObject();
                notificationPost.template = $"We've got your {product.Title}! Check your wall!";

                try
                {
                    dynamic result = fb.Post($"/{userId}/feed", messagePost);
                    dynamic notificationResult = fb.Post($"/{userId}/notifications", notificationPost);
                }
                catch (Exception ex)
                {
                    Log.Error("Facebook Post Error: Unable to post message to facebook. " + ex.Message, this);
                }
            }
            return AutomationActionResult.Continue;
        }

        private string GetContactFirsName(Contact contact)
        {
            if (contact == null || !contact.Identifiers.Identifier.ToLower().Contains("facebook")) return null;
            
            if (!contact.Facets.ContainsKey("SocialProfile"))
            {
                Log.Error("Facebook Post Error: Unable to get the SocialProfile.", this);
                return null;
            }


            var socialProfileFacet = contact.GetFacet<ISocialProfileFacet>("SocialProfile");
            if (socialProfileFacet.Networks == null || !socialProfileFacet.Networks.Contains("Facebook"))
            {
                Log.Error("Facebook Post Error: Facebook network does not exist in this contact's profile.", this);
                return null;
            }


            var fbNetwork = socialProfileFacet.Networks["Facebook"];
            if (fbNetwork == null)
            {
                Log.Error("Facebook Post Error: Facebook network is null.", this);
                return null;
            }
            var firstname = fbNetwork.Fields["first_name"].Value;
            

            return firstname;
        }

        private IEnumerable<Product> GetBackInStockProducts(Contact contact, AutomationStateContext context)
        {
            var provider = (InventoryAutomationProvider)Factory.CreateObject("inventoryAutomationProvider", true);
            if (provider == null)
            {
                Log.Error("Facebook Post Error: Unable to get inventoryAutomationProvider.", this);
                return null;
            }

            var notificationRequests = provider.GetProductsBackInStock(context);
            if (notificationRequests == null || !notificationRequests.Any())
            {
                Log.Info("Facebook Post Info: contact is not signed up for any product notifications.", this);
                return null;
            }
            var cartHelper = new CartHelper("XCentiumCSDemo");
            var products = new List<Product>();

            foreach (var notification in notificationRequests)
            {
                var stockInfo = cartHelper.GetProductStockInformation(notification.Product.ProductId, Settings.GetSetting("Site_XCentiumCSDemo_Catalog", "Adventure Works Catalog"));
                if (stockInfo != null && stockInfo.Status != null && stockInfo.Status.Name == "InStock")
                {
                    var productId = stockInfo.Product.ProductId;
                    var product = Product.GetProduct(productId);
                    if (product == null || string.IsNullOrWhiteSpace(product.Title)) continue;
                    products.Add(product);
                }
            }

            provider.UpdateProductsBackInStock(context, new List<Sitecore.Commerce.Entities.Inventory.StockNotificationRequest>());
            AutomationStateManager.Create(contact).SaveChanges(AutomationManager.Provider);
            return products;
        }
    }

}