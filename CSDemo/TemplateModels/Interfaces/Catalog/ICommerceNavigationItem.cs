using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog {
    
    
    public partial interface ICommerceNavigationItem : ISitecoreItem {
        
        #region Properties
Sitecore.Data.Fields.DatasourceField CategoryDatasource { get; set; } 
#endregion
        
    }
}
