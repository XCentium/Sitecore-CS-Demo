using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Folders {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Sites : SitecoreItem, ISitecoreItem, ISites {
        
        #region Members
        public const string SitecoreItemTemplateId = "{26DC7B6F-9781-45CD-9F66-F13BDB0C1222}";
        #endregion
    }
}
