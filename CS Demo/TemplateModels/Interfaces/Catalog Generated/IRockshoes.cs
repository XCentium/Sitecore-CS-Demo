using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Catalog_Generated {
    
    
    public partial interface IRockshoes : ISitecoreItem {
        
        #region Properties
DateTime IntroductionDate { get; set; } 

IEnumerable<Item> Images { get; set; } 

string ProductSize { get; set; } 

string ProductId { get; set; } 
#endregion
        
    }
}
