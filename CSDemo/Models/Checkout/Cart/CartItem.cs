#region

using System;
using System.Collections.Generic;

#endregion

namespace CSDemo.Models.Checkout.Cart
{
    [Serializable]
    public class CartItem
    {
        public string ProductId { get; set; }
        public string ImageUrl { get; set; }
        public string ProductName { get; set; }
        public string CsProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
        public string ExternalId { get; set; }
        public string Category { get; set; }
        public List<string> Adjustments { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
    }
}