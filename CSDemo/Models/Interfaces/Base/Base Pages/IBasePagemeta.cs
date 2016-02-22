using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Base.BasePages {
    
    
    public partial interface IBasePagemeta : ISitecoreItem {
        
        #region Properties
string Author { get; set; } 

bool HideBreadcrumb { get; set; } 

bool HidePageTitle { get; set; } 

string MetaDescription { get; set; } 

string Title { get; set; } 
#endregion
        
    }
}
