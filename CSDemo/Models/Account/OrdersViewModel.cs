using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using System.Collections.Generic;
using System.Linq;

namespace CSDemo.Models.Account
{
	public class OrdersViewModel
	{
		public IEnumerable<OrderDetailViewModel> Orders { get; set; }
		public Dictionary<KeefePOC.Models.Inmate, List<OrderDetailViewModel>> GroupedOrders { get; set; }
	}
}