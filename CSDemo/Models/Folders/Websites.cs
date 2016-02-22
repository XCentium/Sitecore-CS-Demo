using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Folders {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Websites : SitecoreItem, ISitecoreItem, IWebsites {
        
        #region Members
        public const string SitecoreItemTemplateId = "{AF56D529-811B-4823-A8FA-3F52929614CB}";
        #endregion
    }
}
