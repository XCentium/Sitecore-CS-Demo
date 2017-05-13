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
    public class ProductSampler : IEditableBase
    {
        #region AutoMapped Properties

        [SitecoreId]
        public virtual Guid ID { get; set; }

        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }

        [SitecoreField(Fields.Name), DataMember]
        public virtual string Title { get; set; }

        [SitecoreField(Fields.Price), DataMember]
        public virtual double Price { get; set; }

        [SitecoreField(Fields.Image), DataMember]
        public virtual Image Image { get; set; }

        [SitecoreField(Fields.Group1Products)]
        public virtual IEnumerable<Product> Group1Products { get; set; }

        [SitecoreField(Fields.Group2Products)]
        public virtual IEnumerable<Product> Group2Products { get; set; }

        [SitecoreField(Fields.Group3Products)]
        public virtual IEnumerable<Product> Group3Products { get; set; }

        #endregion

        #region Fieldname Mappings

        public struct Fields
        {
            public const string Name = "Name";
            public const string Price = "Price";
            public const string Image = "Image";
            public const string Group1Products = "Group1Products";
            public const string Group2Products = "Group2Products";
            public const string Group3Products = "Group3Products";
        }

        #endregion
    }
}