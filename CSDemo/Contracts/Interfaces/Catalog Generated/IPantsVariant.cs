using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    public partial interface IPantsVariant : ISitecoreItem {
        
        #region Properties
IEnumerable<Item> VariantImages { get; set; } 

string VariantId { get; set; } 

string ProductColor { get; set; } 

string ProductSize { get; set; } 
#endregion
        
    }
}
