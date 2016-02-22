using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Pages {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class Page : SitecoreItem, ISitecoreItem, IPage {
        
        #region Members
        public const string SitecoreItemTemplateId = "{B3D2832F-2FBF-4466-9D95-D3A0C6476362}";
        
        public const string BodyFieldId = "{2F871D3E-628C-4472-A66B-8F2EFE84AAB5}";
        
        public const string BodyFieldName = "Body";
        
        public const string PageTitleFieldId = "{3230CBC1-3E03-43EC-95AB-C6E1505DE163}";
        
        public const string PageTitleFieldName = "Page Title";
        #endregion
        
        #region Properties
[SitecoreItemField(BodyFieldId)] 
 public virtual string Body { get; set; } 

[SitecoreItemField(PageTitleFieldId)] 
 public virtual string PageTitle { get; set; } 
#endregion
        
    }
}
