using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    public partial interface IBackpack : ISitecoreItem {
        
        #region Properties
IEnumerable<Item> Images { get; set; } 

DateTime IntroductionDate { get; set; } 

string ProductId { get; set; } 

string Brand { get; set; } 
#endregion
        
    }
}
