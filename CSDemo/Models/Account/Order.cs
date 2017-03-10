using Sitecore.Analytics.Outcome;
using Sitecore.Analytics.Outcome.Model;
using Sitecore.Analytics.Tracking;
using Sitecore.Configuration;
using Sitecore.Data;
using System;
using System.Linq;

namespace CSDemo.Models.Account
{
    public class Order
    {
        public static decimal GetTotalAmountOrderedWithinPeriod(Contact contact, int days)
        {
            var manager = Factory.CreateObject("outcome/outcomeManager", true) as OutcomeManager;
            var outcomes = manager.GetForEntity<ContactOutcome>(new ID(contact.ContactId));
            if (outcomes == null || !outcomes.Any()) return 0;
            var purchaseOutcomes = outcomes.OrderByDescending(o => o.DateTime).Where(o => o.DefinitionId == new ID(Constants.Commerce.PurchaseOutcomeId)).ToList();
            if (purchaseOutcomes == null || !purchaseOutcomes.Any()) return 0;

            var recentPurchaseOutcomes = purchaseOutcomes.Where(o=>o.DateTime>= DateTime.Now.AddDays(-days));
            if (!recentPurchaseOutcomes.Any()) return 0;

            return recentPurchaseOutcomes.Sum(o => o.MonetaryValue);
        }

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
