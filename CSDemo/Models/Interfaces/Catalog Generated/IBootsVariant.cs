using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    public partial interface IBootsVariant : ISitecoreItem {
        
        #region Properties
string VariantId { get; set; } 

IEnumerable<Item> VariantImages { get; set; } 

string ProductColor { get; set; } 
#endregion
        
    }
}
