using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.CatalogGenerated {
    
    
    public partial interface ICarabiner : ISitecoreItem {
        
        #region Properties
string ProductId { get; set; } 

IEnumerable<Item> Images { get; set; } 

DateTime IntroductionDate { get; set; } 
#endregion
        
    }
}
