using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    public partial interface IShirtsVariant : ISitecoreItem {
        
        #region Properties
string ProductSize { get; set; } 

IEnumerable<Item> VariantImages { get; set; } 

string ProductColor { get; set; } 

string VariantId { get; set; } 
#endregion
        
    }
}
