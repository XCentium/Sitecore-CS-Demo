using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Cart
{
    public class CartLineItem
    {
        public string ProductId { get; set; }
        public uint Quantity { get; set; }
        public string CatalogName { get; set; }
        public string VariantId { get; set; }
    }
}