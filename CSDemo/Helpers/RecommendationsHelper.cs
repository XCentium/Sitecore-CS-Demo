﻿using System;
using System.Collections.Generic;
using System.Linq;
using CSDemo.Configuration;
using CSDemo.Models.Account;
using CSDemo.Models.Page;
using CSDemo.Models.Product;
using CSDemo.Models.Recommendations;
using CSDemo.Services;
using Sitecore.Commerce.Connect.CommerceServer.Orders.Models;
using Sitecore.Diagnostics;

namespace CSDemo.Helpers
{
    public class RecommendationsHelper
    {
        public static Recommendations GetFrequentlyBoughtTogetherRecommendations(string itemId, int numberOfResults)
        {
            var recommendations = GetItemToItemRecommendations(itemId, numberOfResults, GetSiteRecommendationsApiFbtBuildId());
            recommendations.Type = RecommendationType.Fbt;

            return recommendations;
        }

        public static Recommendations GetItemRecommendations(string itemId, int numberOfResults)
        {
            var recommendations = GetItemToItemRecommendations(itemId, numberOfResults, GetSiteRecommendationsApiBuildId());
            recommendations.Type = RecommendationType.ItemToItem;

            return recommendations;
        }

        public static List<ProductMini> GetUserRecommendations(string userId, int numberOfResults)
        {
            var itemRecommendations = new List<ProductMini>();

            var recommendations = GetUserToItemRecommendations(userId, numberOfResults, GetSiteRecommendationsApiBuildId());

            if (recommendations?.ItemsIds == null || recommendations.ItemsIds.Count <= 0)
            {
                return itemRecommendations;
            }

            return recommendations.ItemsIds.Select(GetProductFromApiProductId).ToList();
        }

        public static List<ProductMini> GetItemRecommendations(string productId, string variantId, int numberOfResults)
        {
            variantId = string.IsNullOrWhiteSpace(variantId) ? "0" : variantId;

            var itemId = $"{productId}_{variantId}";
            var itemRecommendations = new List<ProductMini>();

            var recommendations = GetItemRecommendations(itemId, numberOfResults);

            if (recommendations?.ItemsIds == null || recommendations.ItemsIds.Count <= 0)
            {
                return itemRecommendations;
            }

            return recommendations.ItemsIds.Select(GetProductFromApiProductId).ToList();
        }

        internal static List<ProductMini> GetFrequentlyBoughtTogetherRecommendations(string productId, string variantId, int numberOfResults)
        {
            variantId = string.IsNullOrWhiteSpace(variantId) ? "0" : variantId;

            var itemId = $"{productId}_{variantId}";
            var itemRecommendations = new List<ProductMini>();

            var recommendations = GetFrequentlyBoughtTogetherRecommendations(itemId, numberOfResults);

            if (recommendations?.ItemsIds == null || recommendations.ItemsIds.Count <= 0)
            {
                return itemRecommendations;
            }

            return recommendations.ItemsIds.Select(GetProductFromApiProductId).ToList();
        }

        private static ProductMini GetProductFromApiProductId(string apiProductId)
        {
            var itemArray = apiProductId.Split('_');

            var searchResultItem = ProductHelper.GetItemByProductId(itemArray[0]);

            var item = searchResultItem?.GetItem();

            if (item == null)
                return null;

            var product = GlassHelper.Cast<Product>(item);

            return new ProductMini
            {
                SalePrice = product.SalePrice,
                Price = product.Price,
                Guid = product.ID.ToString(),
                Title = product.Title,
                IsOnSale = product.IsOnSale,
                Id = product.ProductId,
                CatalogName = product.CatalogName,
                VariantId = product.VariantProdId,
                ImageSrc = product.FirstImage,
                Url = $"{product.Url}?v={itemArray[1]}",
                Rating = product.Rating
            };
        }

        private static Recommendations GetItemToItemRecommendations(string itemId, int numberOfResults, string buildId)
        {
            var recommendations = new Recommendations { RecommendationForItemId = itemId };

            var api = new RecommendationsApi(ConfigurationHelper.GetRecommendationsApiKey(),
                ConfigurationHelper.GetRecommendationsApiBaseUri());

            var items = api.GetRecommendations(GetSiteRecommendationsApiModelId(), buildId,
                itemId, numberOfResults);

            foreach (var recoSet in items.RecommendedItemSetInfo)
            {
                foreach (var item in recoSet.Items)
                {
                    recommendations.ItemsIds.Add(item.Id);
                }
            }

            return recommendations;
        }

        private static Recommendations GetUserToItemRecommendations(string userId, int numberOfResults, string buildId)
        {
            var recommendations = new Recommendations { RecommendationForUserId = userId };

            var api = new RecommendationsApi(ConfigurationHelper.GetRecommendationsApiKey(),
                ConfigurationHelper.GetRecommendationsApiBaseUri());

            var items = api.GetUserRecommendations(GetSiteRecommendationsApiModelId(), buildId,
                userId, numberOfResults);

            foreach (var recoSet in items.RecommendedItemSetInfo)
            {
                foreach (var item in recoSet.Items)
                {
                    recommendations.ItemsIds.Add(item.Id);
                }
            }

            return recommendations;
        }

        private static string GetSiteRecommendationsApiFbtBuildId()
        {
            var root = GetSiteRoot();

            return root.RecommendationApiFbtBuildId;
        }

        private static string GetSiteRecommendationsApiBuildId()
        {
            var root = GetSiteRoot();

            return root.RecommendationApiBuildId;
        }

        private static string GetSiteRecommendationsApiModelId()
        {
            var root = GetSiteRoot();

            return root.RecommendationApiModelId;
        }

        internal static Root GetSiteRoot()
        {
            // Fetch the start item from Site definition

            var rootItem = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.ContentStartPath);

            return rootItem == null ? null : GlassHelper.Cast<Root>(rootItem);
        }

        //public static void SendPurchaseEvent(CommerceOrder order)
        //{
        //    try
        //    {
        //        var api = new RecommendationsApi(ConfigurationHelper.GetRecommendationsApiKey(),
        //            ConfigurationHelper.GetRecommendationsApiBaseUri());

        //        var customerId = AccountHelper.GetCommerceUserId();
        //        var modelId = GetSiteRecommendationsApiModelId();

        //        foreach (var line in order.Lines)
        //        {
        //            var product = (CommerceCartProduct)line.Product;
        //            var price = (CommercePrice)line.Product.Price;

        //            api.SendEventUpdate(modelId, GetSiteRecommendationsApiBuildId(), product.ProductId, product.ProductVariantId, line.Quantity, price.ListPrice, customerId, order.Created, RecommendationsApiEventType.Purchase);
        //            api.SendEventUpdate(modelId, GetSiteRecommendationsApiFbtBuildId(), product.ProductId, product.ProductVariantId, line.Quantity, price.ListPrice, customerId, order.Created, RecommendationsApiEventType.Purchase);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error($"RecommendationsHelper.SendPurchaseEvent. Error = {e.Message}", e);
        //    }
        //}
    }
}