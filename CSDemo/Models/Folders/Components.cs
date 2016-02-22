using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Folders {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Components : SitecoreItem, ISitecoreItem, IComponents {
        
        #region Members
        public const string SitecoreItemTemplateId = "{24216566-BD19-4EB9-B421-1CB7A97060E7}";
        #endregion
    }
}
