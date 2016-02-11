using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog {
    
    
    public partial interface ICommerceStaticCatalogGroup : ISitecoreItem {
        
        #region Properties
string Description { get; set; } 

string IncludedCatalogs { get; set; } 
#endregion
        
    }
}
