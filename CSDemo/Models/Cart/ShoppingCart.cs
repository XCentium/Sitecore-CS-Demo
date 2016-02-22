using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

    }
}