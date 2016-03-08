using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Account
{
    public class OrderLine
    {
        public string ProductID { get; set; }
        public string ImageUrl { get; set; }
        public string ProductName { get; set; }
        public string CSProductId { get; set; }
        public string UnitPrice { get; set; }
        public uint Quantity { get; set; }
        public string SubTotal { get; set; }
        public string ExternalID { get; set; }
        public string Category { get; set; }
        public string Url { get; set; }
    }
}