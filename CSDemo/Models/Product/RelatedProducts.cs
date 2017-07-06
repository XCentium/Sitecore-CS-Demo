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
    public class RelatedProductsFallback : IEditableBase
    {
        #region AutoMapped Properties

        [SitecoreId]
        public virtual Guid ID { get; set; }

        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }

        [SitecoreField(Fields.RelatedProducts)]
        public virtual IEnumerable<Product> RelatedProducts { get; set; }

        [SitecoreField(Fields.AlsoBoughtProducts)]
        public virtual IEnumerable<Product> AlsoBoughtProducts { get; set; }

        [SitecoreField(Fields.FrequentlyBoughtTogetherProducts)]
        public virtual IEnumerable<Product> FrequentlyBoughtTogetherProducts { get; set; }

        [SitecoreField(Fields.Override)]
        public virtual bool Override { get; set; }

        #endregion

        #region Fieldname Mappings

        public struct Fields
        {
            public const string RelatedProducts = "RelatedProducts";
            public const string AlsoBoughtProducts = "AlsoBoughtProducts";
            public const string FrequentlyBoughtTogetherProducts = "FrequentlyBoughtTogetherProducts";
            
            public const string Override = "Override";
        }

        #endregion
    }
}