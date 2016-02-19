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


        /// <summary>
        /// Add Item to cart based on ID. 
        /// It the Item already exists, it is incremented, else, it is added as 1
        /// Item can only be added to Cart by Anonymous user or user logged in with CommerceCustomer group
        /// </summary>
        /// <param name="productID">This is the Sitecore ItemID of the product</param>
        /// <returns>It returns true if successful, else, it returns false</returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public ShoppingCart AddToCart(string productID)
        {
            // check if user is logged in and not commerce customer, if true, return false

            var cartManager = new CartManager();

            cartManager.AddToCart(productID);

            return cartManager.shoppingCart;
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public ShoppingCart RemoveFromCart(string productID)
        {
            // check if user is logged in and not commerce customer, if true, return false

            var cartManager = new CartManager();

            cartManager.RemoveItemFromCart(productID);

            return cartManager.shoppingCart;
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public ShoppingCart LoadCart()
        {
            // check if user is logged in and not commerce customer, if true, return false

            var cartManager = new CartManager();

            return cartManager.shoppingCart;
        }
    

    }
}
