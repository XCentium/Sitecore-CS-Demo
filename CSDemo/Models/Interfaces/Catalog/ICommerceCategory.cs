using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Catalog {
    
    
    public partial interface ICommerceCategory : ISitecoreItem {
        
        #region Properties
string ChildProducts { get; set; } 

string ChildCategories { get; set; } 
#endregion
        
    }
}
