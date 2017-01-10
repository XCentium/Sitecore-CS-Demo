#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace CSDemo.Models.Checkout.Cart
{
    [Serializable]
    public class ShoppingCart
    {
        public List<CartItem> CartItems = new List<CartItem>();

        public int Total { get; set; }

        public Decimal Shipping { get; set; }

        public string Currency { get; set; }

        public Decimal LineTotal { get; set; }

        private Decimal _productsTotal;
        public Decimal ProductsTotal
        {

            get
            {
                if (Sitecore.Context.User.IsInRole("CommerceUsers\\Dealer")) { return this.LineTotal > 0 ? (decimal)0.90 * this.GetDiscount() + this.LineTotal : this.GetDiscount() + this.LineTotal; }
                if (Sitecore.Context.User.IsInRole("CommerceUsers\\Retailer")) { return this.LineTotal > 0 ? (decimal)0.75 * this.GetDiscount() + this.LineTotal : this.GetDiscount() + this.LineTotal; }

                return this.GetDiscount() + this.LineTotal;
            }

            set
            {
                _productsTotal = value;
            }
        }


        public Decimal Tax { get; set; }

        private Decimal _grandTotal;

        public Decimal GrandTotal
        {

            get
            {
                if (this._grandTotal > 0)
                {
                    return (this._grandTotal > (this.LineTotal + this.Tax + this.Shipping)) ? this.LineTotal + this.Tax + this.Shipping : this._grandTotal;

                }
                return this.LineTotal + this.Tax + this.Shipping;
            }


            set
            {
                _grandTotal = value;
            }

        }

        private Decimal _discount;

        public Decimal Discount
        {
            get { return this.GetDiscount(); }
            set { _discount = value; }
        }

        public decimal LineDiscount { get; internal set; }
        public decimal OrderLevelDiscountAmount { get; internal set; }

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
                //  var cartToTal = CartItems.Sum(x => (x.UnitPrice * x.Quantity));

                //  return cartToTal - LineTotal;

                return this.LineDiscount + OrderLevelDiscountAmount;
            }

            return discount;
        }
    }
}