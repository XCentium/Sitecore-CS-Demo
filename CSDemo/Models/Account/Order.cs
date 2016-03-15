using CSDemo.Models.Checkout.Cart;
using Sitecore.Analytics.Tracking;
using Sitecore.Commerce.Connect.CommerceServer.Orders;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Contacts;
using Sitecore.Commerce.Services.Orders;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Account
{
    public class Order
    {
        public static CommerceOrder GetMostRecentOrder(Contact contact)
        {
            try
            {
                var contactFactory = new ContactFactory();
                var userName = contactFactory.GetUserId(contact);
                if (string.IsNullOrWhiteSpace(userName)) return null;
                var userId = (new AccountHelper()).GetCommerceUserID(userName);
                if (string.IsNullOrWhiteSpace(userId)) return null;
                var submitRequest = new GetVisitorOrdersRequest(userId, "XCentiumCSDemo");
                var provider = new CommerceOrderServiceProvider();
                var orders = provider.GetVisitorOrders(submitRequest);
                var commerceOrders = new List<CommerceOrder>();
                if (orders != null)
                {
                    var cartHelper = new CartHelper();
                    foreach (var order in orders.OrderHeaders)
                    {
                        var orderRequest = new GetVisitorOrderRequest(order.OrderID, order.CustomerId, "XCentiumCSDemo");
                        if (orderRequest == null) continue;
                        var orderResult = provider.GetVisitorOrder(orderRequest);
                        if (orderResult == null) continue;
                        var commerceOrderHead = orderResult.Order as CommerceOrder;
                        if (commerceOrderHead == null) continue;

                        commerceOrders.Add(commerceOrderHead);
                    }
                }

                var mostRecentOrder = commerceOrders.OrderByDescending(o => o.Created).First();
                return mostRecentOrder;
            }
            catch (NullReferenceException)
            {
                //do nothing; commerce server throws an exception for an unknown user or no orders
                return null;
            }

        }
    }
}