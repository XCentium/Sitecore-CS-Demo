
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
            var purchaseOutcomes = outcomes.OrderByDescending(o => o.DateTime).Where(o => o.DefinitionId == new ID(Constants.Commerce.PurchaseOutcomeId)).ToList();
            if (purchaseOutcomes == null || !purchaseOutcomes.Any()) return 0;

            var latestPurchaseOutcome = purchaseOutcomes.First();
            var latestPurchaseAmount = latestPurchaseOutcome.MonetaryValue;
            return latestPurchaseAmount;
        }
    }
}  
