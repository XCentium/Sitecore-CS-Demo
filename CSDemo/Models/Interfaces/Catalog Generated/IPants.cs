using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    public partial interface IPants : ISitecoreItem {
        
        #region Properties
string Brand { get; set; } 

DateTime IntroductionDate { get; set; } 

IEnumerable<Item> Images { get; set; } 

string ProductId { get; set; } 
#endregion
        
    }
}