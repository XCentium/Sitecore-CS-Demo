using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Contracts.Navigation {
    
    
    public partial interface INavigationLink : ISitecoreItem {
        
        #region Properties
LinkField Link { get; set; } 

Item MegaLink { get; set; } 
#endregion
        
    }
}
