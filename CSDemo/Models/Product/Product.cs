#region

using System;
using System.Collections.Generic;
using System.Globalization;
using CSDemo.Contracts;
using CSDemo.Contracts.Product;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Items;

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

        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string Title { get; set; }

        [SitecoreField(Fields.ProductId)]
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

        [SitecoreField(Fields.DefinitionName)]
        public virtual string DefinitionName { get; set; }

        [SitecoreField(Fields.Description)]
        public virtual string Description { get; set; }

        [SitecoreField(Fields.OnSale)]
        public virtual bool IsOnSale { get; set; }

        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }

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

        public IEnumerable<ProductVariant> ProductVariants { get; set; }

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
                return String.Format("{0:0.00}", Price);
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
            public const string OnSale = "OnSale";
            public const string ParentCategories = "ParentCategories";
            public const string DateOfIntroduction = "IntroductionDate";
            public const string SortFields = "Sort Fields";
            public const string ItemsPerPage = "Items Per Page";
            public const string Variants = "Variants";
            public const string Brand = "Brand";
            public const string ListPrice = "ListPrice";
            public const string FullDescription = "Full Description";
        }

        #endregion
    }
}