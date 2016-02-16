using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceProduct : SitecoreItem, ISitecoreItem, ICommerceProduct {
        
        #region Members
        public const string SitecoreItemTemplateId = "{225F8638-2611-4841-9B89-19A5440A1DA1}";
        
        public const string VariantsFieldId = "{4E3ACD15-B309-480F-8621-C873D82187A1}";
        
        public const string VariantsFieldName = "Variants";
        
        public const string RatingFieldId = "{AB768BEE-5592-498A-B490-15EDD94C7C9C}";
        
        public const string RatingFieldName = "Rating";
        
        public const string OnSaleFieldId = "{65509083-AB7F-4B4F-B4A1-EA353B1BAC3B}";
        
        public const string OnSaleFieldName = "OnSale";
        
        public const string DescriptionFieldId = "{7925E4C2-D1C5-493E-8559-A8106117048D}";
        
        public const string DescriptionFieldName = "Description";
        #endregion
        
        #region Properties
[SitecoreItemField(VariantsFieldId)] 
 public virtual string Variants { get; set; } 

[SitecoreItemField(RatingFieldId)] 
 public virtual string Rating { get; set; } 

[SitecoreItemField(OnSaleFieldId)] 
 public virtual bool OnSale { get; set; } 

[SitecoreItemField(DescriptionFieldId)] 
 public virtual string Description { get; set; } 
#endregion
        
    }
}
