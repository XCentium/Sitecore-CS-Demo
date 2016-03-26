
using CommerceServer.Core;
using CSDemo.Models.Checkout.Cart;
using Sitecore.Analytics.Outcome;
using Sitecore.Analytics.Outcome.Model;
using Sitecore.Analytics.Tracking;
using Sitecore.Commerce.Connect.CommerceServer;
using Sitecore.Commerce.Connect.CommerceServer.Orders;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Commerce.Contacts;
using Sitecore.Commerce.Services.Orders;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Sites;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Account
{
    public class Order
    {
        public static decimal GetLastOrderAmount(Contact contact)
        {
            var manager = Factory.CreateObject("outcome/outcomeManager", true) as OutcomeManager;
            var outcomes = manager.GetForEntity<ContactOutcome>(new ID(contact.ContactId));
            if (outcomes == null || !outcomes.Any()) return 0;
            var purchaseOutcomes = outcomes.OrderByDescending(o => o.DateTime).Where(o => o.Id == new ID(Constants.Commerce.PurchaseOutcomeId)).ToList();
            if (purchaseOutcomes == null || !purchaseOutcomes.Any()) return 0;

            var latestPurchaseOutcome = purchaseOutcomes.First();
            var latestPurchaseAmount = latestPurchaseOutcome.MonetaryValue;
            return latestPurchaseAmount;
        }

        public static CommerceOrder GetMostRecentOrder(Contact contact)
        {
            var contextManager = new CommerceServerContextManager();  //using Sitecore.Commerce.Connect.CommerceServer;
            var orderManagementContext = contextManager.OrderManagementContext;
            var orderManager = orderManagementContext.PurchaseOrderManager;

            CultureInfo culture = new CultureInfo("en-US");
            DataSet searchableProperties = orderManager.GetSearchableProperties(culture.ToString());
            SearchClauseFactory searchClauseFactory = orderManager.GetSearchClauseFactory(searchableProperties, "PurchaseOrder"); //using CommerceServer.Core; Assembly CommerceServer.Core.CrossTier
            SearchClause searchClause = searchClauseFactory.CreateClause(ExplicitComparisonOperator.OnOrAfter, "Created", DateTime.Now.AddMonths(-6));

            SearchOptions options = new SearchOptions();
            options.SetPaging(10, 1);

            var result = orderManager.SearchPurchaseOrders(searchClause, options);


            using (new SiteContextSwitcher(Sitecore.Configuration.Factory.GetSite("XCentiumCSDemo")))
            {
                try
                {

                    var contactFactory = new ContactFactory();
                    var userName = contactFactory.GetUserId(contact);
                    var user = Sitecore.Security.Accounts.User.FromName(userName, true);
                    using (new UserSwitcher(user))
                    {

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
                }
                catch (NullReferenceException ex)
                {
                    //do nothing; commerce server throws an exception for an unknown user or no orders
                    return null;
                }


            }
        }
    }
}