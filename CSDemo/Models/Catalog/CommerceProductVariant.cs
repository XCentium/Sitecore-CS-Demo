using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceProductVariant : SitecoreItem, ISitecoreItem, ICommerceProductVariant {
        
        #region Members
        public const string SitecoreItemTemplateId = "{C92E6CD7-7F14-46E7-BBF5-29CE31262EF4}";
        
        public const string ListPriceFieldId = "{9B2ABE41-AB16-463B-8845-A3A5D050A016}";
        
        public const string ListPriceFieldName = "ListPrice";
        #endregion
        
        #region Properties
[SitecoreItemField(ListPriceFieldId)] 
 public virtual string ListPrice { get; set; } 
#endregion
        
    }
}
