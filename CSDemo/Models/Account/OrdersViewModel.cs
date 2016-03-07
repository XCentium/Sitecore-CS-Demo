using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Account
{
    public class OrdersViewModel
    {
        public IEnumerable<OrderDetailViewModel> Orders { get; set; }
    }
}