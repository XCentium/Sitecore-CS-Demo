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

        public Decimal Tax { get; set; }

        private Decimal grandTotal;

        public Decimal GrandTotal
        {
            get { return this.Shipping + this.LineTotal +this.Tax - this.Discount; }
            set { grandTotal = value; }
        }
        


        public ShoppingCart()
        {
            this.Discount = 0.00m;
            this.Currency = "USD";
            this.Shipping = 0.00m; 
            this.Tax = 0.00m;
            this.GrandTotal = 0.00m;

        }
    }
}