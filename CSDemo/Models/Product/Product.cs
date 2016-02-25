using System;
using System.Collections.Generic;
using CSDemo.Contracts;
using CSDemo.Contracts.Product;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;

namespace CSDemo.Models.Product
{
    [SitecoreType(AutoMap = true)]
    public class Product : IProduct
    {
        #region Properties

        [SitecoreInfo(SitecoreInfoType.Name)]
        public virtual string Title { get; set; }

        [SitecoreField(Fields.ProductId)]
        public virtual string ProductId { get; set; }

        [SitecoreField(Fields.DateOfIntroduction)]
        public virtual DateTime DateOfIntroduction { get; set; }

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

        #endregion

        #region Fields

        public struct Fields
        {
            public const string Size = "Size";
            public const string Price = "List Price";
            public const string DefinitionName = "Definition Name";
            public const string Description = "Description";
            public const string Rating = "Rating";
            public const string ProductId = "Product Id";
            public const string Images = "Images";
            public const string CatalogName = "Catalog Name";
            public const string OnSale = "OnSale";
            public const string DateOfIntroduction = "Date of Introduction";
        }

        #endregion
    }
}