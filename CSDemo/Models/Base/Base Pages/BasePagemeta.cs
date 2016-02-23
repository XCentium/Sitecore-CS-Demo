using System;
using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XCore.Framework.ItemMapper.Configuration.Attributes;
using XCore.Framework.ItemMapper;
namespace CSDemo.Models.Base.BasePages {
    
    
    [SitecoreItemTemplate(SitecoreItemTemplateId)]
    public partial class BasePagemeta : SitecoreItem, ISitecoreItem, IBasePagemeta {
        
        #region Members
        public const string SitecoreItemTemplateId = "{DC84F551-D6AD-46B4-80B0-2AB72BB0A419}";
        
        public const string AuthorFieldId = "{122FB360-2257-4B55-8353-0D146F0A1DE4}";
        
        public const string AuthorFieldName = "Author";
        
        public const string HideBreadcrumbFieldId = "{9C6DD02E-1660-43C6-99F4-AB0FE337FA09}";
        
        public const string HideBreadcrumbFieldName = "Hide Breadcrumb";
        
        public const string HidePageTitleFieldId = "{43192F21-7994-431C-8973-770D471893DE}";
        
        public const string HidePageTitleFieldName = "Hide Page Title";
        
        public const string MetaDescriptionFieldId = "{FBEFDCF0-C9DE-473A-AAB7-E043D2E335BA}";
        
        public const string MetaDescriptionFieldName = "Meta Description";
        
        public const string TitleFieldId = "{4D027FFA-64CF-4502-8C87-F5195FF18D76}";
        
        public const string TitleFieldName = "Title";
        #endregion
        
        #region Properties
[SitecoreItemField(AuthorFieldId)] 
 public virtual string Author { get; set; } 

[SitecoreItemField(HideBreadcrumbFieldId)] 
 public virtual bool HideBreadcrumb { get; set; } 

[SitecoreItemField(HidePageTitleFieldId)] 
 public virtual bool HidePageTitle { get; set; } 

[SitecoreItemField(MetaDescriptionFieldId)] 
 public virtual string MetaDescription { get; set; } 

[SitecoreItemField(TitleFieldId)] 
 public virtual string Title { get; set; } 
#endregion
        
    }
}
