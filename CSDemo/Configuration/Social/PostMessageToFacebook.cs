using CSDemo.Models.Checkout.Cart;
using CSDemo.Models.Product;
using Facebook;
using Sitecore.Analytics.Automation;
using Sitecore.Analytics.Automation.Data;
using Sitecore.Analytics.Tracking;
using Sitecore.Commerce.Automation.MarketingAutomation;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Social.Connector.Facets.Contact.SocialProfile;
using Sitecore.Social.Domain.Model;
using Sitecore.Social.Facebook.Api.Builders;
using Sitecore.Social.Facebook.Api.Model;
using Sitecore.Social.Facebook.MessagePosting.Providers;
using Sitecore.Social.Facebook.Networks.Messages;
using Sitecore.Social.MessagePosting.Providers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

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

            var product = GetBackInStockProduct(contact, context.AutomationStateContext);
            if (product == null)
            {
                Log.Error("Facebook Post Error: Unable to get product info.", this);
                return AutomationActionResult.Continue;
            }
            dynamic messagePost = new ExpandoObject();
            //messagePost.picture = "https://www.cmsbestpractices.com/rockies.png";
            //messagePost.link = "http://csdemo.xcentium.net/categories/boots/aw078-04";
            //messagePost.name = "We've got your boots!";
            //messagePost.caption = _catalogName;
            //messagePost.description = "Lightweight Nubuck leather/nylon, water-resistant boots, polyurethane soles.";
            //messagePost.message = $"Hey {firstname}, the Rockies are back in stock! This pair seems to go quickly, get them before they are gone again.";

            var firstName = GetContactFirsName(contact);
            if (string.IsNullOrWhiteSpace(firstName))
                firstName = "Vasiliy";

            messagePost.picture = product.Image1;
            messagePost.link = product.Url; 
            messagePost.name = $"We've got your {product.Title}!";
            messagePost.caption = _catalogName;
            messagePost.description = product.Description;
            messagePost.message = $"Hey {firstName}, the Rockies are back in stock! This pair seems to go quickly, get them before they are gone again.";

            var fb = new FacebookClient(appId, appSecret);
            var userId = contact.Identifiers.Identifier
                .Replace($"{Constants.Commerce.DefaultSocialDomainForCommerce}\\", string.Empty)
                .Replace("_facebook", string.Empty);
            var userFeedPath = $"/{userId}/feed";

            try
            {
                dynamic result = fb.Post(userFeedPath, messagePost);
            }
            catch (Exception ex)
            {
                Log.Error("Facebook Post Error: Unable to post message to facebook. "+ ex.Message , this);
            }

            return AutomationActionResult.Continue;
        }

        private string GetContactFirsName(Contact contact)
        {
            if (contact == null || !contact.Identifiers.Identifier.ToLower().Contains("facebook")) return null;
            
            if (contact == null || !contact.Facets.ContainsKey("SocialProfile"))
            {
                Log.Error("Facebook Post Error: Unable to get the SocialProfile.", this);
                return null;
            }


            var socialProfileFacet = contact.GetFacet<ISocialProfileFacet>("SocialProfile");

            var test = socialProfileFacet.Networks as IEnumerable<NetworkElement>;
            if (socialProfileFacet == null || socialProfileFacet.Networks == null || !socialProfileFacet.Networks.Contains("Facebook"))
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
            var firstname = fbNetwork.Fields["first_name"].ToString();
            

            return firstname;
        }

        private Product GetBackInStockProduct(Contact contact, AutomationStateContext context)
        {
            var provider = (InventoryAutomationProvider)Factory.CreateObject("inventoryAutomationProvider", true);
            if (provider == null)
            {
                Log.Error("Facebook Post Error: Unable to get inventoryAutomationProvider.", this);
                return null;
            }

            var notificationRequests = provider.GetBackInStockProducts(provider.GetProductNotifications(context), context);
            if (notificationRequests == null)
            {
                Log.Error("Facebook Post Error: Unable to get notificationRequests.", this);
                return null;
            }

            var productIds = new List<string>();
            foreach (var notificationRequest in notificationRequests)
            {
                productIds.Add(notificationRequest.Product.ProductId);
            }

            var products = productIds.Select(i => Product.GetProduct(i));
            if (products == null || !products.Any())
            {
                Log.Error("Facebook Post Error: Unable to get products.", this);
                return null;
            }
            return products.First();
        }
    }

}