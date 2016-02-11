using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.TemplateModels {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class SampleSearchItem : SitecoreItem, ISitecoreItem, ISampleSearchItem {
        
        #region Members
        public const string SitecoreItemTemplateId = "{E97B8435-B38B-4B28-8A7E-3521FDA7D465}";
        #endregion
    }
}
