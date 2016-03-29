#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using CSDemo.Models.Cart;
using CSDemo.Models.Checkout.Cart;
using Sitecore.Analytics.Data.Items;
using Sitecore.Analytics;
using Sitecore.Analytics.Outcome.Extensions;
using Sitecore.Analytics.Outcome.Model;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Analytics.Model.Framework;
using CSDemo.Configuration.Facets;

#endregion

namespace CSDemo.AJAX
{
    /// <summary>
    /// Summary description for Cart
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [System.ComponentModel.ToolboxItem(false)]
    public class Cart : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public ShoppingCart LoadCart()
        {
            return CartHelper.GetMiniCart();
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CheckIfCommerceActionsAllowed()
        {
            if (!string.IsNullOrEmpty(ActionAllowed)) { return ActionAllowed; }

            return string.Empty;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string AddProductToCart(string Quantity, string ProductId, string CatalogName, string VariantId, string contextItemId)
        {
            var ret = string.Empty;
            ret = CartHelper.AddProductToCart(Quantity, ProductId, CatalogName, VariantId);
            RegisterGoal(Constants.Marketing.AddToCartGoalId, contextItemId);
            return ret;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string SubmitOrder(string contextItemId, string orderTotal)
        {
            string ret;
            ret = CartHelper.SubmitCart();
            RegisterGoal(Constants.Marketing.SubmitOrderGoalId, contextItemId);
            //RegisterOutcome(new ID(Constants.Marketing.PurchaseOutcomeDefinitionId));
          
            // SavePurchaseAmount(orderTotal); // Throwing error, so commented out
            return ret;
        }


        
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public bool ApplyShippingAndBillingToCart(string firstname, string lastname, string email, string company,
            string address, string addressline1, string city, string country, string fax, string phone, string zip,
            string firstname2, string lastname2, string email2, string company2, string address2, string addressline12,
            string city2, string country2, string fax2, string phone2, string zip2, string billandshipping)
        {

            return CartHelper.ApplyShippingAndBillingToCart(firstname, lastname, email, company, address, addressline1,
                city, country, fax, phone, zip, firstname2, lastname2, email2, company2, address2, addressline12, city2,
                country2, fax2, phone2, zip2);

        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public bool ApplyShippingMethodToCart(string shippingMethodId)
        {
            return CartHelper.AddShippingMethodToCart(shippingMethodId);
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public bool ApplyPaymentMethodToCart(string paymentExternalID, string nameoncard, string creditcard,
            string expmonth, string expyear, string ccv)
        {
            return CartHelper.ApplyPaymentMethodToCart(paymentExternalID, nameoncard, creditcard, expmonth, expyear, ccv);
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public bool RemoveFromCart(string externalID)
        {
            return CartHelper.RemoveItemFromCart(externalID);
        }

        public struct CurrentCartItem
        {
            public string ExternalID { get; set; }
            public string Quantity { get; set; }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public bool UpdateCartList(List<CurrentCartItem> currentCartItems)
        {
            // check if user is logged in and not commerce customer, if true, return false

            var ret = false;

            foreach (var c in currentCartItems)
            {
                // ensure q can only be an integer
                var q = c.Quantity.Trim();
                if (string.IsNullOrEmpty(q))
                {
                    q = "0";
                }

                if (q.All(Char.IsDigit))
                {
                    ret = CartHelper.UpdateCartItem(c.ExternalID, q);
                }
            }

            return ret;
        }

        public CartHelper CartHelper { get; set; }
        public string ActionAllowed { get; set; }
        public Cart()
        {
            this.CartHelper = new CartHelper();

            this.ActionAllowed = this.CartHelper.CustomerOrAnonymous();
        }



        #region Private Helpers

        private void SavePurchaseAmount(string orderTotal)
        {
            decimal amount = 0;
            if (string.IsNullOrWhiteSpace(orderTotal) || Tracker.Current.Contact == null || !decimal.TryParse(orderTotal, out amount)) return;
            var lastOrderTotalFacet = Tracker.Current.Contact.GetFacet<ILastOrderTotal>(LastOrderTotal._FACET_NAME);
            if (lastOrderTotalFacet == null) return;
            lastOrderTotalFacet.Updated = DateTime.Now.ToUniversalTime();
            lastOrderTotalFacet.Amount = amount;
        }

        private void RegisterGoal(string goalId, string itemId)
        {
            try {
                if (!Tracker.Current.IsActive)
                    Tracker.Current.StartTracking();

                if (!Tracker.Current.IsActive
                    || Tracker.Current.CurrentPage == null)
                    return;

                var deliveryDatabase = Sitecore.Configuration.Factory.GetDatabase(Constants.DeliveryDatabase);
                if (deliveryDatabase == null) return;
                var goalItem = deliveryDatabase.GetItem(goalId);
                if (goalItem == null)
                {
                    Log.Error($"Unable to register goal with ID {goalId}. Make sure everything is deployed and published correctly.", this);
                }
                var goal = new PageEventItem(goalItem);
                if (goal == null)
                {
                    Log.Error($"Unable to register page event goal with ID {goalId}. Make sure everything is deployed and published correctly.", this);
                }
                var pageEventsRow = Tracker.Current.CurrentPage.Register(goal);

                pageEventsRow.Data = "Product added to cart - "
                    + DateTime.Now.ToString("F");

                if (string.IsNullOrWhiteSpace(itemId)) return;
                var contextItem = deliveryDatabase.GetItem(itemId);
                if (contextItem == null) return;

                pageEventsRow.ItemId = contextItem.ID.Guid;
                pageEventsRow.DataKey = contextItem.Paths.Path;
            }
            catch(Exception ex)
            {
                Log.Error($"Unable to register goal with ID {goalId}.", ex);
            }
        }

        private void RegisterOutcome(ID outcomeDefinitionId)
        {
            try
            {
                var outcome = new ContactOutcome(ID.NewID, outcomeDefinitionId, new ID(Tracker.Current.Contact.ContactId));
                Tracker.Current.RegisterContactOutcome(outcome);
            }
            catch (Exception ex)
            {
                Log.Error("Unable to register a goal with ID " + outcomeDefinitionId, ex);
            }
        }

        #endregion
    }
}