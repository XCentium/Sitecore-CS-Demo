using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog {
    
    
    public partial interface ICommerceCatalogFolder : ISitecoreItem {
        
        #region Properties
string SelectedCatalogs { get; set; } 
#endregion
        
    }
}
