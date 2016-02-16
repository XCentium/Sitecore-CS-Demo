using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels.Fields {
    
    
    public partial interface ICommerceField : ISitecoreItem {
        
        #region Properties
bool IsRequired { get; set; } 

bool DisplayOnSite { get; set; } 

bool SpecificationSearchable { get; set; } 

bool FreeTextSearchable { get; set; } 

bool BaseProperty { get; set; } 

bool AssignToAllProductTypes { get; set; } 
#endregion
        
    }
}
