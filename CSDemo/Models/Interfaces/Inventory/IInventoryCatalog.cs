using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Inventory {
    
    
    public partial interface IInventoryCatalog : ISitecoreItem {
        
        #region Properties
string InventoryCatalogName { get; set; } 

string DateCreated { get; set; } 

string InventoryCatalogDescription { get; set; } 

string LastModified { get; set; } 
#endregion
        
    }
}
