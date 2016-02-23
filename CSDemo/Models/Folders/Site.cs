using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Folders {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Site : SitecoreItem, ISitecoreItem, ISite {
        
        #region Members
        public const string SitecoreItemTemplateId = "{D5E5858B-4BED-4666-88C0-6DE55E088CCC}";
        #endregion
    }
}
