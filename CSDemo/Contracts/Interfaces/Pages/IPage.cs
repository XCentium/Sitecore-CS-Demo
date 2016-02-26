using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Pages {
    
    
    public partial interface IPage : ISitecoreItem {
        
        #region Properties
string Body { get; set; } 

string PageTitle { get; set; } 
#endregion
        
    }
}
