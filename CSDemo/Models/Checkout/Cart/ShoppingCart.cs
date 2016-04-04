#region

using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace CSDemo.Models.Cart
{
    [Serializable]
    public class ShoppingCart
    {
        public List<CartItem> CartItems = new List<CartItem>();

        public int Total { get; set; }

        public Decimal Shipping { get; set; }

        public string Currency { get; set; }

        public Decimal LineTotal { get; set; }

        private Decimal productsTotal;
        public Decimal ProductsTotal { 
            
            get
            {
                return this.discount + this.LineTotal;
            }
            
            set
            {
                productsTotal = value;
            } 
        }

        public Decimal Tax { get; set; }

        private Decimal grandTotal;

        public Decimal GrandTotal { get; set; }

        private Decimal discount;

        public Decimal Discount
        {
            get { return this.GetDiscount(); }
            set { discount = value; }
        }
        


        public ShoppingCart()
        {
            this.Discount = 0.00m;
            this.Currency = "USD";
            this.Shipping = 0.00m; 
            this.Tax = 0.00m;
            this.GrandTotal = 0.00m;

        }

        public Decimal GetDiscount()
        {
            var discount = 0.00m;

            if (CartItems.Count > 0)
            {
                var cartToTal = CartItems.Sum(x => (x.UnitPrice * x.Quantity));

                return cartToTal - LineTotal;
            }

            return discount;
        }
    }
}