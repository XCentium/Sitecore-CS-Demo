#region

using System;
using System.Collections.Generic;

#endregion

namespace CSDemo.Models.Cart
{
    [Serializable]
    public class ShoppingCart
    {
        public List<CartItem> CartItems = new List<CartItem>();

        public int Total { get; set; }

        public Decimal Discount { get; set; }

        public Decimal Shipping { get; set; }

        public string Currency { get; set; }

        public Decimal LineTotal { get; set; }

        public ShoppingCart()
        {
            this.Discount = 0.00m;
            this.Currency = "USD";
            this.Shipping = 0.00m;
        }
    }
}