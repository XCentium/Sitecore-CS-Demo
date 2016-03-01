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
            // check if user is logged in and not commerce customer, if true, return false

            var cartHelper = new CartHelper();

            var cart = cartHelper.GetMiniCart();

            return cart;
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public bool AddProductToCart(string Quantity, string ProductId, string CatalogName, string VariantId)
        {
            // check if user is logged in and not commerce customer, if true, return false
            var ret = false;

            var cartHelper = new CartHelper();

            ret = cartHelper.AddProductToCart(Quantity, ProductId, CatalogName, VariantId);

            return ret;

            RegisterGoal(Constants.Marketing.AddToCartGoalId);
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string SubmitOrder()
        {
            // check if user is logged in and not commerce customer, if true, return false
            var ret = string.Empty;
            var cartHelper = new CartHelper();
            ret = cartHelper.SubmitCart();

            RegisterGoal(Constants.Marketing.SubmitOrderGoalId);
            RegisterOutcome(new ID(Constants.Marketing.PurchaseOutcomeDefinitionId));

            return ret;
        }


        // 
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public bool ApplyShippingAndBillingToCart(string firstname, string lastname, string email, string company,
            string address, string addressline1, string city, string country, string fax, string phone, string zip,
            string firstname2, string lastname2, string email2, string company2, string address2, string addressline12,
            string city2, string country2, string fax2, string phone2, string zip2, string billandshipping)
        {
            // check if user is logged in and not commerce customer, if true, return false
            var ret = false;

            var cartHelper = new CartHelper();

            ret = cartHelper.ApplyShippingAndBillingToCart(firstname, lastname, email, company, address, addressline1,
                city, country, fax, phone, zip, firstname2, lastname2, email2, company2, address2, addressline12, city2,
                country2, fax2, phone2, zip2);

            return ret;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public bool ApplyShippingMethodToCart(string shippingMethodId)
        {
            // check if user is logged in and not commerce customer, if true, return false
            var ret = false;

            var cartHelper = new CartHelper();

            ret = cartHelper.AddShippingMethodToCart(shippingMethodId);

            return ret;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public bool ApplyPaymentMethodToCart(string paymentExternalID, string nameoncard, string creditcard,
            string expmonth, string expyear, string ccv)
        {
            // check if user is logged in and not commerce customer, if true, return false
            var ret = false;

            var cartHelper = new CartHelper();

            ret = cartHelper.ApplyPaymentMethodToCart(paymentExternalID, nameoncard, creditcard, expmonth, expyear, ccv);

            return ret;
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public bool RemoveFromCart(string externalID)
        {
            // check if user is logged in and not commerce customer, if true, return false
            var ret = false;

            var cartHelper = new CartHelper();

            ret = cartHelper.RemoveItemFromCart(externalID);

            return ret;
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

            var cartHelper = new CartHelper();

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
                    ret = cartHelper.UpdateCartItem(c.ExternalID, q);
                }
            }

            return ret;
        }

        #region Private Helpers

        private void RegisterGoal(string goalId)
        {
            if (!Tracker.Current.IsActive)
                Tracker.Current.StartTracking();

            if (!Tracker.Current.IsActive
                || Tracker.Current.CurrentPage == null)
                return;

            var goalItem = Sitecore.Context.Database.GetItem(goalId);
            var goal = new PageEventItem(goalItem);
            var pageEventsRow = Tracker.Current.CurrentPage.Register(goal);

            pageEventsRow.Data = "Product added to cart - "
                + DateTime.Now.ToString("F");
            pageEventsRow.ItemId = Sitecore.Context.Item.ID.Guid;
            pageEventsRow.DataKey = Sitecore.Context.Item.Paths.Path;
        }

        private void RegisterOutcome(ID outcomeDefinitionId)
        {
            var outcome = new ContactOutcome(ID.NewID, outcomeDefinitionId, new ID(Tracker.Current.Contact.ContactId));
            Tracker.Current.RegisterContactOutcome(outcome);
        }

        #endregion
    }
}