using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceCategory : SitecoreItem, ISitecoreItem, ICommerceCategory {
        
        #region Members
        public const string SitecoreItemTemplateId = "{4C4FD207-A9F7-443D-B32A-50AA33523661}";
        
        public const string ChildProductsFieldId = "{95F37041-A3F4-4FA2-8C3C-7A3DB52AAC75}";
        
        public const string ChildProductsFieldName = "ChildProducts";
        
        public const string ChildCategoriesFieldId = "{90BC8026-5DA2-4AAC-9330-3286CFD80EC7}";
        
        public const string ChildCategoriesFieldName = "ChildCategories";
        #endregion
        
        #region Properties
[SitecoreItemField(ChildProductsFieldId)] 
 public virtual string ChildProducts { get; set; } 

[SitecoreItemField(ChildCategoriesFieldId)] 
 public virtual string ChildCategories { get; set; } 
#endregion
        
    }
}
