using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Contracts.Page {
    
    
    public partial interface IMetadata : ISitecoreItem {
        
        #region Properties
string MetaDescription { get; set; } 

string PageTitle { get; set; } 
#endregion
        
    }
}
