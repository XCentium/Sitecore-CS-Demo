#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using CSDemo.Contracts;
using CSDemo.Contracts.Product;
using CSDemo.Helpers;
using CSDemo.Models.Checkout.Cart;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Newtonsoft.Json;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Analytics.Automation.Data;
using Sitecore.Analytics.Automation.MarketingAutomation;
using Sitecore.Commerce.Connect.CommerceServer.Catalog.Fields;
using Sitecore.Commerce.Connect.CommerceServer.Inventory;
using Sitecore.Commerce.Connect.CommerceServer.Inventory.Models;
using Sitecore.Commerce.Contacts;
using Sitecore.Commerce.Entities.Inventory;
using Sitecore.Commerce.Services.Inventory;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Log = Sitecore.Diagnostics.Log;

#endregion

namespace CSDemo.Models.Product
{
    [SitecoreType(AutoMap = true), DataContract]
    public class Product : IProduct, IEditableBase
    {
        #region Calculated Properties

        public IEnumerable<ProductVariant> ProductVariants { get; set; }

        public IEnumerable<VariantColor> VariantColors { get; set; }

        [SitecoreField(Fields.UnitOfMeasure)]
        public virtual IEnumerable<UnitOfMeasure> Units { get; set; }

        [SitecoreField(Fields.SortId), DataMember]
        public virtual int SortId { get; set; }

        public IEnumerable<Product> AlsoBoughtProducts
        {
            get
            {
                var response = string.Empty;

                try
                {
                    var url = string.Format(Constants.Products.AlsoBoughtProductsUrl, ProductId);

                    var syncClient = new WebClient();

                    response = syncClient.DownloadString(url);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.Message, ex, this);
                }

                if (string.IsNullOrEmpty(response)) yield return null;

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
                    var productResult = ProductHelper.GetItemByProductId(productId);
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
                RelationshipField control = contextProductItem.Fields[Fields.RelationshipList];
                if (control == null) return relatedProducts;
                IEnumerable<Item> productRelationshipTargets = control.GetProductRelationshipsTargets();
                IEnumerable<Item> relationshipTargets = productRelationshipTargets as Item[] ?? productRelationshipTargets.ToArray();
                if (productRelationshipTargets == null || !relationshipTargets.Any()) return relatedProducts;
                relatedProducts.AddRange(relationshipTargets.Select(t => t.GlassCast<Product>()).Where(t => t != null));
                return relatedProducts;
            }
        }

        public StockInformation StockInformation { get; set; }

        public string CurrencyPrice
        {
            get
            {
                var info = (NumberFormatInfo)Context.Language.CultureInfo.NumberFormat.Clone();
                info.CurrencySymbol = Constants.Commerce.DefaultCurrencyCode;
                info.CurrencyPositivePattern = 3;
                if (Context.User.IsInRole("CommerceUsers\\Dealer")) { return Price > 0 ? ((decimal)0.90 * Price).ToString("C", info) : Price.ToString("C", info); }
                if (Context.User.IsInRole("CommerceUsers\\Retailer")) { return Price > 0 ? ((decimal)0.75 * Price).ToString("C", info) : Price.ToString("C", info); }
                return Price.ToString("C", info);
            }
        }

        public string ListPrice
        {
            get
            {
                var cultureInfo = Context.Culture;
                if (Context.User.IsInRole("CommerceUsers\\Dealer")) { return Price > 0 ? ((decimal)0.90 * Price).ToString("c", cultureInfo) : Price.ToString("c", cultureInfo); }
                if (Context.User.IsInRole("CommerceUsers\\Retailer")) { return Price > 0 ? ((decimal)0.75 * Price).ToString("c", cultureInfo) : Price.ToString("c", cultureInfo); }
                return Price.ToString("c", cultureInfo);
            }
        }

        public string VariantProdId
        {
            get
            {
                if (ProductId == null) return string.Empty;
                return string.Format(Constants.Products.VariantIdFormat, ProductId.Replace(Constants.Common.Dash, Constants.Common.Underscore));

            }
        }

        public string LocationName { get; set; }

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
                foreach (var message in result.SystemMessages)
                {
                    Log.Error(message.Message, message);
                }
            }
        }

        public static void AddUserVisitorSignupForStockNotification(string user, Item engagementPlan)
        {
            Tracker.Current.Session.Identify(user);
            AutomationStateManager manager = Tracker.Current.Session.CreateAutomationStateManager();
            manager.EnrollInEngagementPlan(engagementPlan.ID, engagementPlan.Children.First().ID);
        }

        public List<string> AltImages()
        {
            var altImages = new List<string>();

            if (!string.IsNullOrEmpty(Image1)) { altImages.Add(Image1); }
            if (!string.IsNullOrEmpty(Image2)) { altImages.Add(Image2); }
            if (!string.IsNullOrEmpty(Image3)) { altImages.Add(Image3); }

            return altImages;
        }

        [SitecoreField(Fields.Stores)]
        public IEnumerable<Store.Store> Stores { get; set; }

        #endregion

        #region AutoMapped Properties

        [SitecoreId]
        public virtual Guid ID { get; set; }

        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }

       
        //[SitecoreInfo(SitecoreInfoType.DisplayName)]
        [SitecoreField(Fields.DisplayName), DataMember]
        public virtual string Title { get; set; }

        [SitecoreInfo(SitecoreInfoType.Name), DataMember]
        public virtual string ProductId { get; set; }

        [SitecoreField(Fields.DateOfIntroduction), DataMember]
        public virtual DateTime DateOfIntroduction { get; set; }

        [SitecoreField(Fields.FullDescription)]
        public virtual string FullDescription { get; set; }

        [SitecoreField(Fields.MoreInfo)]
        public virtual string MoreInfo { get; set; }

        [SitecoreField(Fields.Size), DataMember]
        public virtual string Size { get; set; }

        [SitecoreField(Fields.Images), DataMember]
        public virtual IEnumerable<Image> Images { get; set; }

        [SitecoreField(Fields.CatalogName), DataMember]
        public virtual string CatalogName { get; set; }

        [SitecoreField("weight")]
        public virtual decimal Weight { get; set; }

        [SitecoreField(Fields.Price), DataMember]
        public virtual decimal Price { get; set; }

        [DataMember]
        public virtual decimal SalePrice { get; set; }

        [SitecoreField(Fields.DefinitionName), DataMember]
        public virtual string DefinitionName { get; set; }

        [SitecoreField(Fields.Description), DataMember]
        public virtual string Description { get; set; }

        [DataMember]
        public bool IsOnSale
        {
            get
            {
                return SalePrice > 0;
            }
        }

        [SitecoreInfo(SitecoreInfoType.Url), DataMember]
        public virtual string Url { get; set; }

        [SitecoreField(Fields.IsNew), DataMember]
        public virtual bool IsNew { get; set; }

        [SitecoreField(Fields.ParentCategories), DataMember]
        public IEnumerable<Item> Categories { get; set; }

        [SitecoreField(Fields.Rating), DataMember]
        public virtual decimal Rating { get; set; }

        [SitecoreField(Fields.SortFields)]
        public IEnumerable<Item> SortFields { get; set; }

        [SitecoreField(Fields.ItemsPerPage)]
        public virtual string ItemsPerPage { get; set; }

        [SitecoreField(Fields.Variants), DataMember]
        public virtual string Variants { get; set; }

        [SitecoreField(Fields.Brand), DataMember]
        public virtual string Brand { get; set; }

        public virtual string FirstImage
        {

            get
            {

                return Images.ElementAt(0).Src;
            }

        }

        public virtual string DefaultVariant { get; set; }

        [DataMember]
        public virtual VariantBox VariantBox { get; set; }

        [DataMember]
        public virtual VariantSize VariantSize { get; set; }

        [SitecoreField(Fields.UnitOfMeasure)]
        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        [SitecoreField(Fields.ProductTags)]
        public virtual IEnumerable<ProductTag> ProductTags { get; set; }

        [SitecoreField(Fields.Image1)]
        public virtual string Image1 { get; set; }

        [SitecoreField(Fields.Image2)]
        public virtual string Image2 { get; set; }

        [SitecoreField(Fields.Image3)]
        public virtual string Image3 { get; set; }

        public virtual string MetalType { get; set; }

        public virtual string KaratPurity { get; set; }

        public virtual string Width { get; set; }

        public virtual string Style { get; set; }

        public virtual string MetalColor { get; set; }

        public virtual string InsideDimension { get; set; }

        public virtual string FabricationMethod { get; set; }

        public virtual string ShippingWeight { get; set; }

        public virtual string CountryOfOrigin { get; set; }
        public virtual string Thickness { get; set; }

        [SitecoreField(Fields.SquareFeetPerSheet)]
        public virtual string SquareFeetPerSheet { get; set; }
        public virtual string Finish { get; set; }


        [SitecoreField(Fields.XsPrice), DataMember]
        public virtual decimal XsPrice { get; set; }

        [SitecoreField(Fields.Color)]
        public virtual string Color { get; set; }
        public virtual string StoneColor { get; set; }
        public virtual string GlassColor { get; set; }

        public virtual string Artist { get; set; }
        public virtual string Song { get; set; }
        public virtual string Genre { get; set; }
        public virtual string Album { get; set; }
        public virtual string Length { get; set; }

        #endregion

        #region Fieldname Mappings

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
            public const string MoreInfo = "More Info";
            public const string RelationshipList = "Relationship List";
            public const string DisplayName = "__Display name";
            public const string UnitOfMeasure = "Unit Of Measure";
            public const string ProductTags = "Product Tags";
            public const string Image1 = "Image1";
            public const string Image2 = "Image2";
            public const string Image3 = "Image3";
            public const string Thickness = "Thickness";
            public const string SquareFeetPerSheet = "Square Feet Per Sheet";
            public const string Finish = "Finish";
            public const string XsPrice = "XSPrice";
            public const string Color = "ProductColor";
            public const string StoneColor = "StoneColor";
            public const string GlassColor = "GlassColor";
            public const string Stores = "Store Location";
            public const string SortId = "SortId";
        }

        #endregion

        #region Helpers

        public static Product GetProduct(string productId)
        {
            var product = new Product();
            var cartHelper = new CartHelper();
            var productResult = ProductHelper.GetItemByName(productId);
            if (productResult != null)
            {
                var productItem = productResult.GetItem();
                product = productItem.GlassCast<Product>();
                var productVariants = productItem.GetChildren().Select(x => x.GlassCast<ProductVariant>()).ToList();
                // Update Images and stockInfo in ProductVariant
                if (productVariants.Any())
                {
                    var theVariants = new List<ProductVariant>();
                    for (var i = 0; i < productVariants.Count(); i++)
                    {
                        productVariants[i] = ProductHelper.UpdateVariantProperties(productVariants[i], product, cartHelper);
                        theVariants.Add(productVariants[i]);
                    }
                    product.ProductVariants = theVariants;
                }
                if (productItem.HasChildren)
                {
                    ProductHelper.BuildUiVariants(product);
                }


                if (!string.IsNullOrEmpty(product.DefaultVariant))
                {
                    product.StockInformation = cartHelper.GetProductStockInformation(product.ProductId,
                        product.CatalogName, product.DefaultVariant);
                }
                else
                {
                    product.StockInformation = cartHelper.GetProductStockInformation(product.ProductId,
                        product.CatalogName);
                }
            }
            return product;

        }


        public static IEnumerable<Product> GetGeoTargetedProducts(string zipCode)
        {
            if (zipCode == "N/A")
            {
                zipCode = "90292";
                Log.Warn("Using default sample zip 90292.", zipCode);
            }
            Log.Info(string.Format("The zip code is set to {0}.", zipCode), zipCode);
            if (string.IsNullOrWhiteSpace(zipCode)) yield return null;
            var response = string.Empty;

            try
            {
                var url = string.Format(Constants.Products.GeoTargetedProductsUrl, zipCode);

                var syncClient = new WebClient();

                response = syncClient.DownloadString(url);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
            }

            if (string.IsNullOrEmpty(response)) yield return null;

            var result = JsonConvert.DeserializeObject<ComplementaryProductResult>(response);

            if (result != null)
            {
                if (!result.IsSuccessful)
                {
                    if (result.Messages == null)
                    {
                        yield return null;
                    }
                    else { 
                        foreach (var message in result.Messages)
                        {
                            Log.Warn(message, result);
                        }
                    }
                }

                if (result.ProductIds == null || !result.ProductIds.Any())
                {
                    yield return null;
                }
                else
                {
                    foreach (var productId in result.ProductIds)
                    {
                        var productResult = ProductHelper.GetItemByProductId(productId);

                        if (productResult == null) continue;

                        var productItem = productResult.GetItem();
                        yield return GlassHelper.Cast<Product>(productItem);
                    }
                }
            }
            else
            {
                yield return null;
            }
        }
        #endregion
    }
}