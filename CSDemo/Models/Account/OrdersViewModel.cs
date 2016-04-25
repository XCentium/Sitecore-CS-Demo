using System.Collections.Generic;

namespace CSDemo.Models.Account
{
    public class OrdersViewModel
    {
        public IEnumerable<OrderDetailViewModel> Orders { get; set; }
    }
}