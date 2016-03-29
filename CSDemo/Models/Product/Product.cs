#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using CSDemo.Contracts;
using CSDemo.Contracts.Product;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore;
using Sitecore.Commerce.Connect.CommerceServer.Catalog.Fields;
using Sitecore.Data.Items;
using Sitecore.Commerce.Entities.Inventory;
using Sitecore.Data;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Sitecore.Diagnostics;
using System.Net;
using Sitecore.Commerce.Connect.CommerceServer.Inventory.Models;
using Sitecore.Commerce.Connect.CommerceServer.Inventory;
using Sitecore.Commerce.Contacts;
using Sitecore.Commerce.Multishop;
using Sitecore.Commerce.Services.Inventory;
using Sitecore.Configuration;

#endregion

namespace CSDemo.Models.Product
{
    [SitecoreType(AutoMap = true)]
    public class Product : IProduct, IEditableBase
    {
        #region Properties

        [SitecoreId]
        public virtual Guid ID { get; set; }

        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }

        //[SitecoreInfo(SitecoreInfoType.DisplayName)]
        [SitecoreField(Fields.DisplayName)]
        public virtual string Title { get; set; }

        [SitecoreInfo(SitecoreInfoType.Name)]
        public virtual string ProductId { get; set; }

        [SitecoreField(Fields.DateOfIntroduction)]
        public virtual DateTime DateOfIntroduction { get; set; }

        [SitecoreField(Fields.FullDescription)]
        public virtual string FullDescription { get; set; }

        [SitecoreField(Fields.Size)]
        public virtual string Size { get; set; }

        [SitecoreField(Fields.Images)]
        public virtual IEnumerable<Image> Images { get; set; }

        [SitecoreField(Fields.CatalogName)]
        public virtual string CatalogName { get; set; }

        [SitecoreField(Fields.Price)]
        public virtual decimal Price { get; set; }

        public virtual decimal SalePrice { get; set; }

        [SitecoreField(Fields.DefinitionName)]
        public virtual string DefinitionName { get; set; }

        [SitecoreField(Fields.Description)]
        public virtual string Description { get; set; }

        public bool IsOnSale { get
            {
                return SalePrice > 0;
            }
        }

        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }

        [SitecoreField(Fields.IsNew)]
        public virtual bool IsNew { get; set; }

        [SitecoreField(Fields.ParentCategories)]
        public IEnumerable<Item> Categories { get; set; }

        [SitecoreField(Fields.Rating)]
        public virtual decimal Rating { get; set; }

        [SitecoreField(Fields.SortFields)]
        public IEnumerable<Item> SortFields { get; set; }

        [SitecoreField(Fields.ItemsPerPage)] 
        public virtual string ItemsPerPage { get; set; }

        [SitecoreField(Fields.Variants)]
        public virtual string Variants { get; set; }

        [SitecoreField(Fields.Brand)]
        public virtual string Brand { get; set; }

        public virtual Item FirstImage { get; set; }

        public virtual string DefaultVariant { get; set; }

        public IEnumerable<ProductVariant> ProductVariants { get; set; }

        public virtual VariantBox VariantBox { get; set; }

        public virtual VariantSize VariantSize { get; set; }

        public IEnumerable<VariantColor> VariantColors { get; set; }

        public IEnumerable<Product> AlsoBoughtProducts
        {
            get
            {
                //TODO: replace with web service call once the web service is fixed

                var url = string.Format("http://xcp13n.xcentium.net/api/data/relatedproducts/csdemo/{0}",this.ProductId);

                var syncClient = new WebClient();

                var response = syncClient.DownloadString(url);

                var result = JsonConvert.DeserializeObject<ComplementaryProductResult>(response);
                if (!result.IsSuccessful)
                {
                    if (result.Messages == null) yield return null;
                    foreach (var message in result.Messages)
                    {
                        Log.Warn(message, this);
                    }
                    yield return null;
                }
                if (result.ProductIds == null || !result.ProductIds.Any()) yield return null;
                foreach (var productId in result.ProductIds)
                {
                    var productResult = ProductHelper.GetItemByProductID(productId);
                    if (productResult != null)
                    {
                        var productItem = productResult.GetItem();
                        yield return productItem.GlassCast<Product>();
                    }
                }
            }
        }

        public IEnumerable<Product> RelatedProducts
        {
            get
            {
                List<Product> relatedProducts = new List<Product>();
                var contextProductItem = Context.Database.GetItem(new ID(ID));
                if (contextProductItem == null) return relatedProducts;
                RelationshipField control = contextProductItem.Fields[Product.Fields.RelationshipList];
                if (control == null) return relatedProducts;
                IEnumerable<Item> productRelationshipTargets = control.GetProductRelationshipsTargets();
                IEnumerable<Item> relationshipTargets = productRelationshipTargets as Item[] ?? productRelationshipTargets.ToArray();
                if (productRelationshipTargets == null || !relationshipTargets.Any()) return relatedProducts;
                relatedProducts.AddRange(relationshipTargets.Select(t => t.GlassCast<Product>()).Where(t => t != null));
                return relatedProducts;
            }
        }

        public StockInformation StockInformation { get; set;}

        public string CurrencyPrice
        {
            get
            {
                var info = (NumberFormatInfo) Sitecore.Context.Language.CultureInfo.NumberFormat.Clone();
                info.CurrencySymbol = Constants.Commerce.DefaultCurrencyCode;
                info.CurrencyPositivePattern = 3;
                return Price.ToString("C", info);
            }
        }

        public string ListPrice
        {
            get
            {
                var cultureInfo = Sitecore.Context.Culture;
                return Price.ToString("c", cultureInfo);
            }
        }

        public string VariantProdId
        {
            get
            {
                if (ProductId == null) return string.Empty;
                return string.Format(Constants.Products.VariantIDFormat, ProductId.Replace(Constants.Common.Dash,Constants.Common.Underscore));

            }
        }

        [DataType(DataType.EmailAddress)]
        public string VisitorSignupForStockNotificationEmail { get; set; }


        public static void VisitorSignupForStockNotification([NotNull] string shopName, NotificationSigneupInput model, string location)
        {
            Assert.ArgumentNotNull(shopName, "shopName");
            Assert.ArgumentNotNull(model, "model");
            Assert.ArgumentNotNullOrEmpty(model.ProductId, "model.ProductId");
            Assert.ArgumentNotNullOrEmpty(model.Email, "model.Email");

            var contactFactory = new ContactFactory();
            var visitorId = contactFactory.GetContact();
            var builder = new CommerceInventoryProductBuilder();
            CommerceInventoryProduct inventoryProduct = (CommerceInventoryProduct)builder.CreateInventoryProduct(model.ProductId);
            if (string.IsNullOrEmpty(model.VariantId))
            {
                (inventoryProduct).VariantId = model.VariantId;
            }

            if (string.IsNullOrEmpty(inventoryProduct.CatalogName))
            {
                (inventoryProduct).CatalogName = model.CatalogName;
            }

            DateTime interestDate;
            var isDate = DateTime.TryParse(model.InterestDate, out interestDate);
            var request = new VisitorSignUpForStockNotificationRequest(shopName, visitorId, model.Email, inventoryProduct) { Location = location };
            if (isDate)
            {
                request.InterestDate = interestDate;
            }

            var inventoryManager = new InventoryServiceProvider();
            var result = inventoryManager.VisitorSignUpForStockNotification(request);
            if (!result.Success)
            {
                foreach(var message in result.SystemMessages)
                {
                    Log.Error(message.Message, message);
                }
            }
        }


        #endregion

        #region Fields

        public struct Fields
        {
            public const string Size = "ProductSize";
            public const string Price = "ListPrice";
            public const string DefinitionName = "DefinitionName";
            public const string Description = "Description";
            public const string Rating = "Rating";
            public const string ProductId = "ProductId";
            public const string Images = "Images";
            public const string CatalogName = "CatalogName";
            public const string IsNew = "IsNew";
            public const string ParentCategories = "ParentCategories";
            public const string DateOfIntroduction = "IntroductionDate";
            public const string SortFields = "Sort Fields";
            public const string ItemsPerPage = "Items Per Page";
            public const string Variants = "Variants";
            public const string Brand = "Brand";
            public const string ListPrice = "ListPrice";
            public const string FullDescription = "Full Description";
            public const string RelationshipList = "Relationship List";
            public const string DisplayName = "__Display name";
        }

        #endregion
    }
}