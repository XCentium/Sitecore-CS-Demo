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

        public decimal Shipping { get; set; }

        public string Currency { get; set; }

        public decimal LineTotal { get; set; }

        public decimal TotalWeight
        {
            get
            {
                var totalWeight = CartItems.Sum(i => Product.Product.GetProduct(i.ProductId).Weight);
                return totalWeight;
            }
        }

        private decimal _productsTotal;
        public decimal ProductsTotal
        {

            get
            {
                if (Sitecore.Context.User.IsInRole("CommerceUsers\\Dealer")) { return LineTotal > 0 ? (decimal)0.90 * GetDiscount() + LineTotal : GetDiscount() + LineTotal; }
                if (Sitecore.Context.User.IsInRole("CommerceUsers\\Retailer")) { return LineTotal > 0 ? (decimal)0.75 * GetDiscount() + LineTotal : GetDiscount() + LineTotal; }

                return GetDiscount() + LineTotal;
            }

            set
            {
                _productsTotal = value;
            }
        }


        public decimal Tax { get; set; }

        private decimal _grandTotal;

        public decimal GrandTotal
        {

            get
            {
                if (_grandTotal > 0)
                {
                    return (_grandTotal > (LineTotal + Tax + Shipping)) ? LineTotal + Tax + Shipping : _grandTotal;

                }
                return LineTotal + Tax + Shipping;
            }


            set
            {
                _grandTotal = value;
            }

        }

        private decimal _discount;

        public decimal Discount
        {
            get { return GetDiscount(); }
            set { _discount = value; }
        }

        public decimal LineDiscount { get; set; }
        public decimal OrderLevelDiscountAmount { get;  set; }
        public List<string> Adjustments { get; set; }

        public ShoppingCart()
        {
            Discount = 0.00m;
            Currency = "USD";
            Shipping = 0.00m;
            Tax = 0.00m;
            GrandTotal = 0.00m;

        }

        public decimal GetDiscount()
        {
            var discount = 0.00m;

            if (CartItems.Count > 0)
            {
                //  var cartToTal = CartItems.Sum(x => (x.UnitPrice * x.Quantity));

                //  return cartToTal - LineTotal;

                return LineDiscount + OrderLevelDiscountAmount;
            }

            return discount;
        }
    }
}