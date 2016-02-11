using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels {
    
    
    public partial interface ISampleItem : ISitecoreItem {
        
        #region Properties
string Title { get; set; } 

string Text { get; set; } 
#endregion
        
    }
}
