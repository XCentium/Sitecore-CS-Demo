using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Settings {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class AvailableCatalog : SitecoreItem, ISitecoreItem, IAvailableCatalog {
        
        #region Members
        public const string SitecoreItemTemplateId = "{058D0739-66F0-4029-AFA1-0B9630B47E10}";
        #endregion
    }
}
