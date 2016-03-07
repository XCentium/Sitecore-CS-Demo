using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Account
{
    public class OrderDetailViewModel
    {
        public string OrderID { get; set; }
        public string OrderDate { get; set; }
        public int NumberofItems { get; set; }
        public string TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public string UserID { get; set; }
        public string ExternalID { get; set; }

    }
}