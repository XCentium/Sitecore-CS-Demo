using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    public partial interface IBackpackVariant : ISitecoreItem {
        
        #region Properties
string ProductColor { get; set; } 

IEnumerable<Item> VariantImages { get; set; } 

string VariantId { get; set; } 
#endregion
        
    }
}
