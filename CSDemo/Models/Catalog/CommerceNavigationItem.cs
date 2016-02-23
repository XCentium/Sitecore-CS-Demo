using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Catalog {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class CommerceNavigationItem : SitecoreItem, ISitecoreItem, ICommerceNavigationItem {
        
        #region Members
        public const string SitecoreItemTemplateId = "{E55834FB-7C93-44A2-87C0-62BEBA282CED}";
        
        public const string CategoryDatasourceFieldId = "{2882072B-E310-406B-8DD9-B22C9EA4A0F3}";
        
        public const string CategoryDatasourceFieldName = "CategoryDatasource";
        #endregion
        
        #region Properties
[SitecoreItemField(CategoryDatasourceFieldId)] 
 public virtual Sitecore.Data.Fields.DatasourceField CategoryDatasource { get; set; } 
#endregion
        
    }
}
