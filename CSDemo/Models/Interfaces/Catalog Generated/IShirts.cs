using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    public partial interface IShirts : ISitecoreItem {
        
        #region Properties
IEnumerable<Item> Images { get; set; } 

string Brand { get; set; } 

DateTime IntroductionDate { get; set; } 

string ProductId { get; set; } 
#endregion
        
    }
}
