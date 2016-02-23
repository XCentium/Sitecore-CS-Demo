using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceCatalogFolder : SitecoreItem, ISitecoreItem, ICommerceCatalogFolder {
        
        #region Members
        public const string SitecoreItemTemplateId = "{334E2B54-F913-411D-B159-A7B16D65242C}";
        
        public const string SelectedCatalogsFieldId = "{ED3EBCCD-A230-4659-923F-014707E1945D}";
        
        public const string SelectedCatalogsFieldName = "Selected Catalogs";
        #endregion
        
        #region Properties
[SitecoreItemField(SelectedCatalogsFieldId)] 
 public virtual string SelectedCatalogs { get; set; } 
#endregion
        
    }
}
