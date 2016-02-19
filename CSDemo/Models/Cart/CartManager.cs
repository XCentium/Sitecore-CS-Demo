using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace CSDemo.Models.Cart
{
    /// <summary>
    /// Add Item to Cart
    /// Update an Item in Cart
    /// Remove Item from Cart
    /// Empty Cart
    /// List items in cart
    /// </summary>
    public class CartManager
    {

        /// <summary>
        /// Takes sitecore GUID and call the Update cartMetchod 
        /// </summary>
        /// <param name="ID">Sitecore guid</param>
        public void AddToCart(string id)
        {

            UpdateShopingCartWithItem(id, 1);

        }

        private void UpdateShopingCartWithItem(string id, int q)
        {
            // check if cart already had item with ID

            if (this.shoppingCart.CartItems.Count > 0 && this.shoppingCart.CartItems.Any(p => p.ProductID == id))
            {

                // The object already exists, set the quantity attribute

                this.shoppingCart.CartItems.FirstOrDefault(p => p.ProductID == id).Quantity += q;

                // Call Shoppingcart Update data method

                UpdateShppingCartData();


            }
            else
            {

                // add new cartItem to the shopping cart list and update shoppingcart data

                var product = Sitecore.Context.Database.GetItem(new ID(id));
                if (product != null)
                {
                    CartItem CartItem = CreateNewCartItem(product, id);

                    if (CartItem.ProductID != string.Empty)
                    {
                        this.shoppingCart.CartItems.Add(CartItem);

                        UpdateShppingCartData();

                    }

                }
            }
        }

        private CartItem CreateNewCartItem(Item product, string id)
        {
            var CartItem = new CartItem();


            try
            {
                CartItem.ProductID = id;
                CartItem.ProductName = product.DisplayName.ToString();
                CartItem.CSProductId = product["ProductId"];
                CartItem.Quantity = 1;
                CartItem.UnitPrice = Decimal.Parse(product["ListPrice"]);
                CartItem.SubTotal = CartItem.UnitPrice * CartItem.Quantity;

                if (!string.IsNullOrEmpty(product["Images"]))
                {
                    var ProductImages = product["Images"].Split('|');
                    Item ProductImageItem = Sitecore.Context.Database.GetItem(ProductImages[0]);
                    CartItem.ImageUrl = "/~/media/" + ProductImageItem.ID.ToShortID() + ".ashx";
                }
            }
            catch 
            {
                
                throw;
            }

            return CartItem;
        }


        /// <summary>
        /// ShoppingCart Upject data Update Method.
        /// It makes sure the figure in the shopping cart are correct
        /// </summary>
        private void UpdateShppingCartData()
        {
            this.shoppingCart.Currency = "USD";
            this.shoppingCart.Discount = 0.0m;
            this.shoppingCart.Total = 0;
            this.shoppingCart.LineTotal = 0.00m;


            if (this.shoppingCart.CartItems.Count > 0)
            {

                foreach (var p in this.shoppingCart.CartItems.ToList())
                {

                    if (p.Quantity < 1)
                    {
                        this.shoppingCart.CartItems.Remove(p);
                    }
                    else
                    {
                        var subTotal = p.Quantity * p.UnitPrice;
                        this.shoppingCart.CartItems.FirstOrDefault(x => x.ProductID == p.ProductID).SubTotal = subTotal;
                        this.shoppingCart.Total += p.Quantity;
                        this.shoppingCart.LineTotal += subTotal; 
                    }
                }
            }

            HttpContext.Current.Session["Cart"] = this.shoppingCart;
            
        }


        /// <summary>
        /// Update cart 
        /// </summary>
        /// <param name="id">Sitecore guid</param>
        /// <param name="quantity">Quantity of items to add</param>
        public void UpdateCart(string id, int quantity)
        {

        }

        public void RemoveItemFromCart(string id)
        {
            this.shoppingCart.CartItems.RemoveAll(x => x.ProductID == id);
            UpdateShppingCartData();
        }

        public void EmptyCart()
        {

        }

        public void GetCartItems()
        {

        }

        public ShoppingCart shoppingCart = new ShoppingCart();

        public CartManager()
        {

            try
            {
                if (null == System.Web.HttpContext.Current.Session["Cart"] as ShoppingCart)
                {
                    this.shoppingCart = new ShoppingCart();
                    System.Web.HttpContext.Current.Session["Cart"] = this.shoppingCart;
                }
                else
                {
                    this.shoppingCart = System.Web.HttpContext.Current.Session["Cart"] as ShoppingCart;
                }
            }
            catch (Exception ex)
            {
                
                    //this.shoppingCart = new ShoppingCart();
                    //System.Web.HttpContext.Current.Session["Cart"] = this.shoppingCart;
            }

        }

    }
}