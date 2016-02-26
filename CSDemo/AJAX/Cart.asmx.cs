using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.Web.Script.Services;
using CSDemo.Models.Cart;
using CSDemo.Helpers;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;

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
        }

        // 
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public bool SubmitOrder()
        {
            // check if user is logged in and not commerce customer, if true, return false
            var ret = false;

            var cartHelper = new CartHelper();

            ret = cartHelper.SubmitCart();

            return ret;
        }


        // 
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public bool ApplyShippingAndBillingToCart(string firstname, string lastname, string email, string company, string address, string addressline1, string city, string country, string fax, string phone, string zip, string firstname2, string lastname2, string email2, string company2, string address2, string addressline12, string city2, string country2, string fax2, string phone2, string zip2, string billandshipping)
        {
            // check if user is logged in and not commerce customer, if true, return false
            var ret = false;



            var cartHelper = new CartHelper();

            ret = cartHelper.ApplyShippingAndBillingToCart(firstname, lastname, email, company, address, addressline1, city, country, fax, phone, zip, firstname2, lastname2, email2, company2, address2, addressline12, city2, country2, fax2, phone2, zip2);

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
        public bool ApplyPaymentMethodToCart(string paymentExternalID, string nameoncard, string creditcard, string expmonth, string expyear, string ccv)
        {
            // check if user is logged in and not commerce customer, if true, return false
            var ret = false;

            var cartHelper = new CartHelper();

            ret = cartHelper.ApplyPaymentMethodToCart( paymentExternalID, nameoncard, creditcard, expmonth, expyear, ccv);

            return ret;
        }

        ///// <summary>
        ///// Add Item to cart based on ID. 
        ///// It the Item already exists, it is incremented, else, it is added as 1
        ///// Item can only be added to Cart by Anonymous user or user logged in with CommerceCustomer group
        ///// </summary>
        ///// <param name="productID">This is the Sitecore ItemID of the product</param>
        ///// <returns>It returns true if successful, else, it returns false</returns>
        //[WebMethod(EnableSession = true)]
        //[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        //public ShoppingCart AddToCart(string productID)
        //{
        //    // check if user is logged in and not commerce customer, if true, return false

        //    var cartManager = new CartManager();

        //    cartManager.AddToCart(productID);

        //    return cartManager.shoppingCart;
        //}

        //[WebMethod(EnableSession = true)]
        //[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        //public ShoppingCart RemoveFromCart(string productID)
        //{
        //    // check if user is logged in and not commerce customer, if true, return false

        //    var cartManager = new CartManager();

        //    cartManager.RemoveItemFromCart(productID);

        //    return cartManager.shoppingCart;
        //}

        //[WebMethod(EnableSession = true)]
        //[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        //public ShoppingCart LoadCartData()
        //{
        //    // check if user is logged in and not commerce customer, if true, return false



        //    var cartManager = new CartManager();

        //    return cartManager.shoppingCart;
        //}


        //public struct CurrentCartItem
        //{
        //    public string ProductId { get; set; }
        //    public string Quantity { get; set; }
        //}
        
        //[WebMethod(EnableSession = true)]
        //[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        //public ShoppingCart UpdateCartList(List<CurrentCartItem> currentCartItems)
        //{
        //    // check if user is logged in and not commerce customer, if true, return false

        //    var cartManager = new CartManager();            

        //    foreach (CurrentCartItem c in currentCartItems)
        //    {
        //        // ensure q can only be an integer
        //        var q = c.Quantity.Trim();
        //        if (string.IsNullOrEmpty(q)) { q = "0"; }

        //        if (q.All(Char.IsDigit))
        //        {
        //            cartManager.UpdateCartItem(c.ProductId, q);
        //        }
        //    }

        //    return cartManager.shoppingCart;
        //}
   

    }
}
