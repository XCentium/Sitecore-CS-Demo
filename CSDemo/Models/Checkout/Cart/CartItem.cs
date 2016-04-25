#region

using System;

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
        public Decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Decimal SubTotal { get; set; }
        public string ExternalId { get; set; }
        public string Category { get; set; }
    }
}