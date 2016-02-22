using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Configurations {
    
    
    public partial interface ISiteHeader : ISitecoreItem {
        
        #region Properties
string Tagline { get; set; } 

string Title { get; set; } 

string PreTitle { get; set; } 

string PhoneNumber { get; set; } 

string email { get; set; } 
#endregion
        
    }
}
