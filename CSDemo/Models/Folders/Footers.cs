using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Folders {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Footers : SitecoreItem, ISitecoreItem, IFooters {
        
        #region Members
        public const string SitecoreItemTemplateId = "{1731343D-8ECE-4477-B965-F62EF571672B}";
        #endregion
    }
}
