using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog_Generated {
    
    
    public partial interface IGeneralCategory : ISitecoreItem {
        
        #region Properties
string Brand { get; set; } 

string Description { get; set; } 

IEnumerable<Item> Images { get; set; } 
#endregion
        
    }
}
