using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Catalog {
    
    
    public partial interface ICommerceProduct : ISitecoreItem {
        
        #region Properties
string Variants { get; set; } 

string Rating { get; set; } 

bool OnSale { get; set; } 

string Description { get; set; } 
#endregion
        
    }
}
