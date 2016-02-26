#region

using System;

#endregion

namespace CSDemo.Models.Cart
{
    [Serializable]
    public class CartItem
    {
        public string ProductID { get; set; }
        public string ImageUrl { get; set; }
        public string ProductName { get; set; }
        public string CSProductId { get; set; }
        public Decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public Decimal SubTotal { get; set; }
        public string ExternalID { get; set; }
    }
}