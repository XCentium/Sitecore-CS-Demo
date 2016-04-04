#region

using System;
using System.Collections.Generic;
using System.Globalization;
using CSDemo.Contracts.Product;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Data.Items;
using System.Linq;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using Sitecore.Commerce.Entities.Inventory;

#endregion

namespace CSDemo.Models.Product
{
    public class ProductVariant : IProductVariant
    {
        #region Properties
        [SitecoreId]
        public virtual Guid ID { get; set; }
        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string Name { get; set; }

        [SitecoreInfo(SitecoreInfoType.Name)]
        public virtual string VariantId { get; set; }

        [SitecoreField(Fields.ListPrice)]
        public virtual string ListPrice { get; set; }

        [SitecoreField(Fields.Variant_Images)]
        public IEnumerable<Image> Variant_Images { get; set; }

        //public virtual IEnumerable<Image> Images { 
            
        //    get
        //    {
        //        if (this.Variant_Images == null) { return null; }

        //        return this.Variant_Images.Select(x => x.GlassCast<Image>());

        //    }
        
        //}

        [SitecoreField(Fields.ProductSize)]
        public virtual string ProductSize { get; set; }

        [SitecoreField(Fields.ProductColor)]
        public virtual string ProductColor { get; set; }

        public virtual StockInformation StockInformation { get; set; }
        public virtual string StockLabel { get; set; }
        public virtual int StockQuantity { get; set; }

        #endregion
        #region Fields
        public struct Fields
        {
            public const string VariantId = "VariantId";
            public const string ListPrice = "ListPrice";
            public const string Variant_Images = "Variant_Images";
            public const string ProductSize = "ProductSize";
            public const string ProductColor = "ProductColor";
        }
        #endregion 

    }
}