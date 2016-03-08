using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Commerce.Entities.Carts;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;

namespace CSDemo.Models.Account
{
    public class OrderDetailViewModel
    {
        public string OrderID { get; set; }
        public string OrderDate { get; set; }
        public int NumberofItems { get; set; }

        public string SubTotalPrice { get; set; }
        public string TotalPrice { get; set; }

        public string ShippingCost { get; set; }
        public string ShippingMethod { get; set; }
        public string PaymentMethod { get; set; }
        public string Tax { get; set; }
        public string Discount { get; set; }

        public string PaymentAuthorizationCode { get; set; }

        public string OrderStatus { get; set; }
        public string UserID { get; set; }
        public string ExternalID { get; set; }

        public IEnumerable<OrderLine> OrderLines { get; set; }

        public CommerceParty Shipping { get; set; }

        public CommerceParty Billing { get; set; }

    }
}